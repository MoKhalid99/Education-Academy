namespace EducationAcademy.Models
{
	//موديل للعبة كود المتاهة
	public class Position
	{
		public int X { get; set; }
		public int Y { get; set; }

		public Position(int x, int y)
		{
			X = x;
			Y = y;
		}

		// دالة للمقارنة السهلة بين موقعين
		public override bool Equals(object? obj) =>
			obj is Position p && p.X == X && p.Y == Y;

		public override int GetHashCode() => HashCode.Combine(X, Y);
	}
}
