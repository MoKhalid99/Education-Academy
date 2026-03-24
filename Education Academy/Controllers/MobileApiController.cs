using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/mobile")]
public class MobileApiController : ControllerBase
{
	private readonly ApplicationDbContext _context;
	public MobileApiController(ApplicationDbContext context) => _context = context;

	[HttpGet("level-info/{userId}")]
	public async Task<IActionResult> GetLevel(string userId)
	{
		var user = await _context.Users.FindAsync(userId);
		if (user == null) return NotFound();
		return Ok(new { user.Level, user.ExperiencePoints });
	}

	[HttpPost("update-xp")]
	public async Task<IActionResult> UpdateXP([FromBody] UpdateXPRequest req)
	{
		var user = await _context.Users.FindAsync(req.UserId);
		user.ExperiencePoints += req.Points;
		if (user.ExperiencePoints >= 100) { user.Level++; user.ExperiencePoints = 0; }
		await _context.SaveChangesAsync();
		return Ok(new { NewLevel = user.Level });
	}
}