using Microsoft.AspNetCore.Identity;

public class ApplicationUser : IdentityUser
{
	public string FullName { get; set; } = string.Empty;
	public int Level { get; set; } = 0; 
	public int ExperiencePoints { get; set; } = 0; // الـ XP
	public bool IsAdmin { get; set; } = false;
}