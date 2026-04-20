using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
namespace EducationAcademy.Models
{
	// نموذج المستخدم الخاص بتطبيق الأكاديمية التعليمية، يرث من IdentityUser لإدارة المستخدمين والأدوار
	public class ApplicationUser : IdentityUser
	{
		[Required]
		[MaxLength(100)]
		public string FullName { get; set; } = string.Empty;
		public int XP { get; set; } = 0;
		public int CurrentLevel { get; set; } = 1; public bool IsAdmin { get; set; } = false;
		public int MaxLevelReached { get; set; } = 1;
	}
}