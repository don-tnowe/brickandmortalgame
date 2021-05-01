using System.Collections.Generic;

namespace BrickAndMortal.Scripts.ItemOperations
{
	class ItemBag : Godot.Reference
	{
		public int ItemsCollected = 0;

		[Godot.Signal]
		public delegate void AddedItem(string itemJSON);

		private List<string> _items = new List<string>();


		public void CollectItem(Item item)
		{
			item.Uid = ItemsCollected;
			ItemsCollected++;

			AddItem(item.ToJSON());
			EmitSignal(nameof(AddedItem), item.ToJSON());

			SaveData.SaveGame();
		}

		public void AddItem(string item)
		{
			_items.Add(item);
		}

		public void RemoveItem(int uid)
		{
			var uidStr = uid.ToString();
			for (int i = 0; i < _items.Count; i++)
				if (_items[i].StartsWith("{\"Id\":" + uidStr))
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

		public void Clear()
		{
			_items = new List<string>();

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
				returnValue += "\"" + _items[i].Replace("\"", "\\\"") + "\"";
			}
			returnValue += "]";

			return returnValue;
		}
	}
}
