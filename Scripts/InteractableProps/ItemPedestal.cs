using BrickAndMortal.Scripts.ItemOperations;
using Godot;

namespace BrickAndMortal.Scripts.InteractableProps
{
	class ItemPedestal : Sprite
	{
		public Item HeldItem = null;
		public bool HeroLooking = false;
		public bool CanLook = true;


		protected virtual void HeroEntered(HeroComponents.Hero hero)
		{
			HeroLooking = true;
			if (!CanLook)
				return;
			var display = hero.GetNode<ItemStatDisplay>("ItemStatDisplay");
			display.DisplayItemData(HeldItem);
			display.ShowPopup();
		}

		private void HeroExited(HeroComponents.Hero hero)
		{
			HeroLooking = false;
			hero.GetNode<ItemStatDisplay>("ItemStatDisplay").HidePopup();
		}
	}
}






