using BrickAndMortal.Scripts.ItemOperations;
using Godot;

namespace BrickAndMortal.Scripts.StoreFeatures
{
	class CustomerInStore : PathFollow2D
	{
		public int NegotiationItemUid;
		public int CurrentPrice = 1;
		public float CurrentDeny = 0;
		
		private const string _filePathImageStore = "res://Graphics/Characters/Customers/Store/";
		private const string _filePathImagePortrait = "res://Graphics/Characters/Customers/Portraits/";
		
		private CustomerData _customer;

		public void Initialize(CustomerData customer)
		{
			_customer = customer;
			GetNode<Sprite>("Sprite").Texture = ResourceLoader.Load<StreamTexture>(_filePathImageStore + customer.ImageFilename + ".png");
			_customer.NewOrder();
		}

		public void StartNegotiation(Item item)
		{
			NegotiationItemUid = item.Uid;
			CurrentPrice = _customer.GetItemStartingPrice(item);
		}

		public bool RaisePrice()
		{
			CurrentPrice = _customer.GetIncrementedPrice(CurrentPrice);
			CurrentDeny = _customer.GetIncrementedDeny(CurrentPrice, CurrentDeny);
			return _customer.Denied;
		}

		public bool SuperRaisePrice()
		{
			CurrentPrice = _customer.GetSuperIncrementedPrice(CurrentPrice);
			CurrentDeny = _customer.GetSuperIncrementedDeny(CurrentPrice, CurrentDeny);
			return _customer.Denied;
		}

		public bool WillBuyItem(Item item)
		{
			if (item == null)
				return false;
			return _customer.WillBuyItem(item);
		}

			public void DisplayRequest(Control bubble)
		{
			_customer.DisplayRequest(bubble);
		}

		public int GetPriceOpinion()
		{
			return _customer.GetPriceOpinion(CurrentPrice, CurrentDeny);
		}

		public void PlayAnimation(string name)
		{
			GetNode<AnimationPlayer>("Anim").Play(name);
		}
	}
}
