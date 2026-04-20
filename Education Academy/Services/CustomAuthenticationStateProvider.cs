using EducationAcademy.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
//تتبع المستخدمين
namespace EducationAcademy.Services
{
	public class CustomAuthenticationStateProvider : RevalidatingServerAuthenticationStateProvider
	{
		private readonly IServiceScopeFactory _scopeFactory;

		public CustomAuthenticationStateProvider(
			ILoggerFactory loggerFactory,
			IServiceScopeFactory scopeFactory) 
			: base(loggerFactory)
		{
			_scopeFactory = scopeFactory;
		}

		// المدة الزمنية لإعادة التحقق من المستخدم (30 دقيقة)
		protected override TimeSpan RevalidationInterval => TimeSpan.FromMinutes(30);

		protected override async Task<bool> ValidateAuthenticationStateAsync(
			AuthenticationState authenticationState, CancellationToken cancellationToken)
		{
			// نفتح Scope جديد للتحقق من وجود المستخدم في قاعدة البيانات
			using var scope = _scopeFactory.CreateScope();
			var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

			var user = await userManager.GetUserAsync(authenticationState.User);
			if (user == null)
			{
				return false;
			}

			return await ValidateSecurityStampAsync(userManager, authenticationState.User);
		}

		private async Task<bool> ValidateSecurityStampAsync(UserManager<ApplicationUser> userManager, ClaimsPrincipal principal)
		{
			var user = await userManager.GetUserAsync(principal);
			if (user == null) return false;

			if (!userManager.SupportsUserSecurityStamp) return true;

			// استخدام الثوابت الافتراضية للـ SecurityStamp
			var principalStamp = principal.FindFirstValue(new ClaimsIdentityOptions().SecurityStampClaimType);
			var userStamp = await userManager.GetSecurityStampAsync(user);

			return principalStamp == userStamp;
		}
	}
}