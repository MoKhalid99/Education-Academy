using EducationAcademy.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EducationAcademy.Controllers
{
	[Route("account")]
	[ApiController]
	public class AccountController : Controller
	{
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly UserManager<ApplicationUser> _userManager;

		public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
		{
			_signInManager = signInManager;
			_userManager = userManager;
		}

		// إرسال المستخدم إلى المنصة الخارجية (Google, Facebook, twiter.)
		[HttpPost("ExternalLogin")]
		public IActionResult ExternalLogin([FromForm] string provider, [FromForm] string returnUrl = "/")
		{
			var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });

			// ضبط الخصائص لإخبار Identity بالمنصة المطلوبة
			var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

			// توجيه المستخدم باستخدام دالة Challenge المبنية في الكلاس
			return Challenge(properties, provider);
		}

		// استقبال المستخدم بعد تسجيل دخوله في المنصة الخارجية
		[HttpGet("ExternalLoginCallback")]
		public async Task<IActionResult> ExternalLoginCallback(string returnUrl = "/", string remoteError = null)
		{
			if (remoteError != null) return LocalRedirect($"/login?errorMessage=خطأ من المنصة: {remoteError}");

			// استلام البيانات من المنصة الخارجية
			var info = await _signInManager.GetExternalLoginInfoAsync();
			if (info == null) return LocalRedirect("/login");

			// محاولة تسجيل الدخول إذا كان الحساب مربوطاً مسبقاً
			var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);

			if (result.Succeeded) return LocalRedirect(returnUrl);

			// إذا لم يكن لديه حساب، نقوم بإنشاء حساب جديد له باستخدام بياناته 
			var email = info.Principal.FindFirstValue(ClaimTypes.Email);
			var name = info.Principal.FindFirstValue(ClaimTypes.Name);

			if (email != null)
			{
				var user = await _userManager.FindByEmailAsync(email);
				if (user == null)
				{
					user = new ApplicationUser
					{
						UserName = email,
						Email = email,
						FullName = name,
						XP = 0,
						CurrentLevel = 1
					};
					await _userManager.CreateAsync(user);
				}

				// ربط الحساب الخارجي بحساب الأكاديمية
				await _userManager.AddLoginAsync(user, info);
				await _signInManager.SignInAsync(user, isPersistent: false);
			}

			return LocalRedirect(returnUrl);
		}
	}
}