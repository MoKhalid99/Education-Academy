using Blazored.LocalStorage;
using Education_Academy.Components;
using EducationAcademy.Models;
using EducationAcademy.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// إضافة الخدمات الأساسية
builder.Services.AddRazorComponents()
	.AddInteractiveServerComponents();

// تسجيل خدمة التشفير 
builder.Services.AddScoped<EncryptionService>();

// تسجيل خدمة التقدم
builder.Services.AddScoped<IProgressionService, ProgressionService>();

builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

// إضافة دعم الـ Controllers لخدمة الـ API
builder.Services.AddControllers();

builder.Services.AddMudServices();
builder.Services.AddScoped<EncryptionService>();
builder.Services.AddCascadingAuthenticationState();

// ربط قاعدة البيانات 
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AcademyDbContext>(options =>
	options.UseSqlServer(connectionString));

// إعداد Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => {
	options.Password.RequireDigit = false;
	options.Password.RequiredLength = 8;
	options.Password.RequireNonAlphanumeric = false;
	options.Password.RequireUppercase = false;
	options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<AcademyDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddAuthentication(options => {
	options.DefaultScheme = IdentityConstants.ApplicationScheme;
	options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
});

// اضافة خدمات التخزين المحلي
builder.Services.AddBlazoredLocalStorage();

// إعداد HttpClient ليستخدم رابط المشروع الأساسي دائماً للوصول لملفات wwwroot
var frontendUrl = builder.Configuration["FrontendUrl"] ?? "https://localhost:7240/";
builder.Services.AddScoped(sp => new HttpClient
{
	BaseAddress = new Uri(frontendUrl)
});

var app = builder.Build();

// تهيئة بيانات الأدمن 
using (var scope = app.Services.CreateScope())
{
	var services = scope.ServiceProvider;
	await SeedData.InitializeAsync(services);
}

// تشفير Middleware
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error", createScopeForErrors: true);
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // هام جداً للسماح بقراءة مجلد wwwroot
app.UseAntiforgery();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorComponents<App>()
	.AddInteractiveServerRenderMode();

// لمعالجة الدخول والتسجيل وتسجيل الخروج
app.MapPost("/account/login", async (
	[FromForm] string email,
	[FromForm] string password,
	SignInManager<ApplicationUser> signInManager) =>
{
	var result = await signInManager.PasswordSignInAsync(email, password, isPersistent: true, lockoutOnFailure: false);
	if (result.Succeeded) return Results.Redirect("/");
	return Results.Redirect("/login?error=invalid");
});

app.MapPost("/account/register", async (
	[FromForm] string fullName,
	[FromForm] string email,
	[FromForm] string phone,
	[FromForm] string password,
	UserManager<ApplicationUser> userManager,
	SignInManager<ApplicationUser> signInManager) =>
{
	var user = new ApplicationUser
	{
		UserName = email,
		Email = email,
		FullName = fullName,
		PhoneNumber = phone
	};

	var result = await userManager.CreateAsync(user, password);
	if (result.Succeeded)
	{
		await signInManager.SignInAsync(user, isPersistent: true);
		return Results.Redirect("/");
	}
	return Results.Redirect($"/register?error={result.Errors.First().Description}");
});

app.MapGet("/account/logout", async (SignInManager<ApplicationUser> signInManager) =>
{
	await signInManager.SignOutAsync();
	return Results.Redirect("/");
});

app.Run();