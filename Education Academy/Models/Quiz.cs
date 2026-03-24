public class Quiz
{
	public int Id { get; set; }
	public int CourseId { get; set; }
	public Course Course { get; set; } = null!;
	public string JsonFilePath { get; set; } = string.Empty; // مسار الـ JSON في wwwroot
	public int MinScoreToPass { get; set; } = 50;
}