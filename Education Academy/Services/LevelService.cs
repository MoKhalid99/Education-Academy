using EducationAcademy.Models;
using System.Collections.Generic;
//المراحل الخاصة باللعبة المتاهة
namespace EducationAcademy.Services
{
	public static class LevelRepository
	{
		public static List<GameLevel> GetLevels()
		{
			var levels = new List<GameLevel>();

			// المرحلة 1: أساسيات الحركة
			levels.Add(new GameLevel
			{
				Id = 1,
				Title = "البداية",
				Goal = "تحرك للهدف",
				StartPos = new(0, 5),
				TargetPos = new(2, 5)
			});

			// المرحلة 2: ممر ضيق
			levels.Add(new GameLevel
			{
				Id = 2,
				Title = "الزقاق الصخري",
				Goal = "تجاوز العقبة الوحيدة",
				StartPos = new(0, 5),
				TargetPos = new(5, 5),
				Obstacles = { new(3, 5) }
			});

			// المرحلة 3: أول مواجهة
			levels.Add(new GameLevel
			{
				Id = 3,
				Title = "حارس البوابة",
				Goal = "اهزم السلايم للعبور",
				StartPos = new(0, 5),
				TargetPos = new(5, 5),
				Enemies = { new Enemy(3, 5, 1, EnemyType.Slime) }
			});

			// المرحلة 4: الكمين
			levels.Add(new GameLevel
			{
				Id = 4,
				Title = "فخ الزاوية",
				Goal = "السلايم سيطاردك الآن!",
				StartPos = new(0, 5),
				TargetPos = new(5, 0),
				Enemies = { new Enemy(4, 1, 1, EnemyType.Slime) }
			});

			// المرحلة 5: المتاهة البسيطة
			levels.Add(new GameLevel
			{
				Id = 5,
				Title = "المتاهة الملتوية",
				Goal = "جد طريقك بين الصخور",
				StartPos = new(0, 5),
				TargetPos = new(5, 0),
				Obstacles = { new(1, 4), new(2, 3), new(3, 2), new(4, 1) }
			});

			// المرحلة 6: فرقة السلايم
			levels.Add(new GameLevel
			{
				Id = 6,
				Title = "وادي الهلام",
				Goal = "عدوان مزدوج",
				StartPos = new(0, 0),
				TargetPos = new(5, 5),
				Enemies = { new Enemy(2, 2, 1, EnemyType.Slime), new Enemy(4, 4, 1, EnemyType.Slime) }
			});

			// المرحلة 7: الحصن
			levels.Add(new GameLevel
			{
				Id = 7,
				Title = "الحصن الحجري",
				Goal = "دفاعات قوية",
				StartPos = new(0, 5),
				TargetPos = new(5, 0),
				Obstacles = { new(4, 0), new(4, 1) },
				Enemies = { new Enemy(3, 1, 1, EnemyType.Slime) }
			});

			// المرحلة 8: ظهور التنين الأول
			levels.Add(new GameLevel
			{
				Id = 8,
				Title = "نَفَس النار",
				Goal = "التنين يتحمل 3 ضربات!",
				StartPos = new(0, 5),
				TargetPos = new(5, 5),
				Enemies = { new Enemy(3, 5, 3, EnemyType.Dragon) }
			});

			// المرحلة 9: حقل الألغام
			levels.Add(new GameLevel
			{
				Id = 9,
				Title = "منطقة الخطر",
				Goal = "عقبات في كل مكان",
				StartPos = new(0, 3),
				TargetPos = new(5, 3),
				Obstacles = { new(2, 2), new(2, 3), new(2, 4), new(3, 2), new(3, 4) }
			});

			// المرحلة 10: التوأم الناري
			levels.Add(new GameLevel
			{
				Id = 10,
				Title = "برج التنانين",
				Goal = "مواجهة تنينين في مساحة ضيقة",
				StartPos = new(0, 0),
				TargetPos = new(5, 5),
				Enemies = { new Enemy(1, 4, 3, EnemyType.Dragon), new Enemy(4, 1, 3, EnemyType.Dragon) }
			});

			// المرحلة 11: الزحف البطيء
			levels.Add(new GameLevel
			{
				Id = 11,
				Title = "الممر الضيق",
				Goal = "تحرك بحذر",
				StartPos = new(0, 5),
				TargetPos = new(5, 0),
				Obstacles = { new(0, 1), new(1, 1), new(2, 1), new(3, 1), new(4, 1), new(5, 3) },
				Enemies = { new Enemy(5, 1, 2, EnemyType.Slime) }
			});

			// المرحلة 12: هجوم شامل
			levels.Add(new GameLevel
			{
				Id = 12,
				Title = "الفوضى",
				Goal = "سلايم وتنانين معاً",
				StartPos = new(0, 5),
				TargetPos = new(5, 0),
				Enemies = { new Enemy(2, 2, 1, EnemyType.Slime), new Enemy(3, 3, 3, EnemyType.Dragon), new Enemy(1, 1, 1, EnemyType.Slime) }
			});

			// المرحلة 13: القفل والمفتاح
			levels.Add(new GameLevel
			{
				Id = 13,
				Title = "غرفة العزل",
				Goal = "تخلص من الحراس للوصول",
				StartPos = new(0, 5),
				TargetPos = new(5, 5),
				Obstacles = { new(4, 5), new(4, 4), new(5, 3) },
				Enemies = { new Enemy(4, 3, 3, EnemyType.Dragon) }
			});

			// المرحلة 14: المتاهة الكبرى
			levels.Add(new GameLevel
			{
				Id = 14,
				Title = "المتاهة المظلمة",
				Goal = "آخر اختبار قبل الزعيم",
				StartPos = new(0, 0),
				TargetPos = new(5, 5),
				Obstacles = { new(1, 0), new(1, 1), new(1, 2), new(3, 3), new(3, 4), new(3, 5) },
				Enemies = { new Enemy(2, 2, 1, EnemyType.Slime), new Enemy(4, 4, 3, EnemyType.Dragon) }
			});

			// المرحلة 15: معركة الزعيم الأسطوري
			levels.Add(new GameLevel
			{
				Id = 15,
				Title = "عرين الزعيم",
				Goal = "اهزم التنين الأعظم ",
				StartPos = new(0, 5),
				TargetPos = new(5, 0),
				Enemies = { new Enemy(3, 2, 12, EnemyType.Dragon) },
				Obstacles = { new(2, 2), new(4, 2), new(3, 1), new(0, 1), new(1, 0) }
			});

			return levels;
		}
	}
}