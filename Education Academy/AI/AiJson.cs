using System.Text.Json;

namespace ChatbotModule.AI
{
    public static class AiJson
    {
        private static readonly JsonSerializerOptions Options = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public static T? TryParse<T>(string? content)
        {
            if (string.IsNullOrWhiteSpace(content)) return default;

            var text = content.Trim();
            if (text.StartsWith("```"))
            {
                var firstNewline = text.IndexOf('\n');
                if (firstNewline >= 0) text = text[(firstNewline + 1)..];
                if (text.EndsWith("```")) text = text[..^3];
                text = text.Trim();
            }

            try { return JsonSerializer.Deserialize<T>(text, Options); }
            catch (JsonException) { return default; }
        }
    }
}
