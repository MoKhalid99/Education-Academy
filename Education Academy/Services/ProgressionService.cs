using EducationAcademy.Models;
using Microsoft.EntityFrameworkCore;

namespace EducationAcademy.Services
{
	public class ProgressionService : IProgressionService
	{
		private readonly AcademyDbContext _context;

		public ProgressionService(AcademyDbContext context)
		{
			_context = context;
		}

		public async Task UpdateXPAsync(string userId, int points)
		{
			var user = await _context.Users.FindAsync(userId);
			if (user != null)
			{
				user.XP += points;
				await _context.SaveChangesAsync();
			}
		}

		public async Task<bool> UnlockNextLevelAsync(string userId, int currentLevel)
		{
			var user = await _context.Users.FindAsync(userId);

			// شرط فتح المستوى: 
			//  المستخدم موجود
			//  المستوى الذي حله الطالب هو نفسه أعلى مستوى وصل إليه حالياً ليمنع التلاعب بفتح مستويات مستقبلية
			// المستوى الحالي ليس المستوى الأخير
			if (user != null && user.MaxLevelReached == currentLevel && currentLevel < 4)
			{
				user.MaxLevelReached++;
				await _context.SaveChangesAsync();
				return true;
			}
			return false;
		}

		public async Task<UserProgress?> GetUserProgressAsync(string userId, int levelId)
		{
			return await _context.UserProgresses
				.FirstOrDefaultAsync(p => p.UserId == userId && p.LevelId == levelId);
		}

		public async Task<int> GetUserXPAsync(string userId)
		{
			var user = await _context.Users.FindAsync(userId);
			return user?.XP ?? 0;
		}

		public async Task SaveQuizAttemptAsync(UserProgress progress)
		{
			// نتحقق إذا كان المستخدم قد أنهى هذا المستوى من قبل لتجنب التكرار 
			var existing = await GetUserProgressAsync(progress.UserId, progress.LevelId);

			if (existing == null)
			{
				_context.UserProgresses.Add(progress);
			}
			else
			{
				existing.IsCompleted = true;
				existing.CompletionDate = DateTime.Now;
			}

			await _context.SaveChangesAsync();
		}
	}
}

