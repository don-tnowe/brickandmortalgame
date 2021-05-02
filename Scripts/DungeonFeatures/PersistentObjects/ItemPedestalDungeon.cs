using BrickAndMortal.Scripts.ItemOperations;
using Godot;

namespace BrickAndMortal.Scripts.DungeonFeatures.PersistentObjects
{
	class ItemPedestalDungeon : InteractableProps.ItemPedestal, IDungeonPersistent
	{
		/* Doesn't work. No error, but doesn't emit if connected from code.
		public override void _Ready()
		{
			GetNode<DungeonBuilder>("/root/Node").CurRoom.Connect(
				nameof(Room.AllEnemiesDefeated), this, nameof(UnlockItem)
				);
		}
		*/
		
		public string GetSerializedPrefix()
		{
			return "ItemPedestal ||";
		}

		public void Initialize()
		{
			HeldItem = GetNode<DungeonBuilder>("../../..").GetRandomItem();
			GetNode<Sprite>("Item").Frame = HeldItem.ItemType * 8 + HeldItem.Frame;
		}

		public string GetSerialized()
		{
			if (HeldItem == null)
				return GetSerializedPrefix();
			else
				return GetSerializedPrefix() + HeldItem.ToJSON();
		}

		public void DeserializeFrom(string from)
		{
			if (!from.Equals(GetSerializedPrefix()))
			{
				HeldItem = new Item(from.Substring(GetSerializedPrefix().Length));
				GetNode<Sprite>("ViewportTex/Viewport/CaseFront").Frame = Frame;
				GetNode<Sprite>("ViewportTex/CaseBack").Frame = Frame;
				GetNode<Sprite>("Item").Frame = HeldItem.ItemType * 8 + HeldItem.Frame;
			}
			else
				GetNode<AnimationPlayer>("Anim").Play("Opened");
		}

		public void UnlockItem()
		{
			SaveData.ItemBag.CollectItem(HeldItem);
			HeldItem = null;
			CanLook = false;
			GetNode<AnimationPlayer>("Anim").Play("Open");
		}
	}
}







