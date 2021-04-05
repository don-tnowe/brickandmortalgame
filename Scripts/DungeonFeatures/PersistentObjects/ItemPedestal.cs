using BrickAndMortal.Scripts.ItemOperations;
using System.Text.Json;
using Godot;

namespace BrickAndMortal.Scripts.DungeonFeatures.PersistentObjects
{
	class ItemPedestal : DungeonPersistentBase
	{
		public Item HeldItem = null;
		
		public override void Initialize()
		{
			HeldItem = GetNode<DungeonBuilder>("../../..").CurPool.GetRandomItem();
			GetNode<Sprite>("Item").Frame = HeldItem.ItemType * 8 + HeldItem.Frame;
		}

		public void UnlockItem()
		{
			HeldItem = null;
			GetNode<AnimationPlayer>("Anim").Play("Open");
		}

		public override string GetSerialized()
		{
			if (HeldItem == null)
				return "{}";
			else
				return HeldItem.ToJSON();
		}

		public override void DeserializeFrom(string from)
		{
			if (!from.Equals("{}"))
			{
				HeldItem = new Item(from);
				GetNode<Sprite>("ViewportTex/Viewport/CaseFront").Frame = Frame;
				GetNode<Sprite>("ViewportTex/CaseBack").Frame = Frame;
				GetNode<Sprite>("Item").Frame = HeldItem.ItemType * 8 + HeldItem.Frame;
			}
			else
				GetNode<AnimationPlayer>("Anim").Play("Open");
		}
		
		
		private void HeroEntered(HeroComponents.Hero body)
		{
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




