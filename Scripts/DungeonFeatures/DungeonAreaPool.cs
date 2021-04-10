using BrickAndMortal.Scripts.ItemOperations;
using System;
using Godot;

namespace BrickAndMortal.Scripts.DungeonFeatures
{
	public class DungeonAreaPool : Resource
	{
		[Export]
		private string _roomLoadFolder = "res://Scenes/Rooms";
		[Export]
		private string[] _roomNames = {"Start"};
		[Export]
		private int[] _itemStats = new int[3];
		[Export]
		private int[] _itemEnch = { 0, 1, 2 };
		[Export]
		private int[] _itemEnchValues = { 20, 20, 20 };

		private Random _random = new Random();

		public string GetRandomRoomPath()
		{
			return _roomLoadFolder + "/" + _roomNames[_random.Next() % _roomNames.Length] + ".tscn";
		}

		public Item GetRandomItem()
		{
			var item = new Item()
			{
				Id = -1,
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

			item.HeldEnchantments[0] = ItemData.GetRandomEnchants(_itemEnch, enchCount, (EquipFlags)(1 << item.ItemType), _random);
			for (int i = 0; i < item.HeldEnchantments[0].Length; i++)
			{
				item.HeldEnchantments[1][i] = (int)(_itemEnchValues[item.HeldEnchantments[0][i]] * (0.5 + _random.NextDouble()));
			}

			return item;
		}
	}
}
