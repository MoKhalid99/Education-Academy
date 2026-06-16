using ChatbotModule.AI;
using ChatbotModule.Configuration;
using ChatbotModule.Models;

namespace ChatbotModule.Services
{
    // Turns persisted chat history into a provider-agnostic AI request:
    // system prompt + the recent turns, each truncated to bound input size.
    public static class ContextBuilder
    {
        public static AiCompletionRequest Build(IEnumerable<ChatMessage> history, ChatbotOptions options)
        {
            var request = new AiCompletionRequest
            {
                SystemPrompt = PromptTemplates.ResolveSystemPrompt(options.SystemPrompt),
                MaxTokens = options.MaxTokens
            };

            foreach (var message in history)
            {
                request.Messages.Add(new AiMessage(
                    role: message.Role,
                    content: AiText.Truncate(message.Content, options.MaxCharsPerMessage)));
            }

            return request;
        }
    }
}
