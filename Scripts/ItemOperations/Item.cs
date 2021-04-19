using System.Text.Json;
using Godot;

namespace BrickAndMortal.Scripts.ItemOperations
{
	public class Item
	{
		public int Uid;
		public string Pool;

		public int ItemType;
		public int Power;
		public int Shine;
		public int Magic;
		public int[][] HeldEnchantments = new int[][] { new int[4], new int[4]};

		public int Frame;
		public int CrawlNumber;

		public Item() { }
		
		public Item(string from)
		{
			var parsed = JsonSerializer.Deserialize<System.Collections.Generic.Dictionary<string, JsonElement>>(from);

			Uid = parsed["Id"].GetInt32();
			Pool = parsed["Pool"].GetString();

			ItemType = parsed["ItemType"].GetInt32();
			Power = parsed["Power"].GetInt32();
			Shine = parsed["Shine"].GetInt32();
			Magic = parsed["Magic"].GetInt32();

			var enchants = parsed["Enchants"];

			for (int i = 0; i < enchants.GetArrayLength(); ++i)
			{
				HeldEnchantments[0][i] = enchants[i].GetInt32();
			}

			enchants = parsed["EnchantValues"];

			for (int i = 0; i < enchants.GetArrayLength(); ++i)
			{
				HeldEnchantments[1][i] = enchants[i].GetInt32();
			}

			Frame = parsed["Frame"].GetInt32();
			CrawlNumber = parsed["CrawlNumber"].GetInt32();
		}

		public string ToJSON()
		{
			// Seed MUST be the first value.
			var returnValue = "{"
				+ "\"Id\":" + Uid
				+ ", \"Pool\":\"" + Pool + "\""
				+ ", \"ItemType\":" + ItemType
				+ ", \"Power\":" + Power
				+ ", \"Shine\":" + Shine
				+ ", \"Magic\":" + Magic + ", \"Enchants\":["
				;
			for (int i = 0; i < HeldEnchantments[0].Length; ++i)
			{
				if (i != 0)
					returnValue += ", ";
				returnValue += HeldEnchantments[0][i];
			}
			returnValue += "], \"EnchantValues\":[";

			for (int i = 0; i < HeldEnchantments[1].Length; ++i)
			{
				if (i != 0)
					returnValue += ", ";
				returnValue += HeldEnchantments[1][i];
			}

			returnValue += "], \"Frame\":" + Frame;
			returnValue += ", \"CrawlNumber\":" + CrawlNumber;

			returnValue += "}";

			return returnValue;
		}
	}
}
