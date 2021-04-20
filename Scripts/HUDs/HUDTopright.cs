using BrickAndMortal.Scripts.ItemOperations;
using Godot;

namespace BrickAndMortal.Scripts.HUDs
{
	public class HUDTopright : Control
	{
		[Export]
		private NodePath _pathHero;

		private const int _heartsPerRow = 10;
		private const int _pointsPerHeart = 10;

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
			_nodeFlyingItem.GetChild<Sprite>(0).Frame = item.ItemType * 8 + item.Frame;

			var hero = GetNode<HeroComponents.Hero>(_pathHero);

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
