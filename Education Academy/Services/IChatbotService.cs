using ChatbotModule.Models;

namespace ChatbotModule.Services
{
	public interface IChatbotService
	{
		Task<ChatSession> CreateSessionAsync(string userId, string? title = null);
		Task<ChatResponse?> SendMessageAsync(string userId, ChatRequest request, CancellationToken cancellationToken = default);
		Task<IEnumerable<ChatMessage>?> GetHistoryAsync(string userId, int sessionId);
	}
}