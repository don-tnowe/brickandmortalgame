using System.Collections.Generic;

namespace BrickAndMortal.Scripts.ItemOperations
{
	class ItemBag
	{
		private List<string> _items = new List<string>();

		public void CollectItem(Item item)
		{
			AddItem(item.ToJSON());
			SaveData.SaveGame();
		}

		public void AddItem(string item)
		{
			_items.Add(item);
		}

		public void RemoveItem(int seed)
		{
			var seedStr = seed.ToString();
			for (int i = 0; i < _items.Count; i++)
				if (_items[i].Substring(7).StartsWith(seedStr))
				{
					_items.RemoveAt(i);
					break;
				}
			SaveData.SaveGame();
		}

		public Item[] GetItemArray()
		{
			var returnValue = new Item[_items.Count];
			for (int i = 0; i < returnValue.Length; i++)
				returnValue[i] = new Item(_items[i]);
			return returnValue;
		}

		public string GetSaveJSON()
		{
			var returnValue = "\"Items\":[";
			for (int i = 0; i < _items.Count; i++)
			{
				if (i > 0)
					returnValue += ", ";
				returnValue += "\"" + _items[i] + "\"";
			}
			returnValue += "]";

			return returnValue;
		}
	}
}
