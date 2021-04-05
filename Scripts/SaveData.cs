using Godot;

namespace BrickAndMortal.Scripts
{
	static class SaveData
	{
		public static int Screen = 0;
		public static int HeroPos = 0;

		public static int Money = 0;

		private static readonly ItemOperations.ItemBag _itemBag = new ItemOperations.ItemBag();

		private const string _fileFolder = "user://Saves/";
		private static string _fileName = "savegame0.dat";

		public static void SaveGame()
		{
			var str = "{";

			str += "}";

			var file = new File();
			file.Open(_fileFolder + _fileName, File.ModeFlags.Write);
			file.StoreLine("WRITE JSON HERE");
			file.Close();
		}

		public static void LoadGame()
		{
			var file = new File();
			if (!file.FileExists(_fileFolder + _fileName))
				return;
			file.Open(_fileFolder + _fileName, File.ModeFlags.Read);
			var contents = file.GetLine();
			// Load as JSON
			file.Close();
		}
	}
}
