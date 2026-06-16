using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatbotModule.Models
{
    public class ChatMessage
    {
        [Key]
        public int MessageID { get; set; }

        public int SessionID { get; set; }
        [ForeignKey("SessionID")]
        public ChatSession? Session { get; set; }

        // "user", "assistant", or "system" (see ChatbotModule.AI.AiRoles).
        [Required]
        [StringLength(20)]
        public string Role { get; set; } = string.Empty;

        public string? Content { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
