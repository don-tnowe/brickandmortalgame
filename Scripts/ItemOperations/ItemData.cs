using System;

namespace BrickAndMortal.Scripts.ItemOperations
{
	public static class ItemData
	{
		public static ItemEnchantment[] AllEnchantments =
		{
			 new ItemEnchantment() {
					Id = 0, Name = "Sharpness",
					MagicCost = 3, BaseValue = 3,
					ApplicableTo = EquipFlags.Weapon
				},
			 new ItemEnchantment() {
					Id = 1, Name = "Spellpower",
					MagicCost = 3, BaseValue = 0,
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

		[Flags]
		public enum EquipFlags
		{
			None = 0,
			First = 1,
			All = 255,

			Weapon = 1,
			Spell = 2,
			Chestplate = 4,
			Helmet = 8,
			Boots = 16,
			Gauntlets = 32,
			Ring = 64,
			Necklace = 128,

			AllArmor = Chestplate | Helmet | Boots | Gauntlets,
			AllJewellery = Ring | Necklace,
		}
	}
}
