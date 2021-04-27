using Godot;

namespace BrickAndMortal.Scripts.Menus
{
	class TownDayResults : BaseMenu
	{
		public override void _Ready()
		{
			var nodeLabels = GetNode("Values");

			for (int i = 0; i < 5; i++)
				nodeLabels.GetChild<Label>(i).Text = GetDayStat(i).ToString();
		}

		private int GetDayStat(int idx)
		{
			switch (idx)
			{
				case 0:
					return SaveData.LastDayEarned;
				case 1:
					return SaveData.LastDayCustomers;
				case 2:
					return SaveData.LastDaySold;
				case 3:
					return SaveData.BestDayEarned;
				case 4:
					return SaveData.CurCrawl;
				default:
					return 0;
			}		
		}
	}
}
