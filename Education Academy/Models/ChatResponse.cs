namespace ChatbotModule.Models
{
    // Response body for POST /api/chat/send
    public class ChatResponse
    {
        public int SessionId { get; set; }
        public string? Reply { get; set; }
        public DateTime CreatedAt { get; set; }

        // Null on success; set when the AI provider call failed.
        public string? Error { get; set; }
    }
}
