namespace ChatbotModule.AI
{
	public sealed class AiMessage
	{
		public string Role { get; set; } = AiRoles.User;
		public string Content { get; set; } = string.Empty;

		public AiMessage() { }

		public AiMessage(string role, string content)
		{
			Role = role;
			Content = content;
		}
	}

	public enum AiResponseFormatType
	{
		Text,
		JsonObject,
		JsonSchema
	}

	public sealed class AiResponseFormat
	{
		public AiResponseFormatType Type { get; set; } = AiResponseFormatType.Text;
		public string? SchemaName { get; set; }
		// تم تغيير الاسم هنا من JsonSchema إلى SchemaJson لمنع التضارب مع اسم الدالة
		public string? SchemaJson { get; set; }

		public static AiResponseFormat JsonObject() => new() { Type = AiResponseFormatType.JsonObject };

		public static AiResponseFormat JsonSchema(string name, string schemaJson) =>
			new() { Type = AiResponseFormatType.JsonSchema, SchemaName = name, SchemaJson = schemaJson };
	}

	public sealed class AiCompletionRequest
	{
		public string? SystemPrompt { get; set; }
		public List<AiMessage> Messages { get; set; } = new();
		public int MaxTokens { get; set; } = 1000;
		public double? Temperature { get; set; }
		public string? Model { get; set; }
		public AiResponseFormat? ResponseFormat { get; set; }

		public static AiCompletionRequest FromPrompt(string prompt, string? systemPrompt = null, int maxTokens = 1000)
		{
			return new AiCompletionRequest
			{
				SystemPrompt = systemPrompt,
				MaxTokens = maxTokens,
				Messages = new List<AiMessage> { new(AiRoles.User, prompt) }
			};
		}
	}

	public sealed class AiCompletionResult
	{
		public bool Success { get; init; }
		public string Content { get; init; } = string.Empty;
		public string? ErrorMessage { get; init; }

		public static AiCompletionResult Ok(string content) => new() { Success = true, Content = content };
		public static AiCompletionResult Fail(string error) => new() { Success = false, ErrorMessage = error };
	}

	public interface IAiClient
	{
		Task<AiCompletionResult> CompleteAsync(AiCompletionRequest request, CancellationToken cancellationToken = default);
	}
}