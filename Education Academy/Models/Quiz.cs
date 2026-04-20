using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EducationAcademy.Models
{
	//  (Entity Model)
	public class Quiz
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "عنوان الاختبار مطلوب")]
		public string Title { get; set; } = string.Empty;

		public int LevelId { get; set; }
		public Level Level { get; set; } = null!;

		public int XPValue { get; set; } = 30; // النقاط الافتراضية
	}

	//  JSON (Data Transfer Object)
	public class QuizData
	{
		[JsonPropertyName("title")]
		public string Title { get; set; } = string.Empty;

		[JsonPropertyName("xpValue")]
		public int XPValue { get; set; } = 30;

		[JsonPropertyName("timeLimitMinutes")]
		public int TimeLimitMinutes { get; set; } = 20;

		[JsonPropertyName("questions")]
		public List<QuestionItem> Questions { get; set; } = new();
	}

	// كل سؤال داخل ملف الجيسون
	public class QuestionItem
	{
		[JsonPropertyName("id")]
		public int Id { get; set; }

		[JsonPropertyName("text")]
		[Required]
		public string Text { get; set; } = string.Empty;

		[JsonPropertyName("options")]
		public List<string> Options { get; set; } = new();

		[JsonPropertyName("correctAnswer")]
		[Required]
		public string CorrectAnswer { get; set; } = string.Empty;
	}
	// تتبع إكمال المستخدم للاختبارات
	public class UserQuizCompletion
	{
		public int Id { get; set; }
		public string UserId { get; set; } = string.Empty;
		public int LevelId { get; set; }
		public int QuizNum { get; set; }
		public int Score { get; set; }
		public DateTime CompletionDate { get; set; } = DateTime.Now;
	}
}