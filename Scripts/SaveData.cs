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

		public static int[] Upgrades = { 0, 0, 0 };

		private const string _fileFolder = "user://";
		private static string _fileName = "Savegame0.dat";

		public static void SaveGame()
		{
			var str = "{";

			str += "\"Money\":" + Money;
			str += ", \"CurCrawl\":" + CurCrawl;

			str += ", " + ItemBag.GetSaveJSON();

			str += ", \"PossibleCustomers\":" + JsonSerializer.Serialize(PossibleCustomers);
			
			str += ", \"Upgrades\":" + JsonSerializer.Serialize(Upgrades);

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
			
			var parsedValues = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(contents);

			Money = parsedValues["Money"].GetInt32();
			CurCrawl = parsedValues["CurCrawl"].GetInt32();

			ItemBag.ItemsCollected = parsedValues["ItemsCollected"].GetInt32();
			var parsedDict = parsedValues["Items"];
			ItemBag.Clear();
			for (int i = 0; i < parsedDict.GetArrayLength(); ++i)
				ItemBag.AddItem(parsedDict[i].GetString());
			
			var parsedArr = parsedValues["PossibleCustomers"];
			
			for (int i = 0; i < parsedArr.GetArrayLength(); ++i)
				PossibleCustomers.Add(parsedArr[i].GetString());
			
			parsedArr = parsedValues["Upgrades"];
			
			for (int i = 0; i < parsedArr.GetArrayLength(); ++i)
				Upgrades[i] = parsedArr[i].GetInt32();
				
			file.Close();
		}
	}
}
