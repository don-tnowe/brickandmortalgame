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
			_nodeTween = GetNode<Tween>("../../../Tween");
		}

		private void ItemSelected(Control node, int idx)
		{
			_nodeItemStatDisplay.DisplayItemData(_itemArray[idx]);
			node.GrabFocus();

			_nodeTween.InterpolateProperty(_nodeItemStatDisplay, "rect_scale",
				new Vector2(0.9f, 0.9f), Vector2.One,
				0.25f, Tween.TransitionType.Quad, Tween.EaseType.Out
				);
			_nodeTween.InterpolateProperty(node.GetChild(0), "scale",
				new Vector2(2f, 2f), Vector2.One, 
				0.75f, Tween.TransitionType.Elastic, Tween.EaseType.Out
				);
			_nodeTween.InterpolateProperty(node, "modulate",
				new Color(4, 4, 4, 1), new Color(1, 1, 1, 1),
				0.25f, Tween.TransitionType.Quad, Tween.EaseType.Out
				);
			_nodeTween.Start();
		}

		protected virtual void ItemAccepted(Control node, int idx) { }

		public override void OpenMenu()
		{
			base.OpenMenu();
			_nodeTween.Stop(this);
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
				TextureButton newItem;

				if (_nodeItemGrid.GetChildCount() <= i)
				{
					newItem = (TextureButton)_sceneItem.Instance();
					_nodeItemGrid.AddChild(newItem);
					newItem.Connect("focus_entered", this, "ItemSelected", new Godot.Collections.Array() { newItem, i });
					newItem.Connect("mouse_entered", this, "ItemSelected", new Godot.Collections.Array() { newItem, i });
					newItem.Connect("pressed", this, "ItemAccepted", new Godot.Collections.Array() { newItem, i });
				}
				else
					newItem = _nodeItemGrid.GetChild<TextureButton>(i);

				newItem.GetChild<Sprite>(0).Frame = _itemArray[i].ItemType * 8 + _itemArray[i].Frame;

				var columns = _nodeItemGrid.Columns;

				newItem.FocusNeighbourLeft = new NodePath();
				newItem.FocusNeighbourRight = new NodePath();
				newItem.FocusNeighbourTop = new NodePath();
				newItem.FocusNeighbourBottom = new NodePath();
				newItem.FocusNext = new NodePath();
				newItem.FocusPrevious = new NodePath();

				if (i % columns == columns - 1)
				{
					var to = _nodeItemGrid.GetChild<Control>(i - columns + 1);
					newItem.FocusNeighbourRight = newItem.GetPathTo(to);
					to.FocusNeighbourLeft = to.GetPathTo(newItem);
				}

				if (i >= _itemArray.Length - columns)
				{
					var to = _nodeItemGrid.GetChild<Control>(i % columns);
					newItem.FocusNeighbourBottom = newItem.GetPathTo(to);
					to.FocusNeighbourTop = to.GetPathTo(newItem);
				}

				if (i == _itemArray.Length - 1)
				{
					var to = _nodeItemGrid.GetChild<Control>(i / columns * columns);
					newItem.FocusNeighbourRight = newItem.GetPathTo(to);
					to.FocusNeighbourLeft = to.GetPathTo(newItem);

					to = _nodeItemGrid.GetChild<Control>(0);
					newItem.FocusNext = newItem.GetPathTo(to);
					to.FocusPrevious = to.GetPathTo(newItem);
				}

				_nodeTween.InterpolateProperty(newItem, "modulate",
					new Color(4, 4, 4, 1), new Color(1, 1, 1, 1),
					0.25f, Tween.TransitionType.Quad, Tween.EaseType.Out, 0.2f + i * 0.05f
					);
			}
			for (int i = _itemArray.Length; i < _nodeItemGrid.GetChildCount(); i++)
			{
				_nodeItemGrid.GetChild(i).QueueFree();
			}

			ItemSelected(_nodeItemGrid.GetChild<Control>(_itemArray.Length - 1), _itemArray.Length - 1);

			_nodeTween.Start();
		}
	}
}
