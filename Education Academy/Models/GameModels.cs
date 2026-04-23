namespace FG1MAdd;

public class LevelData
{
	public string Language { get; set; } = "";
	public string Color { get; set; } = "#333";
	public List<StageData> Stages { get; set; } = new();
}

public class StageData
{
	public int Level { get; set; }
	public double EnemyHP { get; set; }
	public string Code { get; set; } = "";
	public string Output { get; set; } = "";
	public string Description { get; set; } = "";
	public string MoveName { get; set; } = "";
}

public class Fighter
{
	public double X { get; set; } = 20;
	public double HP { get; set; } = 100;
	public double MaxHP { get; set; } = 100;
	public double EP { get; set; } = 100;
	public double MaxEP { get; set; } = 100;
	public double BaseDamage { get; set; } = 10;

	public string StateClass { get; set; } = "idle";
	public string FlipClass { get; set; } = "face-right";

	// حالات خاصة
	public double PoisonTimer { get; set; } = 0;
	public bool IsShielded { get; set; } = false;
	public DateTime LastSpecialAction { get; set; } = DateTime.Now.AddSeconds(-20);
}