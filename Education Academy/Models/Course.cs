public class Course
{
	public int Id { get; set; }
	public string Title { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public int RequiredLevel { get; set; } = 0; // الليفل المطلوب لفتح المادة
	public string EncryptedFilePath { get; set; } = string.Empty; // مسار الملف المشفر

	public ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();
}