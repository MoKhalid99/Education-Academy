using System.ComponentModel.DataAnnotations;
//لو في اخطاء في تسجيل الدخول هتظهر للمستخدم
namespace EducationAcademy.Models
{
    public class LoginViewModel
 {
		[Required(ErrorMessage = "الايميل مطلوب")]
		[EmailAddress(ErrorMessage = "صيغة الايميل غير صحيحة")]
		public string Email { get; set; } = string.Empty;

		[Required(ErrorMessage = "الباسورد مطلوب")]
		public string Password { get; set; } = string.Empty;
	}
}
