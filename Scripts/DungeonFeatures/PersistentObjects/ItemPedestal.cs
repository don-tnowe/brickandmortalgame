using BrickAndMortal.Scripts.ItemOperations;
using System.Text.Json;
using Godot;

namespace BrickAndMortal.Scripts.DungeonFeatures.PersistentObjects
{
	class ItemPedestal : DungeonPersistentBase
	{
		public Item item = null;
		
		public override void Initialize()
		{
			item = GetNode<DungeonBuilder>("../../..").CurPool.GetRandomItem();
			GetNode<Sprite>("Item").Frame = item.ItemType * 8 + item.Frame;
		}

		public void UnlockItem()
		{
			item = null;
			GetNode<AnimationPlayer>("Anim").Play("Open");
		}

		public override string GetSerialized()
		{
			if (item == null)
				return "{}";
			else
				return item.ToJSON();
		}

		public override void DeserializeFrom(string from)
		{
			if (!from.Equals("{}"))
			{
				item = new Item(from);
				GetNode<Sprite>("ViewportTex/Viewport/CaseFront").Frame = Frame;
				GetNode<Sprite>("ViewportTex/CaseBack").Frame = Frame;
				GetNode<Sprite>("Item").Frame = item.ItemType * 8 + item.Frame;
			}
			else
				GetNode<AnimationPlayer>("Anim").Play("Open");
		}
		
		
		private void HeroEntered(HeroComponents.Hero body)
		{
			if (item == null)
				return;
			var tween = body.NodeTween;
			var display = body.GetNode<ItemStatDisplay>("ItemStatDisplay");
			display.DisplayItemData(item);

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
			if (item == null)
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




