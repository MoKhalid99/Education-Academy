using System.Collections.Generic;
//موديل المراحل الخاصة بلعبة المتاهة 
namespace EducationAcademy.Models
{
	public class GameLevel
	{
		public int Id { get; set; }
		public string Title { get; set; } = string.Empty;
		public string Goal { get; set; } = string.Empty;

		// نقطة البداية والنهاية
		public Position StartPos { get; set; } = new(0, 5);
		public Position TargetPos { get; set; } = new(5, 0);

		// قائمة العقبات الحجارة
		public List<Position> Obstacles { get; set; } = new();

		// قائمة الأعداء في المرحلة
		public List<Enemy> Enemies { get; set; } = new();

		/// دالة لإنشاء نسخة عميقة (Deep Copy) من المرحلة 
		/// تُستخدم عند بدء المرحلة أو إعادة تشغيلها لضمان عودة الأعداء للحياة
		public GameLevel Copy()
		{
			return new GameLevel
			{
				Id = Id,
				Title = Title,
				Goal = Goal,
				StartPos = new Position(StartPos.X, StartPos.Y),
				TargetPos = new Position(TargetPos.X, TargetPos.Y),
				Obstacles = new List<Position>(Obstacles.Select(o => new Position(o.X, o.Y))),
				Enemies = Enemies.Select(e => new Enemy(e.X, e.Y, e.MaxHealth, e.Type)).ToList()
			};
		}
	}
}