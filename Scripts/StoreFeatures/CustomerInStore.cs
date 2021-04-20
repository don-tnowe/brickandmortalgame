using BrickAndMortal.Scripts.ItemOperations;
using Godot;

namespace BrickAndMortal.Scripts.StoreFeatures
{
	class CustomerInStore : PathFollow2D
	{
		public int NegotiationItemUid;

		public CustomerData Customer
		{
			set
			{
				_customer = value;
				GetNode<Sprite>("Sprite").Texture = ResourceLoader.Load<StreamTexture>(_filePathImageStore + value.ImageFilename + ".png");
			}
			get { return _customer; }
		}

		private CustomerData _customer;

		private const string _filePathImageStore = "res://Graphics/Characters/Customers/Store/";
		private const string _filePathImagePortrait = "res://Graphics/Characters/Customers/Portraits/";
	
		public void PlayAnimation(string name)
		{
			GetNode<AnimationPlayer>("Anim").Play(name);
		}
	}
}
