using System.ComponentModel.DataAnnotations;
//موديل البيانات المدخلة
namespace EducationAcademy.Models
{
    public class RegisterModel
 {
		[Required(ErrorMessage = "الاسم مطلوب")]
		public string FullName { get; set; } = string.Empty;

		[Required(ErrorMessage = "الايميل مطلوب")]
		[EmailAddress(ErrorMessage = "صيغة الايميل غير صحيحة")]
		public string Email { get; set; } = string.Empty;

		[Required(ErrorMessage = "رقم الهاتف مطلوب")]
		// التحقق من رقم الهاتف المصري (010, 011, 012, 015)
		[RegularExpression(@"^(010|011|012|015|\+2010|\+2011|\+2012|\+2015)[0-9]{8}$",
			ErrorMessage = "يجب إدخال رقم هاتف مصري صحيح")]
		public string Phone { get; set; } = string.Empty;

		[Required(ErrorMessage = "الباسورد مطلوب")]
		[MinLength(6, ErrorMessage = "الباسورد يجب أن يكون 6 أحرف على الأقل")]
		public string Password { get; set; } = string.Empty;
	}
}
