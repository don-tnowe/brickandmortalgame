using System.Text.Json;
using Godot;

namespace BrickAndMortal.Scripts
{
	static class SaveData
	{
		public static int Screen = 0;
		public static Vector2 HeroPos;
		public static int CurCrawl = 0;

		public static int Money = 0;

		public static ItemOperations.ItemBag ItemBag = new ItemOperations.ItemBag();

		private const string _fileFolder = "user://Saves/";
		private static string _fileName = "savegame0.dat";

		public static void SaveGame()
		{
			var str = "{";
			str += ItemBag.GetSaveJSON();
			str += "}";
			//TODO: Save/ Load for other things

			var file = new File();
			file.Open(_fileFolder + _fileName, File.ModeFlags.Write);
			file.StoreLine(str);
			file.Close();
		}

		public static void LoadGame()
		{
			var file = new File();
			if (!file.FileExists(_fileFolder + _fileName))
				return;
			file.Open(_fileFolder + _fileName, File.ModeFlags.Read);
			var contents = file.GetLine();
			GD.Print(contents);

			var parsed = JsonSerializer.Deserialize<System.Collections.Generic.Dictionary<string, JsonElement>>(contents);

			var items = parsed["Items"];

			for (int i = 0; i < items.GetArrayLength(); ++i)
			{
				ItemBag.AddItem(items[i].GetString());
			}

			//TODO: Save/ Load for other things
			file.Close();
		}
	}
}
