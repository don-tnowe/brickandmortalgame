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
		private int[] _itemEnchantments = { 0, 1, 2 };

		private Random _random = new Random();

		public string GetRandomRoomPath()
		{
			return _roomLoadFolder + "/" + _roomNames[_random.Next() % _roomNames.Length] + ".tscn";
		}

		public Item GetRandomItem()
		{
			var item = new Item() 
			{
				ItemType = _random.Next(3),
				Power = _itemPSM[0] / 2 + _random.Next(_itemPSM[0]),
				Shine = _itemPSM[1] / 2 + _random.Next(_itemPSM[1]),
				Magic = _random.Next(_itemPSM[2]),

				Frame = _random.Next(4),
			};

			var n = 1;
			var enchCount = Math.Max(4 - item.Magic / 20, 0);
			var forbiddenEnchants = new List<int>();
			forbiddenEnchants.Add(int.MaxValue);

			while (n < enchCount)
			{
				var idx = _random.Next(_itemEnchantments.Length - forbiddenEnchants.Count);
				for (int i = 0; i < forbiddenEnchants.Count; i++)
					if (idx >= forbiddenEnchants[i])
						idx++;
					else
					{
						forbiddenEnchants.Insert(i, idx);
						break;
					}
				if ((ItemData.AllEnchantments[_itemEnchantments[idx]].ApplicableTo & (ItemData.EquipFlags)(1 << item.ItemType)) != 0)
				{
					item.HeldEnchantments[n, 0] = _itemEnchantments[idx];
					item.HeldEnchantments[n, 1] = 1 + _random.Next(19 - n * 4);
					++n;
				}
			}

			return item;
		}
	}
}
