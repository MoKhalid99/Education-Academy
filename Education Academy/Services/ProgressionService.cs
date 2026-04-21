using EducationAcademy.Models;
using Microsoft.EntityFrameworkCore;

namespace EducationAcademy.Services
{
	public class ProgressionService : IProgressionService
	{
		private readonly IServiceScopeFactory _scopeFactory;

		// حقن IServiceScopeFactory بدلاً من DbContext المباشر
		public ProgressionService(IServiceScopeFactory scopeFactory)
		{
			_scopeFactory = scopeFactory;
		}

		public async Task UpdateXPAsync(string userId, int points)
		{
			using var scope = _scopeFactory.CreateScope();
			var context = scope.ServiceProvider.GetRequiredService<AcademyDbContext>();

			var user = await context.Users.FindAsync(userId);
			if (user != null)
			{
				user.XP += points;
				await context.SaveChangesAsync();
			}
		}

		public async Task<bool> UnlockNextLevelAsync(string userId, int currentLevel)
		{
			using var scope = _scopeFactory.CreateScope();
			var context = scope.ServiceProvider.GetRequiredService<AcademyDbContext>();

			var user = await context.Users.FindAsync(userId);

			if (user != null && user.MaxLevelReached == currentLevel && currentLevel < 4)
			{
				user.MaxLevelReached++;
				await context.SaveChangesAsync();
				return true;
			}
			return false;
		}

		public async Task<UserProgress?> GetUserProgressAsync(string userId, int levelId)
		{
			using var scope = _scopeFactory.CreateScope();
			var context = scope.ServiceProvider.GetRequiredService<AcademyDbContext>();

			return await context.UserProgresses
				.FirstOrDefaultAsync(p => p.UserId == userId && p.LevelId == levelId);
		}

		public async Task<int> GetUserXPAsync(string userId)
		{
			using var scope = _scopeFactory.CreateScope();
			var context = scope.ServiceProvider.GetRequiredService<AcademyDbContext>();

			var user = await context.Users.FindAsync(userId);
			return user?.XP ?? 0;
		}

		public async Task SaveQuizAttemptAsync(UserProgress progress)
		{
			using var scope = _scopeFactory.CreateScope();
			var context = scope.ServiceProvider.GetRequiredService<AcademyDbContext>();

			var existing = await context.UserProgresses
				.FirstOrDefaultAsync(p => p.UserId == progress.UserId && p.LevelId == progress.LevelId);

			if (existing == null)
			{
				context.UserProgresses.Add(progress);
			}
			else
			{
				existing.IsCompleted = true;
				existing.CompletionDate = DateTime.Now;
				context.UserProgresses.Update(existing); // تأكيد التحديث
			}

			await context.SaveChangesAsync();
		}
	}
}
