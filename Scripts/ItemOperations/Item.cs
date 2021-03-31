using System;

namespace BrickAndMortal.Scripts.ItemOperations
{
    public class Item
    {
        private ItemEnchantment[] AllEnchantments =
        {
             new ItemEnchantment() {
                    Name = "Sharpness",
                    MagicCost = 3, BaseValue = 3,
                    ApplicableTo = EquipType.Weapon
                },
             new ItemEnchantment() {
                    Name = "Spellpower",
                    MagicCost = 3, BaseValue = 0,
                    ApplicableTo = EquipType.Spell
                },
             new ItemEnchantment() {
                    Name = "Protection",
                    MagicCost = 3, BaseValue = 3,
                    ApplicableTo = EquipType.AllArmor
                },
             new ItemEnchantment() {
                    Name = "Iron Damage",
                    MagicCost = 6, BaseValue = 5,
                    ApplicableTo = EquipType.All
                },
             new ItemEnchantment() {
                    Name = "Fire Damage",
                    MagicCost = 6, BaseValue = 10,
                    ApplicableTo = EquipType.All
                },
             new ItemEnchantment() {
                    Name = "Poison Damage",
                    MagicCost = 6, BaseValue = 10,
                    ApplicableTo = EquipType.All
                },
             new ItemEnchantment() {
                    Name = "Magic Damage",
                    MagicCost = 9, BaseValue = 20,
                    ApplicableTo = EquipType.All
                },
             new ItemEnchantment() {
                    Name = "Ghostly Damage",
                    MagicCost = 9, BaseValue = 20,
                    ApplicableTo = EquipType.All
                }
        };

        public int Power;
        public int Shine;
        public int Magic;
        public int[][] HeldEnchantments;
    }

    [Flags]
    public enum EquipType
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
