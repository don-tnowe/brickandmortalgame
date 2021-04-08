using BrickAndMortal.Scripts.ItemOperations;
using Godot;

namespace BrickAndMortal.Scripts.Menus
{
	class MenuItemBag : BaseMenu
	{
		[Export]
		private PackedScene _sceneItem;

		private GridContainer _nodeItemGrid;
		private ItemStatDisplay _nodeItemStatDisplay;
		private Tween _nodeTween;

		private Item[] _itemArray;

		public override void _Ready()
		{
			_nodeItemGrid = GetNode<GridContainer>("Box/Scroll/Box/ItemGrid");
			_nodeItemStatDisplay = GetNode<ItemStatDisplay>("Box/ItemStatDisplay");
			_nodeTween = GetNode<Tween>("../Tween");
		}

		private void ItemSelected(Control node, int idx)
		{
			_nodeItemStatDisplay.DisplayItemData(_itemArray[idx]);


			_nodeTween.InterpolateProperty(_nodeItemStatDisplay, "rect_scale",
				new Vector2(0.9f, 0.9f), Vector2.One,
				0.25f, Tween.TransitionType.Quad, Tween.EaseType.Out
				);
			_nodeTween.InterpolateProperty(node, "rect_scale",
				new Vector2(1.5f, 1.5f), Vector2.One, 
				0.5f, Tween.TransitionType.Elastic, Tween.EaseType.Out
				);
			_nodeTween.InterpolateProperty(node, "modulate",
				new Color(4, 4, 4, 1), new Color(1, 1, 1, 1),
				0.25f
				);
			_nodeTween.Start();
		}

		public override void OpenMenu()
		{
			base.OpenMenu();
			_nodeTween.Stop(this);
			Modulate = new Color(1, 1, 1, 1);
			Visible = true;
			_itemArray = SaveData.ItemBag.GetItemArray();

			if (_itemArray.Length == 0)
			{
				_nodeItemStatDisplay.Visible = false;
				GetNode<CanvasItem>("MsgBagEmpty").Visible = true;
				return;
			}

			_nodeItemStatDisplay.Visible = true;
			GetNode<CanvasItem>("MsgBagEmpty").Visible = false;

			for (int i = 0; i < _itemArray.Length; i++)
			{
				if (_nodeItemGrid.GetChildCount() <= i)
				{
					var newItem = _sceneItem.Instance();
					_nodeItemGrid.AddChild(newItem);
					newItem.Connect("focus_entered", this, "ItemSelected", new Godot.Collections.Array() { newItem, i });
				}
				_nodeItemGrid.GetChild(i).GetChild<Sprite>(0).Frame = _itemArray[i].ItemType * 8 + _itemArray[i].Frame;
				_nodeTween.InterpolateProperty(_nodeItemGrid.GetChild(i), "rect_scale",
					new Vector2(1.5f, 1.5f), Vector2.One,
					0.5f, Tween.TransitionType.Elastic, Tween.EaseType.Out, i * 0.05f
					);
			}
			for (int i = _itemArray.Length; i < _nodeItemGrid.GetChildCount(); i++)
			{
				_nodeItemGrid.GetChild(i).QueueFree();
			}

			_nodeItemGrid.GetChild<Control>(_itemArray.Length - 1).GrabFocus();
			ItemSelected(_nodeItemGrid.GetChild<Control>(_itemArray.Length - 1), _itemArray.Length - 1);

			_nodeTween.Start();
		}

		public override void CloseMenu()
		{
			base.CloseMenu();
			_nodeTween.InterpolateProperty(this, "modulate",
				new Color(1, 1, 1, 1), new Color(1, 1, 1, 0),
				0.25f
				);
			_nodeTween.Start();
			_nodeTween.InterpolateCallback(this, 0.25f, "set_visible", false);
		}
	}
}
