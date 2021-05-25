using BrickAndMortal.Scripts.ItemOperations;
using System.Collections.Generic;
using Godot;

namespace BrickAndMortal.Scripts.StoreFeatures
{
	class CustomerData : Resource
	{
		[Export]
		private int id;
		[Export]
		private EquipFlags[] _needsTypes = new EquipFlags[] { EquipFlags.Weapon, EquipFlags.Spell, EquipFlags.AllArmor };
		[Export]
		private int[] _needsEnchants = new int[] { 0, 1, 2 };
		[Export]
		private int _needsMin = 0;
		[Export]
		private int _needsMax = 1;
		[Export]
		private float _minPriceMultiplier = 0.6f;
		[Export]
		private float _denyPriceMultiplier = 2;
		[Export]
		private float _priceIncrementMultiplier = 0.2f;
		[Export]
		private float _denyChanceInit = -1;
		[Export]
		private float _denyChanceIncrement = 0.2f;
		[Export]
		private Curve _opinionCurve = new Curve();
		[Export]
		private int _opinionPersonality = 1;
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
		
		public int CurrentPrice;
		public float CurrentDeny;
		
		private EquipFlags _lastOrderEquipFlags;
		private ItemEnchantment[] _lastOrderEnchants;
		private int _lastOrderStartPrice;
		private int _lastOrderDenyPrice;

		private System.Random _random;

		public void Initialize(System.Random randomizer)
		{
			_random = randomizer;
			_lastOrderEquipFlags = _needsTypes[_random.Next(_needsTypes.Length)];
			_lastOrderEnchants = ItemEnchantment.GetRandomEnchants(_needsEnchants, _needsMin + _random.Next(_needsMax - _needsMin + 1), _lastOrderEquipFlags, _random);
			CurrentDeny = _denyChanceInit;
		}

		public bool WillBuyItem(Item item)
		{
			if (item == null)
				return false;

			if (((EquipFlags)(1 << item.ItemType) & _lastOrderEquipFlags) == 0)
				return false;

			for (int i = 0; i < _lastOrderEnchants.Length; i++)
			{
				var found = false;
				for (int j = 0; j < item.HeldEnchantments.Length; j++)
					if (item.HeldEnchantmentValues[j] > 0 && item.HeldEnchantments[j] == _lastOrderEnchants[i])
						found = true;

				if (!found)
					return false;
			}
			return true;
		}

		public void StartNegotiation(Item item)
		{
			float price = 
				item.Power * _multiplierPower
				+ item.Shine * _multiplierShine
				+ item.Magic * _multiplierMagic
				;

			for (int i = 0; i < item.HeldEnchantments.Length; i++)
			{
				var ench = item.HeldEnchantments[i];
				var value = item.HeldEnchantmentValues[i];

				if (ench != null && _multipliersEnchants.ContainsKey(ench.Id))
					price += value * ench.BaseValue * _multipliersEnchants[ench.Id];
			}

			if (price < 1)
				price = 1;

			_lastOrderDenyPrice = (int)(price * _denyPriceMultiplier);
			_lastOrderStartPrice = (int)price;
			
			CurrentPrice = (int)(price * (_random.NextDouble() * 0.5 + 0.5) * _minPriceMultiplier);
		}

		public void IncrementDeny()
		{
			if (CurrentPrice > _lastOrderDenyPrice)
				CurrentDeny += _denyChanceIncrement;
		}

		public void IncrementPrice()
		{
			var halfIncrement = (int)(_priceIncrementMultiplier * 0.5f * CurrentPrice);
			CurrentPrice += (_random.Next(halfIncrement) + halfIncrement);
		}

		public void SuperIncrementPrice()
		{
			var halfIncrement = (int)(_priceIncrementMultiplier * CurrentPrice);
			CurrentPrice += _random.Next(halfIncrement) + halfIncrement;
		}

		public int GetPriceOpinion()
		{
			float opinion;
			
			if (CurrentPrice < _lastOrderDenyPrice)
				opinion = (float)CurrentPrice / _lastOrderDenyPrice * 2;
				
			else if (CurrentDeny + _denyChanceInit <= 0)
				opinion = 2 - CurrentDeny / _denyChanceInit;
				
			else
				opinion = 3 + (CurrentDeny + _denyChanceInit);
				
			return (int)(_opinionCurve.Interpolate(opinion / 4) * 16);
		}
		
		public int GetOpinionPersonality()
		{
			return _opinionPersonality;
		}
		
		public void DisplayRequest(Control bubble)
		{
			var box = bubble.GetNode<Control>("Clip/Box");

			var equipFrame = 0;
			switch (_lastOrderEquipFlags)
			{
				case EquipFlags.Weapon:
					equipFrame = 0;
					break;
				case EquipFlags.Spell:
					equipFrame = 1;
					break;

				case EquipFlags.Chestplate:
					equipFrame = 2;
					break;
				case EquipFlags.Helmet:
					equipFrame = 3;
					break;
				case EquipFlags.Boots:
					equipFrame = 4;
					break;
				case EquipFlags.Gauntlets:
					equipFrame = 5;
					break;

				case EquipFlags.Ring:
					equipFrame = 6;
					break;
				case EquipFlags.Necklace:
					equipFrame = 7;
					break;

				case EquipFlags.WeaponOrSpell:
					equipFrame = 16;
					break;
				case EquipFlags.AllArmor:
					equipFrame = 17;
					break;
				case EquipFlags.AllJewellery:
					equipFrame = 18;
					break;
			};

			box.GetChild(0).GetChild<Sprite>(0).Frame = equipFrame;

			for (int i = 1; i < box.GetChildCount(); i++)
				if (_lastOrderEnchants.Length < i)
				{
					box.GetChild<CanvasItem>(i).Visible = false;
				}
				else
				{
					box.GetChild<CanvasItem>(i).Visible = true;
					box.GetChild(i).GetChild<Sprite>(0).Frame = _lastOrderEnchants[i - 1].Id;
				}
			box.RectSize = new Vector2(0, box.RectSize.y);
		}
	}
}
