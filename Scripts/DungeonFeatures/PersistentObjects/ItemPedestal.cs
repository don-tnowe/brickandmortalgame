using BrickAndMortal.Scripts.ItemOperations;
using Godot;

namespace BrickAndMortal.Scripts.DungeonFeatures.PersistentObjects
{
	class ItemPedestal : DungeonPersistentBase
	{
		public Item HeldItem = null;

		public override string SerializedPrefix
		{
			get => Prefixes[1];
		}

		public override void Initialize()
		{
			HeldItem = (Item)GetNode<DungeonBuilder>("../../..").GetRandomItem();
			GetNode<Sprite>("Item").Frame = HeldItem.ItemType * 8 + HeldItem.Frame;
		}

		public void UnlockItem()
		{
			SaveData.ItemBag.CollectItem(HeldItem);
			HeldItem = null;
			GetNode<AnimationPlayer>("Anim").Play("Open");
		}
				
		private void HeroEntered(HeroComponents.Hero hero)
		{
			if (HeldItem == null)
				return;
			var tween = hero.NodeTween;
			var display = hero.GetNode<ItemStatDisplay>("ItemStatDisplay");
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
		
		private void HeroExited(HeroComponents.Hero hero)
		{
			if (HeldItem == null)
				return;
			var tween = hero.NodeTween;
			var display = hero.GetNode<ItemStatDisplay>("ItemStatDisplay");

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

		public override string GetSerialized()
		{
			if (HeldItem == null)
				return SerializedPrefix;
			else
				return SerializedPrefix + HeldItem.ToJSON();
		}

		public override void DeserializeFrom(string from)
		{
			if (!from.Equals(SerializedPrefix))
			{
				HeldItem = new Item(from.Substring(SerializedPrefix.Length));
				GetNode<Sprite>("ViewportTex/Viewport/CaseFront").Frame = Frame;
				GetNode<Sprite>("ViewportTex/CaseBack").Frame = Frame;
				GetNode<Sprite>("Item").Frame = HeldItem.ItemType * 8 + HeldItem.Frame;
			}
			else
				GetNode<AnimationPlayer>("Anim").Play("Open");
		}

	}
}






