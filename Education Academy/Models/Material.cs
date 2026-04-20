using System.ComponentModel.DataAnnotations;

namespace EducationAcademy.Models
{
    public class Material
 {
		public int Id { get; set; }
		[Required]
		public string Title { get; set; } = string.Empty;
		[Required]
		public string EncryptedFilePath { get; set; } = string.Empty; // مسار الملف المشفر
		public int LevelId { get; set; }
		public Level Level { get; set; } = null!;
	}
}
