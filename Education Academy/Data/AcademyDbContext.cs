using EducationAcademy.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

// DbContext الخاص بتطبيق الأكاديمية التعليمية، يستخدم IdentityDbContext لإدارة المستخدمين والأدوار
//الداتا بيز
public class AcademyDbContext : IdentityDbContext<ApplicationUser>
{
	public AcademyDbContext(DbContextOptions<AcademyDbContext> options) : base(options) { }

	public DbSet<Level> Levels { get; set; }
	public DbSet<Material> Materials { get; set; }
	public DbSet<Quiz> Quizzes { get; set; }
	public DbSet<UserProgress> UserProgresses { get; set; }

	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);

		// إضافة المستويات الافتراضية
		builder.Entity<Level>().HasData(
			new Level { Id = 1, LevelNumber = 1, RequiredXP = 100 },
			new Level { Id = 2, LevelNumber = 2, RequiredXP = 200 },
			new Level { Id = 3, LevelNumber = 3, RequiredXP = 300 },
			new Level { Id = 4, LevelNumber = 4, RequiredXP = 400 }
		);
	}
}