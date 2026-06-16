using EducationAcademy.Models;
using ChatbotModule.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EducationAcademy.Data
{
	// DbContext الأساسي للأكاديمية التعليمية مدمج معه نظام المحادثة ومتوافق مع SQL Server
	public class AcademyDbContext : IdentityDbContext<ApplicationUser>
	{
		public AcademyDbContext(DbContextOptions<AcademyDbContext> options) : base(options) { }

		// جداول الأكاديمية التعليمية
		public DbSet<Level> Levels { get; set; }
		public DbSet<Material> Materials { get; set; }
		public DbSet<Quiz> Quizzes { get; set; }
		public DbSet<UserProgress> UserProgresses { get; set; }

		// جداول نظام المحادثة (الـ Chatbot) التي تم دمجها
		public DbSet<ChatSession> ChatSessions { get; set; }
		public DbSet<ChatMessage> ChatMessages { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			// ضروري جداً لتهيئة جداول الـ Identity الأساسية
			base.OnModelCreating(builder);

			#region إعدادات الأكاديمية التعليمية

			// إضافة المستويات الافتراضية
			builder.Entity<Level>().HasData(
				new Level { Id = 1, LevelNumber = 1, RequiredXP = 100 },
				new Level { Id = 2, LevelNumber = 2, RequiredXP = 200 },
				new Level { Id = 3, LevelNumber = 3, RequiredXP = 300 },
				new Level { Id = 4, LevelNumber = 4, RequiredXP = 400 }
			);

			#endregion

			#region إعدادات نظام المحادثة (Chatbot) المتوافقة مع SQL Server

			// إعدادات جدول ChatSession
			builder.Entity<ChatSession>(entity =>
			{
				entity.HasKey(s => s.SessionID);

				// تحديد حجم المعرف كـ nvarchar(450) ليتوافق مع معرفات الـ Identity الافتراضية
				entity.Property(s => s.UserID)
					  .IsRequired()
					  .HasColumnType("nvarchar(450)");

				entity.HasIndex(s => s.UserID);
			});

			// إعدادات جدول ChatMessage
			builder.Entity<ChatMessage>(entity =>
			{
				entity.HasKey(m => m.MessageID);

				entity.Property(m => m.Role)
					  .IsRequired()
					  .HasColumnType("nvarchar(20)");

				entity.Property(m => m.Content)
					  .IsRequired()
					  .HasColumnType("nvarchar(max)"); // للنصوص الطويلة ومحتوى الرسائل

				// إعداد العلاقة مع الحذف التلقائي المتتالي
				entity.HasOne(m => m.Session)
					  .WithMany(s => s.Messages)
					  .HasForeignKey(m => m.SessionID)
					  .OnDelete(DeleteBehavior.Cascade);

				entity.HasIndex(m => m.SessionID);
			});

			#endregion
		}
	}
}