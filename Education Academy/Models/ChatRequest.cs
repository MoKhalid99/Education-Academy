using System.ComponentModel.DataAnnotations;

namespace ChatbotModule.Models
{
    // Request body for POST /api/chat/send
    public class ChatRequest
    {
        [Required]
        public int SessionId { get; set; }

        [Required]
        [StringLength(4000)]
        public string Message { get; set; } = string.Empty;
    }
}
