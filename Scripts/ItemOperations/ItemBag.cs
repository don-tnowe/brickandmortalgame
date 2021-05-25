using System.Collections.Generic;

namespace BrickAndMortal.Scripts.ItemOperations
{
	class ItemBag : Godot.Reference
	{
		public int ItemsCollected = 0;

		[Godot.Signal]
		public delegate void AddedItem(string itemJSON);

		private List<Item> _items = new List<Item>();


		public void CollectItem(Item item)
		{
			item.Uid = ItemsCollected;
			ItemsCollected++;

			AddItem(item);
			EmitSignal(nameof(AddedItem), item);

			SaveData.SaveGame();
		}

		public void AddItem(Item item)
		{
			_items.Add(item);
		}

		public void RemoveItem(int uid)
		{
			for (int i = 0; i < _items.Count; i++)
				if (_items[i].Uid == uid)
				{
					_items.RemoveAt(i);
					break;
				}

			SaveData.SaveGame();
		}

		public Item[] GetItemArray()
		{
			return _items.ToArray();
		}

		public void Clear()
		{
			_items = new List<Item>();
		}

		public int GetItemCount()
		{
			return _items.Count;
		}

		public string GetSaveJSON()
		{
			var returnValue = "\"ItemsCollected\":" + ItemsCollected + ", \"Items\":[";
			for (int i = 0; i < _items.Count; i++)
			{
				if (i > 0)
					returnValue += ", ";
				returnValue += "\"" + _items[i].ToJSON().Replace("\"", "\\\"") + "\"";
			}
			returnValue += "]";

			return returnValue;
		}
	}
}
