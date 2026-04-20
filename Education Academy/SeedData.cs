using Microsoft.AspNetCore.Identity;
using EducationAcademy.Models;
//ملف الادمن الافتراضي
public static class SeedData
{
	public static async Task InitializeAsync(IServiceProvider serviceProvider)
	{
		// جلب الخدمات اللازمة من الـ Service Provider
		var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
		var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

		// التأكد من وجود الأدوار (Roles) الأساسية
		string[] roleNames = { "Admin", "Student" };
		foreach (var roleName in roleNames)
		{
			if (!await roleManager.RoleExistsAsync(roleName))
			{
				await roleManager.CreateAsync(new IdentityRole(roleName));
			}
		}

		// إعداد بيانات الأدمن
		string adminEmail = "adminMKH@chaseplan.com";
		string adminPassword = "12345678"; 

		var adminUser = await userManager.FindByEmailAsync(adminEmail);

		if (adminUser == null)
		{
			// إنشاء المستخدم الجديد بخصائص 
			adminUser = new ApplicationUser
			{
				UserName = adminEmail,
				Email = adminEmail,
				FullName = "MKH Admin",
				IsAdmin = true,
				EmailConfirmed = true,
				XP = 9999,          // الأدمن يملك نقاط كاملة افتراضياً
				CurrentLevel = 4    // الأدمن يفتح كل المستويات
			};

			var createPowerUser = await userManager.CreateAsync(adminUser, adminPassword);
			if (createPowerUser.Succeeded)
			{
				// ربط المستخدم بدور الأدمن
				await userManager.AddToRoleAsync(adminUser, "Admin");
			}
		}
		else
		{
			// تأكد من أن الأدمن الحالي يملك دور "Admin" في حال تم إنشاؤه سابقاً بدون دور
			if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
			{
				await userManager.AddToRoleAsync(adminUser, "Admin");
			}
		}
	}
}