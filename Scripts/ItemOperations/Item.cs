using System.Text.Json;
using Godot;

namespace BrickAndMortal.Scripts.ItemOperations
{
	public class Item : Reference //Inherit
	{
		public int Uid;
		public string Pool;

		public int ItemType;
		public int Power;
		public int Shine;
		public int Magic;
		public ItemEnchantment[] HeldEnchantments = new ItemEnchantment[4];
		public int[] HeldEnchantmentValues = new int[4];

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
				HeldEnchantments[i] = ItemEnchantment.GetEnchant(enchants[i].GetInt32());
			}

			enchants = parsed["EnchantValues"];

			for (int i = 0; i < enchants.GetArrayLength(); ++i)
			{
				HeldEnchantmentValues[i] = enchants[i].GetInt32();
			}

			Frame = parsed["Frame"].GetInt32();
			CrawlNumber = parsed["CrawlNumber"].GetInt32();
		}

		public string ToJSON()
		{
			// UID MUST be the first value.
			var returnValue = "{"
				+ "\"Id\":" + Uid
				+ ", \"Pool\":\"" + Pool + "\""
				+ ", \"ItemType\":" + ItemType
				+ ", \"Power\":" + Power
				+ ", \"Shine\":" + Shine
				+ ", \"Magic\":" + Magic + ", \"Enchants\":["
				;
			for (int i = 0; i < HeldEnchantments.Length; ++i)
			{
				if (HeldEnchantments[i] == null)
					break;
					
				if (i != 0)
					returnValue += ", ";
					
				returnValue += HeldEnchantments[i].Id;
			}
			returnValue += "], \"EnchantValues\":[";

			for (int i = 0; i < HeldEnchantmentValues.Length; ++i)
			{
				if (i != 0)
					returnValue += ", ";
				returnValue += HeldEnchantmentValues[i];
			}

			returnValue += "], \"Frame\":" + Frame;
			returnValue += ", \"CrawlNumber\":" + CrawlNumber;

			returnValue += "}";

			return returnValue;
		}
	}
}
