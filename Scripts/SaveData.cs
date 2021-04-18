using System.Collections.Generic;
using System.Text.Json;
using Godot;

namespace BrickAndMortal.Scripts
{
	static class SaveData
	{
		public static int Screen = 0;
		public static int CurCrawl = 0;
		
		public static int Money = 0;

		public static ItemOperations.ItemBag ItemBag = new ItemOperations.ItemBag();

		public static List<string> PossibleCustomers = new List<string>{ 
			"Barbarian", "WarriorHeroic", "ApprenticeCool", "ApprenticeNoble", "Pyro",
			"MonsterSlayer", "Botanist", "Tinkerer", "Trickster"
		};

		private const string _fileFolder = "user://";
		private static string _fileName = "Savegame0.dat";

		public static void SaveGame()
		{
			var str = "{";

			str += "\"Money\":" + Money;
			str += ", \"CurCrawl\":" + CurCrawl;

			str += ", " + ItemBag.GetSaveJSON();

			str += ", \"Customers\":" + JsonSerializer.Serialize(PossibleCustomers);

			str += "}";

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
			
			var parsed = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(contents);

			Money = parsed["Money"].GetInt32();
			CurCrawl = parsed["CurCrawl"].GetInt32();

			ItemBag.ItemsCollected = parsed["ItemsCollected"].GetInt32();
			var items = parsed["Items"];
			ItemBag.Clear();
			for (int i = 0; i < items.GetArrayLength(); ++i)
				ItemBag.AddItem(items[i].GetString());
			file.Close();
		}
	}
}
