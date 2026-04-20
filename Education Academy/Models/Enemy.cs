namespace EducationAcademy.Models
{
	//موديل كود كومبت لعبة المتاهة
	public enum EnemyType
	{
		Slime, 
		Dragon 
	}
	public class Enemy
	{
		public Guid Id { get; set; } = Guid.NewGuid();
		public int X { get; set; }
		public int Y { get; set; }
		public int Health { get; set; }
		public int MaxHealth { get; set; }
		public EnemyType Type { get; set; }
		public bool IsAlive => Health > 0;

		public Enemy(int x, int y, int health, EnemyType type)
		{
			X = x;
			Y = y;
			Health = health;
			MaxHealth = health;
			Type = type;
		}

		public void TakeDamage(int damage)
		{
			Health -= damage;
			if (Health < 0) Health = 0;
		}
	}
}