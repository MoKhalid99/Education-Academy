namespace ChatbotModule.Services
{
    // Default prompt(s) for the chatbot. Override per-deployment via ChatbotOptions.SystemPrompt.
    public static class PromptTemplates
    {
        public const string DefaultSystemPrompt =
            "You are a helpful assistant. Follow these rules: " +
            "Answer concisely and accurately. " +
            "If you are unsure or do not have enough information, say so plainly instead of guessing. " +
            "Never invent APIs, functions, methods, parameters, or facts; if you are not certain " +
            "something exists, say you are not sure rather than fabricating it. " +
            "If the request is ambiguous or missing key details, ask one brief clarifying question first. " +
            "Put any code in fenced code blocks with a language tag. " +
            "Reply in the same language the user wrote in whenever possible.";

        public static string ResolveSystemPrompt(string? overridePrompt) =>
            string.IsNullOrWhiteSpace(overridePrompt) ? DefaultSystemPrompt : overridePrompt!;
    }
}
