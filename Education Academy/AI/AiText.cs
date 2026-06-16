namespace ChatbotModule.AI
{
    public static class AiText
    {
        public static string Truncate(string? text, int maxChars)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;
            if (text.Length <= maxChars) return text;
            return text.Substring(0, maxChars) + "\n...[truncated]";
        }
    }
}
