using Microsoft.AspNetCore.Identity;

public static class SeedData
{
	public static async Task InitializeAsync(IServiceProvider serviceProvider)
	{
		var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
		var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

		// إنشاء صلاحية الأدمن إذا لم تكن موجودة
		if (!await roleManager.RoleExistsAsync("Admin"))
		{
			await roleManager.CreateAsync(new IdentityRole("Admin"));
		}

		// إنشاء حساب الأدمن الموحد
		string adminEmail = "adminMKH@chaseplan.com";
		string adminPassword = "12345678";

		if (await userManager.FindByEmailAsync(adminEmail) == null)
		{
			var adminUser = new ApplicationUser
			{
				UserName = adminEmail,
				Email = adminEmail,
				FullName = "MKH",
				IsAdmin = true,
				EmailConfirmed = true
			};

			var result = await userManager.CreateAsync(adminUser, adminPassword);
			if (result.Succeeded)
			{
				await userManager.AddToRoleAsync(adminUser, "Admin");
			}
		}
	}
}