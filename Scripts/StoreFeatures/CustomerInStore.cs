using Godot;

namespace BrickAndMortal.Scripts.StoreFeatures
{
	class CustomerInStore : PathFollow2D
	{
		public int CurrentPrice;
		public float DenyChance;
		
		private const string _filePathImageStore = "res://Graphics/Characters/Customers/Store/";
		private const string _filePathImagePortrait = "res://Graphics/Characters/Customers/Portraits/";
		
		private CustomerData _customer;
		private Sprite _nodeSprite;
		
		public void Initialize(CustomerData customer)
		{
			_customer = customer;
			_nodeSprite.Texture = ResourceLoader.Load<StreamTexture>(_filePathImageStore + customer.ImageFilename + ".png");
		}

		public void StartBargaining(object item)
		{
			CurrentPrice = _customer.GetItemStartingPrice(item);
		}

		public void RaisePrice()
		{
			CurrentPrice = _customer.GetIncrementedPrice(CurrentPrice);
			DenyChance = _customer.GetIncrementedDeny(DenyChance);
			if (DenyChance >= 1)
				Deny();
		}

		public void SuperRaisePrice()
		{
			CurrentPrice = _customer.GetIncrementedPrice(CurrentPrice);
			DenyChance = _customer.GetIncrementedDeny(DenyChance);
			if (DenyChance > 0)
				Deny();
		}

		public void Accept()
		{

		}

		public void Deny()
		{

		}
	}
}
