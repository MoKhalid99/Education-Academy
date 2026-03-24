using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
		: base(options)
	{
	}

	public DbSet<Course> Courses { get; set; }
	public DbSet<Quiz> Quizzes { get; set; }

	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);

		// إعداد العلاقات بين الجداول
		builder.Entity<Course>()
			.HasMany(c => c.Quizzes)
			.WithOne(q => q.Course)
			.HasForeignKey(q => q.CourseId);
	}
}