using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ChatbotModule.Configuration;

namespace ChatbotModule.AI
{
	public sealed class OpenAiClient : IAiClient
	{
		private readonly HttpClient _httpClient;
		private readonly ChatbotOptions _options;
		private readonly ILogger<OpenAiClient> _logger;

		public OpenAiClient(HttpClient httpClient, IOptions<ChatbotOptions> options, ILogger<OpenAiClient> logger)
		{
			_httpClient = httpClient;
			_options = options.Value;
			_logger = logger;
		}

		public async Task<AiCompletionResult> CompleteAsync(AiCompletionRequest request, CancellationToken cancellationToken = default)
		{
			var apiKey = _options.OpenAiApiKey;
			var endpoint = "https://api.openai.com/v1/chat/completions";
			var defaultModel = "gpt-4o-mini";

			if (string.IsNullOrWhiteSpace(apiKey))
			{
				apiKey = _options.GeminiApiKey;
				if (!string.IsNullOrWhiteSpace(apiKey))
				{
					endpoint = "https://generativelanguage.googleapis.com/v1beta/openai/chat/completions";
					defaultModel = "gemini-2.5-flash";
				}
				else
				{
					return AiCompletionResult.Fail("AI provider is not configured. Set Chatbot:OpenAiApiKey or Chatbot:GeminiApiKey.");
				}
			}

			var messages = new List<object>();
			if (!string.IsNullOrWhiteSpace(request.SystemPrompt))
			{
				messages.Add(new { role = AiRoles.System, content = request.SystemPrompt });
			}
			foreach (var message in request.Messages)
			{
				messages.Add(new { role = message.Role, content = message.Content });
			}

			if (messages.Count == 0)
			{
				return AiCompletionResult.Fail("AI request contained no messages.");
			}

			var model = request.Model ?? (string.IsNullOrWhiteSpace(_options.Model) ? defaultModel : _options.Model);
			var responseFormat = BuildResponseFormat(request.ResponseFormat);

			try
			{
				var payload = BuildPayload(model, messages, request, responseFormat);
				var (ok, status, body) = await PostAsync(endpoint, apiKey!, payload, cancellationToken);

				if (!ok && responseFormat != null && status == 400)
				{
					_logger.LogWarning("AI provider rejected response_format (status 400); retrying without it.");
					payload = BuildPayload(model, messages, request, null);
					(ok, status, body) = await PostAsync(endpoint, apiKey!, payload, cancellationToken);
				}

				if (!ok)
				{
					_logger.LogError("AI provider returned {Status}: {Body}", status, body);
					return AiCompletionResult.Fail($"AI service returned status {status}.");
				}

				using var doc = JsonDocument.Parse(body);
				var content = doc.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();

				return string.IsNullOrEmpty(content)
					? AiCompletionResult.Fail("AI provider returned an empty response.")
					: AiCompletionResult.Ok(content);
			}
			catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
			{
				return AiCompletionResult.Fail("AI request was cancelled.");
			}
			catch (TaskCanceledException)
			{
				_logger.LogError("AI request timed out.");
				return AiCompletionResult.Fail("AI request timed out.");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Unexpected error calling the AI provider.");
				return AiCompletionResult.Fail("Unexpected error contacting the AI service.");
			}
		}

		private static string BuildPayload(string model, List<object> messages, AiCompletionRequest request, object? responseFormat)
		{
			var payload = new Dictionary<string, object?>
			{
				["model"] = model,
				["messages"] = messages,
				["max_tokens"] = request.MaxTokens
			};
			if (request.Temperature.HasValue) payload["temperature"] = request.Temperature.Value;
			if (responseFormat != null) payload["response_format"] = responseFormat;
			return JsonSerializer.Serialize(payload);
		}

		private object? BuildResponseFormat(AiResponseFormat? rf)
		{
			if (rf == null) return null;
			switch (rf.Type)
			{
				case AiResponseFormatType.JsonObject:
					return new { type = "json_object" };
				case AiResponseFormatType.JsonSchema:
					if (string.IsNullOrWhiteSpace(rf.SchemaJson)) return new { type = "json_object" };
					try
					{
						using var schemaDoc = JsonDocument.Parse(rf.SchemaJson);
						var schema = schemaDoc.RootElement.Clone();
						return new { type = "json_schema", json_schema = new { name = string.IsNullOrWhiteSpace(rf.SchemaName) ? "response" : rf.SchemaName, strict = true, schema } };
					}
					catch (JsonException)
					{
						_logger.LogWarning("Invalid JSON schema supplied; falling back to json_object.");
						return new { type = "json_object" };
					}
				default:
					return null;
			}
		}

		private async Task<(bool ok, int status, string body)> PostAsync(string endpoint, string apiKey, string json, CancellationToken ct)
		{
			using var httpRequest = new HttpRequestMessage(HttpMethod.Post, endpoint)
			{
				Content = new StringContent(json, Encoding.UTF8, "application/json")
			};
			httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

			using var response = await _httpClient.SendAsync(httpRequest, ct);
			var body = await response.Content.ReadAsStringAsync(ct);
			var ok = (int)response.StatusCode is >= 200 and < 300;
			return (ok, (int)response.StatusCode, body);
		}
	}
}