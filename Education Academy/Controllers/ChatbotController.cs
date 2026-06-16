using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ChatbotModule.Models;
using ChatbotModule.Services;

namespace ChatbotModule.Controllers
{
	[Route("api/chat")]
	[ApiController]
	[Authorize] // يحمي جميع العمليات ويتطلب التحقق من الهوية (JWT)
	public class ChatbotController : ControllerBase
	{
		private readonly IChatbotService _chatbotService;
		private readonly IChatRateLimiter _rateLimiter;

		public ChatbotController(IChatbotService chatbotService, IChatRateLimiter rateLimiter)
		{
			_chatbotService = chatbotService;
			_rateLimiter = rateLimiter;
		}

		// POST /api/chat/new-session
		[HttpPost("new-session")]
		public async Task<IActionResult> NewSession([FromBody] NewSessionDto? dto)
		{
			if (!TryGetUserId(out var userId)) return Unauthorized();

			// تمرير الـ string userId بنجاح دون مشاكل تحويل
			var session = await _chatbotService.CreateSessionAsync(userId, dto?.Title);
			return Ok(new
			{
				session.SessionID,
				session.Title,
				session.CreatedAt
			});
		}

		// POST /api/chat/send
		[HttpPost("send")]
		public async Task<IActionResult> Send([FromBody] ChatRequest request, CancellationToken cancellationToken)
		{
			if (!TryGetUserId(out var userId)) return Unauthorized();

			if (!await _rateLimiter.AllowAsync(userId, cancellationToken))
			{
				return StatusCode(429, new { message = "Rate limit exceeded. Please slow down." });
			}

			if (request == null || string.IsNullOrWhiteSpace(request.Message))
			{
				return BadRequest("Message is required.");
			}

			var response = await _chatbotService.SendMessageAsync(userId, request, cancellationToken);
			if (response == null)
			{
				return NotFound("Chat session not found.");
			}
			if (response.Error != null)
			{
				return StatusCode(502, new { message = response.Error });
			}

			return Ok(response);
		}

		// GET /api/chat/history/{sessionId}
		[HttpGet("history/{sessionId}")]
		public async Task<IActionResult> History(int sessionId)
		{
			if (!TryGetUserId(out var userId)) return Unauthorized();

			var messages = await _chatbotService.GetHistoryAsync(userId, sessionId);
			return messages == null ? NotFound("Chat session not found.") : Ok(messages);
		}


		private bool TryGetUserId(out string userId)
		{
			var s = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			userId = s ?? string.Empty;
			return !string.IsNullOrEmpty(s);
		}

		public class NewSessionDto
		{
			[System.ComponentModel.DataAnnotations.StringLength(200)]
			public string? Title { get; set; }
		}
	}
}