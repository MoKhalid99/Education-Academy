using System.ComponentModel.DataAnnotations;

namespace ChatbotModule.Models
{
	public class ChatSession
	{
		[Key]
		public int SessionID { get; set; }

		// معرف المستخدم المالك للجلسة (مستخرج كـ string من الـ Token)
		public string UserID { get; set; } = string.Empty;

		[StringLength(200)]
		public string? Title { get; set; }

		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public DateTime? UpdatedAt { get; set; }

		public ICollection<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
	}
}