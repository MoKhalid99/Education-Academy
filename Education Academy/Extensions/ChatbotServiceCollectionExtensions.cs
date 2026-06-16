using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ChatbotModule.AI;
using ChatbotModule.Configuration;
using EducationAcademy.Data;
using ChatbotModule.Services;

namespace ChatbotModule.Extensions
{
	public static class ChatbotServiceCollectionExtensions
	{
		public static IServiceCollection AddChatbotModule(this IServiceCollection services, Action<ChatbotOptions> configure)
		{
			services.Configure(configure);

			var options = new ChatbotOptions();
			configure(options);

			if (string.IsNullOrWhiteSpace(options.ConnectionString))
			{
				throw new InvalidOperationException(
					"ChatbotOptions.ConnectionString is required so the module can register AcademyDbContext (SQL Server).");
			}

			// ربط التطبيق بـ SQL Server من خلال السياق التعليمي الشامل والموحد للأكاديمية
			services.AddDbContext<AcademyDbContext>(o =>
				o.UseSqlServer(options.ConnectionString));

			services.AddHttpClient<IAiClient, OpenAiClient>(client =>
				client.Timeout = TimeSpan.FromSeconds(options.TimeoutSeconds));

			services.AddScoped<IChatbotService, ChatbotService>();

			services.TryAddSingleton<IChatRateLimiter, NoOpChatRateLimiter>();

			return services;
		}
	}
}