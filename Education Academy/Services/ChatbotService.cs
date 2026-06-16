using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ChatbotModule.AI;
using ChatbotModule.Configuration;
using EducationAcademy.Data;
using ChatbotModule.Models;

namespace ChatbotModule.Services
{
	public class ChatbotService : IChatbotService
	{
		private readonly AcademyDbContext _db;
		private readonly IAiClient _aiClient;
		private readonly ChatbotOptions _options;

		public ChatbotService(AcademyDbContext db, IAiClient aiClient, IOptions<ChatbotOptions> options)
		{
			_db = db;
			_aiClient = aiClient;
			_options = options.Value;
		}

		public async Task<ChatSession> CreateSessionAsync(string userId, string? title = null)
		{
			var session = new ChatSession
			{
				UserID = userId, // إسناد نصي سليم
				Title = title,
				CreatedAt = DateTime.UtcNow
			};

			await _db.ChatSessions.AddAsync(session);
			await _db.SaveChangesAsync();
			return session;
		}

		public async Task<ChatResponse?> SendMessageAsync(string userId, ChatRequest request, CancellationToken cancellationToken = default)
		{
			var session = await _db.ChatSessions
				.FirstOrDefaultAsync(s => s.SessionID == request.SessionId && s.UserID == userId, cancellationToken);

			if (session == null)
			{
				return null;
			}

			var userMessage = new ChatMessage
			{
				SessionID = session.SessionID,
				Role = AiRoles.User,
				Content = request.Message,
				CreatedAt = DateTime.UtcNow
			};
			await _db.ChatMessages.AddAsync(userMessage, cancellationToken);
			await _db.SaveChangesAsync(cancellationToken);

			var recentMessages = await _db.ChatMessages
				.Where(m => m.SessionID == session.SessionID)
				.OrderByDescending(m => m.MessageID)
				.Take(_options.MaxHistoryCount)
				.ToListAsync(cancellationToken);
			recentMessages.Reverse();

			var aiRequest = ContextBuilder.Build(recentMessages, _options);

			var result = await _aiClient.CompleteAsync(aiRequest, cancellationToken);
			if (!result.Success)
			{
				return new ChatResponse
				{
					SessionId = session.SessionID,
					Error = result.ErrorMessage
				};
			}

			var assistantMessage = new ChatMessage
			{
				SessionID = session.SessionID,
				Role = AiRoles.Assistant,
				Content = result.Content,
				CreatedAt = DateTime.UtcNow
			};
			await _db.ChatMessages.AddAsync(assistantMessage, cancellationToken);

			session.UpdatedAt = DateTime.UtcNow;
			await _db.SaveChangesAsync(cancellationToken);

			return new ChatResponse
			{
				SessionId = session.SessionID,
				Reply = assistantMessage.Content,
				CreatedAt = assistantMessage.CreatedAt
			};
		}

		public async Task<IEnumerable<ChatMessage>?> GetHistoryAsync(string userId, int sessionId)
		{
			var ownsSession = await _db.ChatSessions
				.AnyAsync(s => s.SessionID == sessionId && s.UserID == userId);

			if (!ownsSession)
			{
				return null;
			}

			return await _db.ChatMessages
				.Where(m => m.SessionID == sessionId)
				.OrderBy(m => m.MessageID)
				.ToListAsync();
		}
	}
}