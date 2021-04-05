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

			var n = 1;
			var enchCount = Math.Max(4 - item.Magic / 20, 0);
			var forbiddenEnchants = new List<int>();
			forbiddenEnchants.Add(int.MaxValue);

			while (n < enchCount)
			{
				var idx = random.Next(_itemEnch.Length - forbiddenEnchants.Count);
				for (int i = 0; i < forbiddenEnchants.Count; i++)
					if (idx >= forbiddenEnchants[i])
						idx++;
					else
					{
						forbiddenEnchants.Insert(i, idx);
						break;
					}
				if ((ItemData.AllEnchantments[_itemEnch[idx]].ApplicableTo & (ItemData.EquipFlags)(1 << item.ItemType)) != 0)
				{
					item.HeldEnchantments[n, 0] = _itemEnch[idx];
					item.HeldEnchantments[n, 1] = _itemEnchValues[idx] / 2 + random.Next(_itemEnchValues[idx]);
					 ++n;
				}
			}

			return item;
		}
	}
}
