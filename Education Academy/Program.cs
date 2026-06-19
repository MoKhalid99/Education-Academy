using Blazored.LocalStorage;
using ChatbotModule.Controllers;
using ChatbotModule.Extensions;
using Education_Academy.Components;
using EducationAcademy.Data;
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

// إضافة دعم الـ Controllers لخدمة الـ API للتسجيل الخارجي
builder.Services.AddControllers();

builder.Services.AddMudServices();
builder.Services.AddCascadingAuthenticationState();

// جلب نص الاتصال الموحد من ملف appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// تسجيل قاعدة البيانات الأساسية للأكاديمية (والتي تحتوي الآن على جداول الشات بوت مدمجة)
builder.Services.AddDbContext<AcademyDbContext>(options =>
	options.UseSqlServer(connectionString));

// تهيئة موديول الشات بوت وتمرير نص الاتصال لتجاوز خطأ الـ Design-Time
builder.Services.AddChatbotModule(options =>
{
	options.ConnectionString = connectionString;
	options.OpenAiApiKey = builder.Configuration["Chatbot:OpenAiApiKey"];
	options.GeminiApiKey = builder.Configuration["Chatbot:GeminiApiKey"];

	options.SystemPrompt = "You are EDo, an advanced text-only AI assistant built for Education Academy. You must always refer to yourself as EDo. Answer accurately, concisely, and directly in Arabic unless the user speaks in another language. Do not use any icons, markdown symbols for icons, or emojis in your responses; provide text-only formatting.";

	options.MaxHistoryCount = 10; // عدد الرسائل السابقة التي يتذكرها في سياق المحادثة
	options.MaxTokens = 1000;
});
builder.Services.AddControllers()
	.AddApplicationPart(typeof(ChatbotController).Assembly);

// إعداد نظام الـ Identity ليعمل مع كونتكس الأكاديمية الموحد
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
	options.Password.RequireDigit = false;
	options.Password.RequiredLength = 4;
	options.Password.RequireNonAlphanumeric = false;
	options.Password.RequireUppercase = false;
	options.Password.RequireLowercase = false;
})
.AddEntityFrameworkStores<AcademyDbContext>()
.AddDefaultTokenProviders();

// إعداد تسجيل الدخول الخارجي (Google , facebook , twiter )
builder.Services.AddAuthentication(options => {
	options.DefaultScheme = IdentityConstants.ApplicationScheme;
	options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
})
.AddGoogle(options =>
{
	options.ClientId = builder.Configuration["Authentication:Google:ClientId"]
		?? throw new InvalidOperationException("لم يتم العثور على Google ClientId");
	options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]
		?? throw new InvalidOperationException("لم يتم العثور على Google ClientSecret");
})
.AddFacebook(options =>
{
	options.AppId = builder.Configuration["Authentication:Facebook:AppId"]
		?? throw new InvalidOperationException("لم يتم العثور على Facebook AppId");
	options.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"]
		?? throw new InvalidOperationException("لم يتم العثور على Facebook AppSecret");
})
.AddTwitter(options =>
{
	options.ConsumerKey = builder.Configuration["Authentication:Twitter:ConsumerKey"]
		?? throw new InvalidOperationException("لم يتم العثور على Twitter ConsumerKey");
	options.ConsumerSecret = builder.Configuration["Authentication:Twitter:ConsumerSecret"]
		?? throw new InvalidOperationException("لم يتم العثور على Twitter ConsumerSecret");

});
// اضافة خدمات التخزين المحلي
builder.Services.AddBlazoredLocalStorage();

// إعداد HttpClient ليستخدم رابط المشروع الأساسي دائماً للوصول لملفات wwwroot
var frontendUrl = builder.Configuration["FrontendUrl"] ?? "https://localhost:7240/";
builder.Services.AddScoped(sp => new HttpClient
{
	BaseAddress = new Uri(frontendUrl)
});


builder.Services.AddHttpClient();
builder.Services.AddBlazoredLocalStorage();

var app = builder.Build();

// تهيئة خط الأنابيب (HTTP request pipeline)
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

app.MapControllers();

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
		await signInManager.SignInAsync(user, isPersistent: false);
		return Results.Redirect("/");
	}
	return Results.Redirect("/register?error=failed");
});

app.MapPost("/account/logout", async (SignInManager<ApplicationUser> signInManager) =>
{
	await signInManager.SignOutAsync();
	return Results.Redirect("/");
});

app.Run();