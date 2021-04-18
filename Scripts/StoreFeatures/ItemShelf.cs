
using BrickAndMortal.Scripts.ItemOperations;
using Godot;

namespace BrickAndMortal.Scripts.StoreFeatures
{
	class ItemShelf : InteractableProps.ItemPedestal
	{
		public bool CanChoose = true;
		public AnimationPlayer NodeAnim;

		public bool CanSell
		{
			set
			{
				_canSell = value;
				CanChoose = false;
				CanLook = value;
				if (value && _heldItemIdx != -1)
					NodeAnim.Play("CanSell");
				else
					NodeAnim.Play("CantSell");
			}
			get
			{
				return _canSell;
			}
		}

		private int _heldItemIdx = -1;

		private StoreManager _nodeStoreManager;

		private bool _canSell = false;

		public override void _Ready()
		{
			base._Ready();
			_nodeStoreManager = GetNode<StoreManager>("../../..");
			NodeAnim = GetNode<AnimationPlayer>("Anim");
		}

		protected override void HeroEntered(HeroComponents.Hero hero)
		{
			HeroLooking = true;
			if (!CanLook)
				return;

			var message = "";
			if (CanChoose)
				message = "StorePromptShelf";
			else if (_canSell)
				message = "StorePromptSell";

			var display = hero.GetNode<ItemStatDisplay>("ItemStatDisplay");
			display.DisplayItemData(message, HeldItem);
			display.ShowPopup();
		}

		private void Interacted(object with)
		{
			if (CanChoose)
				_nodeStoreManager.ChooseShelfItem(this);
			else if (_canSell)
				_nodeStoreManager.SellFromShelf(this);
		}
		
		public void SetItem(Item newItem, int idx)
		{
			HeldItem = newItem;

			if (_heldItemIdx != -1)
				_nodeStoreManager.ItemsOnShelves[_heldItemIdx] = false;
			if (idx != -1)
				_nodeStoreManager.ItemsOnShelves[idx] = true;

			_heldItemIdx = idx;

			if (idx != -1)
				NodeAnim.Play("HasItem");
			else
				NodeAnim.Play("NoItem");

			if (newItem != null)
				GetNode<Sprite>("Item").Frame = newItem.ItemType * 8 + newItem.Frame;
		}
	}
}






