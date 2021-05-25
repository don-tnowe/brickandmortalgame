using BrickAndMortal.Scripts.ItemOperations;
using System;
using Godot;

namespace BrickAndMortal.Scripts.DungeonFeatures
{
	public class DungeonAreaPool : Resource
	{
		[Export(PropertyHint.Dir)]
		private string _roomLoadFolder = "res://Scenes/Rooms";
		[Export]
		private int _roomCount = 1;
		[Export]
		private int[] _itemStats = new int[3];
		[Export]
		private int[] _itemEnch = { 0, 1, 2 };
		[Export]
		private int[] _itemEnchValues = { 20, 20, 20 };

		private Random _random = new Random();

		public string GetRandomRoomPath()
		{
			return _roomLoadFolder + "/Room" + _random.Next() % _roomCount + ".tscn";
		}

		public Item GetRandomItem()
		{
			var item = new Item()
			{
				Uid = -1,
				Pool = ResourcePath.Substring(ResourcePath.FindLast("/") + 1),

				ItemType = _random.Next(3),
				Power = _itemStats[0] / 2 + _random.Next(_itemStats[0]),
				Shine = _itemStats[1] / 2 + _random.Next(_itemStats[1]),
				Magic = _random.Next(_itemStats[2]),

				Frame = _random.Next(4),
				CrawlNumber = SaveData.CurCrawl
			};

			var enchCount = _random.Next(5);
			item.Magic = Math.Max(item.Magic - enchCount * 10, 0);

			item.HeldEnchantments = ItemEnchantment.GetRandomEnchants(_itemEnch, enchCount, (EquipFlags)(1 << item.ItemType), _random);
			
			for (int i = 0; i < item.HeldEnchantments.Length; i++)
			{
				var id = item.HeldEnchantments[i].Id;
				for (int j = 0; j < _itemEnch.Length; j++)
					if (_itemEnch[j] == id)
					{
						item.HeldEnchantmentValues[i] = (int)(_itemEnchValues[j] * (0.5 + _random.NextDouble()));
						break;
					}
			}

			return item;
		}
	}
}
