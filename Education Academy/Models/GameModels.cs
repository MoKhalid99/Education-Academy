namespace FG1MAdd;

public class LevelData
{
	public string Language { get; set; } = "";
	public string Color { get; set; } = "#333";
	public string StyleSuffix { get; set; } = "";
	public List<StageData> Stages { get; set; } = new();
}

public class StageData
{
	public int Level { get; set; }
	public int Hp { get; set; }
	public string Code { get; set; } = "";
	public string Output { get; set; } = "";
	public string HealCode { get; set; } = "";
	public string MoveName { get; set; } = "";
}

public class Fighter
{
	public double X { get; set; }
	public double HP { get; set; }
	public double MaxHP { get; set; }
	public double EP { get; set; }
	public double MaxEP { get; set; }
	public double BaseDamage { get; set; } = 3;

	public string StateClass { get; set; } = "idle";
	public int AttackIndex { get; set; } = 0;
	public bool IsBlocking { get; set; }
	public bool IsHit { get; set; }

	// الخصائص الجديدة للذكاء الاصطناعي والتأثيرات
	public string FlipClass { get; set; } = ""; // لتغيير اتجاه الوجه ديناميكياً
	public int PoisonStacks { get; set; } = 0;  // لتراكم سم البايثون
	public bool IsShielded { get; set; } = false; // لدرع السي شارب
}

public class Projectile
{
	public double X { get; set; }
	public double Y { get; set; } = 55;
	public bool IsActive { get; set; } = false;
}

public class UpgradeOption
{
	public string Title { get; set; } = "";
	public string Description { get; set; } = "";
	public string Icon { get; set; } = "";
	public Action ApplyUpgrade { get; set; } = () => { };
}