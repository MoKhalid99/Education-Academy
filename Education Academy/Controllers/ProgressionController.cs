using EducationAcademy.Models;
using EducationAcademy.Services;
using Microsoft.AspNetCore.Mvc;

namespace EducationAcademy.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ProgressionController : ControllerBase
	{
		private readonly IProgressionService _progressionService;

		public ProgressionController(IProgressionService progressionService)
		{
			_progressionService = progressionService;
		}

		[HttpPost("update-xp")]
		public async Task<IActionResult> UpdateXP([FromBody] UpdateXPRequest request)
		{
			if (string.IsNullOrEmpty(request.UserId)) return BadRequest("User ID is required");

			await _progressionService.UpdateXPAsync(request.UserId, request.Points);
			return Ok(new { message = "XP updated successfully" });
		}

		[HttpPost("complete-level")]
		public async Task<IActionResult> CompleteLevel([FromBody] LevelCompletionRequest request)
		{
			// حفظ التقدم في UserProgress
			var progress = new UserProgress
			{
				UserId = request.UserId,
				LevelId = request.LevelId,
				IsCompleted = true,
				CompletionDate = DateTime.Now
			};
			await _progressionService.SaveQuizAttemptAsync(progress);

			// محاولة فتح المستوى التالي
			bool unlocked = await _progressionService.UnlockNextLevelAsync(request.UserId, request.LevelId);

			return Ok(new
			{
				NextLevelUnlocked = unlocked,
				message = unlocked ? "Next level unlocked!" : "Progress saved"
			});
		}
		[HttpGet("get-xp/{userId}")]
		public async Task<IActionResult> GetUserXP(string userId)
		{
			var xp = await _progressionService.GetUserXPAsync(userId);
			return Ok(xp);
		}
	}


	// نموذج بيانات للطلب (DTO)
	public class LevelCompletionRequest
	{
		public string UserId { get; set; } = string.Empty;
		public int LevelId { get; set; }
	}
}
