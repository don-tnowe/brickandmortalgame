using BrickAndMortal.Scripts.ItemOperations;
using Godot;

namespace BrickAndMortal.Scripts.Menus
{
	class MenuItemBag : BaseMenu
	{
		[Export]
		private PackedScene _sceneItem;

		public bool[] RestrictedItems;
		
		protected GridContainer _nodeItemGrid;
		private ItemStatDisplay _nodeItemStatDisplay;
		protected Tween _nodeTween;

		protected Item[] _itemArray;

		public override void _Ready()
		{
			_nodeItemGrid = GetNode<GridContainer>("Box/Scroll/Box/ItemGrid");
			_nodeItemStatDisplay = GetNode<ItemStatDisplay>("Box/ItemStatDisplay");
			_nodeTween = GetNode<Tween>("/root/Node/UI/Tween");
		}

		private void ItemFocused(Control node, int idx)
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

		protected virtual void ItemSelected(TextureButton node, int idx) { }

		public override void OpenMenu()
		{
			base.OpenMenu();
			_nodeTween.Stop(this);
			_itemArray = SaveData.ItemBag.GetItemArray();
			GetNode<Label>("Money").Text = SaveData.Money.ToString();

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
					newItem.Connect("focus_entered", this, nameof(ItemFocused), new Godot.Collections.Array() { newItem, i });
					newItem.Connect("mouse_entered", this, nameof(ItemFocused), new Godot.Collections.Array() { newItem, i });
					newItem.Connect("pressed", this, nameof(ItemSelected), new Godot.Collections.Array() { newItem, i });
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
				
				// Leftmost column: wrap to last row 
				if (i != 0 && i % columns == 0)
				{
					var to = _nodeItemGrid.GetChild<Control>((i - 1) % _nodeItemGrid.GetChildCount());
					newItem.FocusNeighbourLeft = newItem.GetPathTo(to);
					to.FocusNeighbourRight = to.GetPathTo(newItem);
				}
				
				// Bottom row
				if (i >= _itemArray.Length - columns)
				{
					var to = _nodeItemGrid.GetChild<Control>(i % columns);
					newItem.FocusNeighbourBottom = newItem.GetPathTo(to);
					to.FocusNeighbourTop = to.GetPathTo(newItem);
				}
				
				// Last element: wrap to start
				if (i == _itemArray.Length - 1)
				{
					var to = _nodeItemGrid.GetChild<Control>(0);
					newItem.FocusNeighbourRight = newItem.GetPathTo(to);
					to.FocusNeighbourLeft = to.GetPathTo(newItem);

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
			
			if (RestrictedItems != null)
				for (int i = 0; i < RestrictedItems.Length; i++)
					_nodeItemGrid.GetChild<TextureButton>(i).Disabled = RestrictedItems[i];

			ItemFocused(_nodeItemGrid.GetChild<Control>(_itemArray.Length - 1), _itemArray.Length - 1);

			_nodeTween.InterpolateProperty(this, "modulate",
				new Color(1, 1, 1, 0), new Color(1, 1, 1, 1),
				0.5f
				);
			_nodeTween.Start();
		}
	}
}
