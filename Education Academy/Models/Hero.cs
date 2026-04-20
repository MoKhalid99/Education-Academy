namespace EducationAcademy.Models
{
	//موديل الشخصية الرئسية في لعبة المتاهة
	public class Hero
	{
		public int Health { get; set; } = 3;
		public int MaxHealth { get; set; } = 3;

		public void Reset()
		{
			Health = MaxHealth;
		}
	}
}
