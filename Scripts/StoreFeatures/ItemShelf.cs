using BrickAndMortal.Scripts.ItemOperations;
using Godot;

namespace BrickAndMortal.Scripts.StoreFeatures
{
    class ItemShelf : Sprite
    {
        public bool CanSell {
			get { return _canSell; } 
			set { 
				_canSell = value;
				if (value)
					Modulate = new Color(1, 1, 1, 1);
				else
					Modulate = new Color(0.5f, 0.5f, 0.5f, 1);
			}
		}

		public Item HeldItem = null;

		private bool _canSell = false;

		public void SetItem(Item newItem)
		{
			HeldItem = newItem;
			GetNode<Sprite>("Item").Frame = HeldItem.ItemType * 8 + HeldItem.Frame;
		}

		private void HeroEntered(HeroComponents.Hero body)
		{
			if (!CanSell)
				return;
			if (HeldItem == null)
				return;

			var tween = body.NodeTween;
			var display = body.GetNode<ItemStatDisplay>("ItemStatDisplay");
			display.DisplayItemData(HeldItem);

			tween.Stop(display);
			tween.InterpolateProperty(display, "rect_position",
				display.RectPosition, new Vector2(-33, -87),
				0.5f, Tween.TransitionType.Quad, Tween.EaseType.Out
				);
			tween.InterpolateProperty(display, "modulate",
				new Color(0, 0, 0, 0), new Color(1, 1, 1, 1),
				0.5f
				);

			tween.Start();
		}

		private void HeroExited(HeroComponents.Hero body)
		{
			if (HeldItem == null)
				return;
			var tween = body.NodeTween;
			var display = body.GetNode<ItemStatDisplay>("ItemStatDisplay");

			tween.Stop(display);
			tween.InterpolateProperty(display, "rect_position",
				display.RectPosition, new Vector2(-33, -75),
				0.5f, Tween.TransitionType.Quad, Tween.EaseType.In
				);
			tween.InterpolateProperty(display, "modulate",
				display.Modulate, new Color(0, 0, 0, 0),
				0.5f
				);
			tween.InterpolateCallback(display, 0.5f, "hide");


			tween.Start();
		}
	}
}
