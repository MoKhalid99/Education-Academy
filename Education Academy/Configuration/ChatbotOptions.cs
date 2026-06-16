namespace ChatbotModule.Configuration
{
    // All tunables for the chatbot module. Bind from configuration ("Chatbot" section)
    // or set inline via AddChatbotModule(options => { ... }).
    public class ChatbotOptions
    {
        public const string SectionName = "Chatbot";

        // AI provider credentials. If OpenAiApiKey is empty, GeminiApiKey is used.
        public string? OpenAiApiKey { get; set; }
        public string? GeminiApiKey { get; set; }

        // Optional model override. Null = provider default (gpt-4o-mini / gemini-2.5-flash).
        public string? Model { get; set; }

        // Context memory: how many most-recent messages are replayed to the model.
        public int MaxHistoryCount { get; set; } = 10;

        // Generation + input bounds.
        public int MaxTokens { get; set; } = 1000;
        public int MaxCharsPerMessage { get; set; } = 2000;

        // HTTP timeout for AI calls (seconds).
        public int TimeoutSeconds { get; set; } = 60;

        // Optional override of the default system prompt.
        public string? SystemPrompt { get; set; }

        // MySQL connection string for the chat tables (used by AddChatbotModule).
        public string? ConnectionString { get; set; }
    }
}
