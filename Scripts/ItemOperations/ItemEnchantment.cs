using System.Collections.Generic;
using System;

namespace BrickAndMortal.Scripts.ItemOperations
{
	public class ItemEnchantment
	{
		public int Id;
		public string Name;
		public int MagicCost;
		public int BaseValue;
		public EquipFlags ApplicableTo;
		
		private static ItemEnchantment[] _allEnchantments =
		{
			 new ItemEnchantment() {
					Id = 0, Name = "Sharpness",
					MagicCost = 3, BaseValue = 3,
					ApplicableTo = EquipFlags.Weapon
				},
			 new ItemEnchantment() {
					Id = 1, Name = "Spellpower",
					MagicCost = 3, BaseValue = 3,
					ApplicableTo = EquipFlags.Spell
				},
			 new ItemEnchantment() {
					Id = 2, Name = "Protection",
					MagicCost = 3, BaseValue = 3,
					ApplicableTo = EquipFlags.AllArmor
				},
			 new ItemEnchantment() {
					Id = 3, Name = "Iron Damage",
					MagicCost = 6, BaseValue = 5,
					ApplicableTo = EquipFlags.All
				},
			 new ItemEnchantment() {
					Id = 4, Name = "Fire Damage",
					MagicCost = 6, BaseValue = 10,
					ApplicableTo = EquipFlags.All
				},
			 new ItemEnchantment() {
					Id = 5, Name = "Poison Damage",
					MagicCost = 6, BaseValue = 10,
					ApplicableTo = EquipFlags.All
				},
			 new ItemEnchantment() {
					Id = 6, Name = "Magic Damage",
					MagicCost = 9, BaseValue = 20,
					ApplicableTo = EquipFlags.All
				},
			 new ItemEnchantment() {
					Id = 7, Name = "Ghostly Damage",
					MagicCost = 9, BaseValue = 20,
					ApplicableTo = EquipFlags.All
				}
		};

		public static ItemEnchantment GetEnchant(int idx)
		{
			return _allEnchantments[idx];
		}

		public static ItemEnchantment[] GetRandomEnchants(int[] enchPool, int count, EquipFlags equipFlags, Random random = null)
		{
			if (random == null)
				random = new Random();

			var forbiddenEnchants = new List<int>();
			var appliedEnchants = new ItemEnchantment[count];
			int n = 0;

			while (n < count)
			{
				var idxInPool = random.Next(enchPool.Length - forbiddenEnchants.Count);
				
				var insertIdx = 0;
				while (insertIdx < forbiddenEnchants.Count && idxInPool >= forbiddenEnchants[insertIdx])
				{
					idxInPool++;
					insertIdx++;
				}
				forbiddenEnchants.Insert(insertIdx, idxInPool);

				var ench = _allEnchantments[enchPool[idxInPool]];
				
				if ((ench.ApplicableTo & equipFlags) != 0)
				{
					appliedEnchants[n] = ench;
					++n;
				}
			}
			return appliedEnchants;
		}
	}
	[Flags]
	public enum EquipFlags
	{
		None = 0,
		All = 255,

		Weapon = 1,
		Spell = 2,
		Chestplate = 4,
		Helmet = 8,
		Boots = 16,
		Gauntlets = 32,
		Ring = 64,
		Necklace = 128,

		WeaponOrSpell = Weapon | Spell,
		AllArmor = Chestplate | Helmet | Boots | Gauntlets,
		AllJewellery = Ring | Necklace,
	}
}
