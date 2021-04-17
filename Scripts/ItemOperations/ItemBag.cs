using System.Collections.Generic;
using Godot;

namespace BrickAndMortal.Scripts.ItemOperations
{
	class ItemBag
	{
		public int ItemsCollected = 0;

		public delegate void AddedItem(Item item);
		public event AddedItem EventAddedItem;

		private List<string> _items = new List<string>();


		public void CollectItem(Item item)
		{
			item.Id = ItemsCollected;
			ItemsCollected++;

			AddItem(item.ToJSON());
			EventAddedItem(item);

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

		public int GetItemCount()
		{
			return _items.Count;
		}

		public string GetSaveJSON()
		{
			var returnValue = "\"ItemsColected\":" + ItemsCollected + ", \"Items\":[";
			for (int i = 0; i < _items.Count; i++)
			{
				if (i > 0)
					returnValue += ", ";
				returnValue += "\"" + _items[i].Replace("\"", "\\\"") + "\"";
			}
			returnValue += "]";

			return returnValue;
		}
	}
}
