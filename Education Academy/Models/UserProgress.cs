namespace EducationAcademy.Models
{
	// هذا النموذج يمثل تقدم المستخدم في المواد والاختبارات، يتتبع ما إذا كان المستخدم قد أكمل مادة أو اختبار معين
	public class UserProgress
 {
		public int Id { get; set; }
		public string UserId { get; set; } = string.Empty;

		public int LevelId { get; set; }
		public DateTime CompletionDate { get; set; } = DateTime.Now;

		public bool IsCompleted { get; set; }
		public ApplicationUser? User { get; set; }
	}
}
