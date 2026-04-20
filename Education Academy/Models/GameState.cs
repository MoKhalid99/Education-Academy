namespace EducationAcademy.Models
{
	// موديل يمثل حالة اللعبة الحالية، مثل موقع اللاعب والمستوى الحالي في لعبة المتاهة 
	public class GameState
	{
		public int PlayerX { get; set; }
		public int PlayerY { get; set; }
		public int CurrentLevelIndex { get; set; }
		public int MaxUnlockedLevel { get; set; }
		public bool IsRunning { get; set; }
		public string Message { get; set; } = "";

		public void ResetPlayer(int startX, int startY)
		{
			PlayerX = startX;
			PlayerY = startY;
			Message = "";
			IsRunning = false;
		}
	}
}