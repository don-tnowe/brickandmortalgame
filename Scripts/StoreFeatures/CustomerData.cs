using BrickAndMortal.Scripts.ItemOperations;
using System.Collections.Generic;
using Godot;

namespace BrickAndMortal.Scripts.StoreFeatures
{
	class CustomerData : Resource
	{
		[Export]
		public string ImageFilename;
		[Export]
		private EquipFlags[] _needsTypes = new EquipFlags[] { EquipFlags.Weapon, EquipFlags.Spell, EquipFlags.AllArmor };
		[Export]
		private int[] _needsEnchants = new int[] { 0, 1, 2 };
		[Export]
		private int _needCount = 1;
		[Export]
		private float _minPriceMultiplier = -0.4f;
		[Export]
		private float _denyPriceMultiplier = 2;
		[Export]
		private float _priceIncrementMultiplier = 0.25f;
		[Export]
		private float _denyChanceInit = -1;
		[Export]
		private float _denyChanceIncrement = 0.2f;
		[Export]
		private float _multiplierPower = 1;
		[Export]
		private float _multiplierShine = 4;
		[Export]
		private float _multiplierMagic = 0;
		[Export]
		private Dictionary<int, float> _multipliersEnchants = new Dictionary<int, float> {
			{0, 1},
			{1, 1},
			{2, 1},
			{3, 1},
			{4, 1}
		};

		public EquipFlags LastOrderEquipFlags;
		public int[] LastOrderEnchants;
		public int LastOrderStartPrice;
		public int LastOrderDenyPrice;

		private System.Random _random = new System.Random();

		public void NewOrder()
		{
			LastOrderEquipFlags = _needsTypes[_random.Next(_needsTypes.Length)];
			LastOrderEnchants = ItemData.GetRandomEnchants(_needsEnchants, _needCount, LastOrderEquipFlags, _random);
		}

		public bool WillBuyItem(Item item)
		{
			if (((EquipFlags)(1 << item.ItemType) & LastOrderEquipFlags) == 0)
				return false;
			for (int i = 0; i < LastOrderEnchants.Length; i++)
			{
				var found = false;
				for (int j = 0; j < item.HeldEnchantments[0].Length; j++)
				{
					if (item.HeldEnchantments[0][j] == LastOrderEnchants[i])
						found = true;
				}
				if (!found)
					return false;
			}
			return true;
		}


		public int GetItemStartingPrice(object item)
		{
			return GetItemStartingPrice((Item)item);
		}

		public int GetItemStartingPrice(Item item)
		{
			float price = (
				item.Power * _multiplierPower
				+ item.Shine * _multiplierShine
				+ item.Magic * _multiplierMagic
				);
			for (int i = 0; i < item.HeldEnchantments[0].Length; i++)
				if (_multipliersEnchants.ContainsKey(item.HeldEnchantments[0][i]))
					price += item.HeldEnchantments[1][i] * ItemData.AllEnchantments[item.HeldEnchantments[1][i]].BaseValue * _multipliersEnchants[i];
			if (price < 1)
				price = 1;
			LastOrderDenyPrice = (int)(price * _denyPriceMultiplier);
			price *= (float)(1.0 + _random.NextDouble() * (_minPriceMultiplier - 1));
			LastOrderStartPrice = (int)price;
			return (int)price;
		}

		public float GetIncrementedDeny(float currentDeny)
		{
			if (currentDeny < _denyChanceInit)
				return 0;
			if (_random.NextDouble() < currentDeny - _denyChanceInit)
				return 1;
			return _denyChanceIncrement;
		}

		public int GetIncrementedPrice(int currentPrice)
		{
			return _random.Next((int)(_denyChanceInit * _priceIncrementMultiplier));
		}
	}
}
