using BrickAndMortal.Scripts.ItemOperations;
using Godot;

namespace BrickAndMortal.Scripts.HUDs
{
	public class HUDTopright : Control
	{
		[Export]
		private NodePath _pathHero;

		private Node2D _nodeFlyingItem;
		private Label _nodeLabelItemcount;
		private Control _nodeAnchorCenter;
		private Control _nodeBackpackCenter;
		private AnimationPlayer _nodeAnim;

		public override void _Ready()
		{
			_nodeFlyingItem = GetNode<Node2D>("AnchorCenter/FlyingItem");
			_nodeLabelItemcount = GetNode<Label>("AnchorTR/Control/ItemCount");
			_nodeAnchorCenter = GetNode<Control>("AnchorCenter");
			_nodeBackpackCenter = GetNode<Control>("AnchorTR/Control");
			_nodeAnim = GetNode<AnimationPlayer>("Anim");
			
			SaveData.ItemBag.EventAddedItem += ItemCollect;
			CallDeferred("UpdateItemCount");
		}

		private void ItemCollect(Item item)
		{
			if (_nodeFlyingItem.GetChildCount() > 0)
				_nodeFlyingItem.GetChild<Sprite>(0).Frame = item.ItemType * 8 + item.Frame;

			var hero = GetNode<HeroComponents.Hero>(_pathHero);

			// TODO Fix THIS
			
			/*
			E 0:01:59.034   track_remove_key: Index p_idx = 1 is out of bounds (vt->values.size() = 1).
  <C++ Source>  scene/resources/animation.cpp:909 @ track_remove_key()
			E 0:01:59.034   track_remove_key: Index p_idx = 1 is out of bounds (vt->values.size() = 1).
  <C++ Source>  scene/resources/animation.cpp:909 @ track_remove_key()
			E 0:02:24.665   godot_icall_0_1: Parameter "ptr" is null.
  <C++ Source>  modules/mono/glue/mono_glue.gen.cpp:14 @ godot_icall_0_1()
  <Stack Trace> :0 @ Int32 Godot.NativeCalls.godot_icall_0_1(IntPtr , IntPtr )()
				Node.cs:550 @ Int32 Godot.Node.GetChildCount()()
				HUDTopright.cs:31 @ void BrickAndMortal.Scripts.HUDs.HUDTopright.ItemCollect(BrickAndMortal.Scripts.ItemOperations.Item )()
				ItemBag.cs:22 @ void BrickAndMortal.Scripts.ItemOperations.ItemBag.CollectItem(BrickAndMortal.Scripts.ItemOperations.Item )()
				ItemPedestalDungeon.cs:42 @ void BrickAndMortal.Scripts.DungeonFeatures.PersistentObjects.ItemPedestalDungeon.UnlockItem()()
				:0 @ void Godot.NativeCalls.godot_icall_2_598(IntPtr , IntPtr , System.String , System.Object[] )()
				Object.cs:361 @ void Godot.Object.EmitSignal(System.String , System.Object[] )()
				Room.cs:50 @ void BrickAndMortal.Scripts.DungeonFeatures.Room.EnemyDefeated(Int32 )()
				:0 @ void Godot.NativeCalls.godot_icall_2_598(IntPtr , IntPtr , System.String , System.Object[] )()
				Object.cs:361 @ void Godot.Object.EmitSignal(System.String , System.Object[] )()
				BaseEnemy.cs:122 @ void BrickAndMortal.Scripts.Combat.Enemies.BaseEnemy.Vanquished()()
				:0 @ void Godot.NativeCalls.godot_icall_2_598(IntPtr , IntPtr , System.String , System.Object[] )()
				Object.cs:361 @ void Godot.Object.EmitSignal(System.String , System.Object[] )()
				CombatActor.cs:45 @ void BrickAndMortal.Scripts.Combat.CombatActor.set_Health(Int32 )()
				CombatActor.cs:83 @ void BrickAndMortal.Scripts.Combat.CombatActor.Hurt(BrickAndMortal.Scripts.Combat.CombatAttack )()
			E 0:02:24.921   IntPtr Godot.Object.GetPtr(Godot.Object ): System.ObjectDisposedException: Cannot access a disposed object.
Object name: 'BrickAndMortal.Scripts.HUDs.HUDTopright'.
  <C++ Error>   Unhandled exception
  <C++ Source>  /root/godot/modules/mono/glue/GodotSharp/GodotSharp/Core/Object.base.cs:37 @ IntPtr Godot.Object.GetPtr(Godot.Object )()
  <Stack Trace> Object.base.cs:37 @ IntPtr Godot.Object.GetPtr(Godot.Object )()
				Node.cs:618 @ Godot.Node Godot.Node.GetNode(Godot.NodePath )()
				NodeExtensions.cs:7 @ BrickAndMortal.Scripts.HeroComponents.Hero Godot.Node.GetNode<BrickAndMortal.Scripts.HeroComponents.Hero >(Godot.NodePath )()
				HUDTopright.cs:34 @ void BrickAndMortal.Scripts.HUDs.HUDTopright.ItemCollect(BrickAndMortal.Scripts.ItemOperations.Item )()
				:0 @ ()
				ItemBag.cs:22 @ void BrickAndMortal.Scripts.ItemOperations.ItemBag.CollectItem(BrickAndMortal.Scripts.ItemOperations.Item )()
				ItemPedestalDungeon.cs:42 @ void BrickAndMortal.Scripts.DungeonFeatures.PersistentObjects.ItemPedestalDungeon.UnlockItem()()
				:0 @ ()
				Object.cs:361 @ void Godot.Object.EmitSignal(System.String , System.Object[] )()
				Room.cs:50 @ void BrickAndMortal.Scripts.DungeonFeatures.Room.EnemyDefeated(Int32 )()
				:0 @ ()
				Object.cs:361 @ void Godot.Object.EmitSignal(System.String , System.Object[] )()
				BaseEnemy.cs:122 @ void BrickAndMortal.Scripts.Combat.Enemies.BaseEnemy.Vanquished()()
				:0 @ ()
				Object.cs:361 @ void Godot.Object.EmitSignal(System.String , System.Object[] )()
				CombatActor.cs:45 @ void BrickAndMortal.Scripts.Combat.CombatActor.set_Health(Int32 )()
				CombatActor.cs:83 @ void BrickAndMortal.Scripts.Combat.CombatActor.Hurt(BrickAndMortal.Scripts.Combat.CombatAttack )()

			*/

			_nodeFlyingItem.Position = hero.GlobalPosition - hero.NodeCam.GetCameraScreenCenter();

			_nodeAnim.GetAnimation("GainItem").BezierTrackSetKeyValue(0, 0, _nodeFlyingItem.Position.x);
			_nodeAnim.GetAnimation("GainItem").BezierTrackSetKeyValue(1, 0, _nodeFlyingItem.Position.y);
			_nodeAnim.GetAnimation("GainItem").BezierTrackSetKeyValue(0, 1, _nodeBackpackCenter.RectGlobalPosition.x - _nodeAnchorCenter.RectPosition.x);
			_nodeAnim.GetAnimation("GainItem").BezierTrackSetKeyValue(1, 1, _nodeBackpackCenter.RectGlobalPosition.y - _nodeAnchorCenter.RectPosition.y);
			_nodeAnim.Play("GainItem");
		}

		private void UpdateItemCount()
		{
			_nodeLabelItemcount.Text = SaveData.ItemBag.GetItemCount().ToString();
		}
	}
}
