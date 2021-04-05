using BrickAndMortal.Scripts.ItemOperations;
using System.Collections.Generic;
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
		private int[] _itemPSM = new int[3];
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
			var seed = _random.Next();
			var random = new Random(seed);
			var item = new Item()
			{
				Seed = seed,
				Pool = ResourcePath.Substring(ResourcePath.FindLast("/") + 1),

				ItemType = random.Next(3),
				Power = _itemPSM[0] / 2 + random.Next(_itemPSM[0]),
				Shine = _itemPSM[1] / 2 + random.Next(_itemPSM[1]),
				Magic = random.Next(_itemPSM[2]),

				Frame = random.Next(4),
			};

			var enchCount = Math.Max(4 - item.Magic / 20, 0);

			item.HeldEnchantments[0] = ItemData.GetRandomEnchants(_itemEnch, enchCount, (EquipFlags)(1 << item.ItemType), random);
			for (int i = 0; i < item.HeldEnchantments[0].Length; i++)
			{
				item.HeldEnchantments[1][i] = (int)(_itemEnchValues[item.HeldEnchantments[0][i]] * (0.5 + random.NextDouble()));
			}

			return item;
		}
	}
}
