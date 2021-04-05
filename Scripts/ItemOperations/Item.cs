using System.Text.Json;
using Godot;

namespace BrickAndMortal.Scripts.ItemOperations
{
	public class Item
	{
		public int Seed;
		public string Pool;

		public int ItemType;
		public int Power;
		public int Shine;
		public int Magic;
		public int[,] HeldEnchantments = new int[4, 2];

		public int Frame;
		public int CrawlNumber;

		public Item() { }
		
		public Item(string from)
		{
			var parsed = JsonSerializer.Deserialize<System.Collections.Generic.Dictionary<string, JsonElement>>(from);

			Seed = parsed["Seed"].GetInt32();
			Pool = parsed["Pool"].GetString();

			ItemType = parsed["ItemType"].GetInt32();
			Power = parsed["Power"].GetInt32();
			Shine = parsed["Shine"].GetInt32();
			Magic = parsed["Magic"].GetInt32();

			var enchants = parsed["Enchants"];
			for (int i = 0; i < enchants.GetArrayLength(); ++i)
			{
				HeldEnchantments[i, 0] = enchants[i][0].GetInt32();
				HeldEnchantments[i, 1] = enchants[i][1].GetInt32();
			}
			Frame = parsed["Frame"].GetInt32();
			CrawlNumber = parsed["CrawlNumber"].GetInt32();
		}

		public string ToJSON()
		{
			// Seed MUST be the first value.
			var returnValue = "{"
				+ "\"Seed\":" + Seed
				+ ", \"Pool\":\"" + Pool + "\""
				+ ", \"ItemType\":" + ItemType
				+ ", \"Power\":" + Power
				+ ", \"Shine\":" + Shine
				+ ", \"Magic\":" + Magic + ", \"Enchants\":["
				;
			for (int i = 0; i < HeldEnchantments.GetLength(0); ++i)
			{
				if (i != 0)
					returnValue += ", ";
				returnValue += "[" + HeldEnchantments[i, 0] + ", " + HeldEnchantments[i, 1] + "]";
			}

			returnValue += "], \"Frame\":" + Frame;
			returnValue += ", \"CrawlNumber\":" + CrawlNumber;

			returnValue += "}";

			return returnValue;
		}
	}
}
