using EducationAcademy.Models;

namespace EducationAcademy.Services
{
		public interface IProgressionService
		{
			// إضافة نقاط XP للمستخدم
			Task UpdateXPAsync(string userId, int points);

			// التحقق من فتح المستوى التالي
			Task<bool> UnlockNextLevelAsync(string userId, int currentLevel);

			// جلب سجل تقدم المستخدم لمستوى معين
			Task<UserProgress?> GetUserProgressAsync(string userId, int levelId);

			// حفظ سجل حل الكويز
			Task SaveQuizAttemptAsync(UserProgress progress);

		    Task<int> GetUserXPAsync(string userId);
	}
}
