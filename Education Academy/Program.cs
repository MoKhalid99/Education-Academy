using Education_Academy.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// ربط قاعدة البيانات SQL Server
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseSqlServer(connectionString));

//إعداد Identity (تسجيل الدخول)
builder.Services.AddDefaultIdentity<ApplicationUser>(options => {
	options.Password.RequireDigit = false;
	options.Password.RequiredLength = 8;
	options.Password.RequireNonAlphanumeric = false;
})
.AddRoles<IdentityRole>() // تفعيل نظام الرولز للأدمن
.AddEntityFrameworkStores<ApplicationDbContext>();

//  إضافة خدمات MudBlazor وخدماتنا الخاصة
builder.Services.AddMudServices();
builder.Services.AddScoped<FileSecurityService>();
builder.Services.AddHttpClient();

builder.Services.AddRazorComponents()
	.AddInteractiveServerComponents();

var app = builder.Build();

// تفعيل حساب الأدمن الموحد عند التشغيل
using (var scope = app.Services.CreateScope())
{
	await SeedData.InitializeAsync(scope.ServiceProvider);
}

// إعدادات Middleware التقليدية
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error", createScopeForErrors: true);
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorComponents<App>()
	.AddInteractiveServerRenderMode();

app.Run();