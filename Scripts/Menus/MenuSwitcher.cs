using Godot;

namespace BrickAndMortal.Scripts.Menus
{
	class MenuSwitcher : Control
	{
		[Export]
		private bool _loops = true;

		private int _selected = 1;
		private bool _isOpen = false;

		private Label[] _nodesNavLabels = new Label[3];
		private Node _nodeMenus;
		private Tween _nodeTween;

		public override void _Ready()
		{
			_nodeMenus = GetNode("Menus");
			_nodeTween = GetNode<Tween>("../Tween");
			
			for (int i = 0; i < 3; i++)
				_nodesNavLabels[i] = GetNode("Top/Labels").GetChild<Label>(i);
		}

		private void SwitchMenu(int direction)
		{
			var newSelected = Mathf.PosMod(_selected + direction, _nodeMenus.GetChildCount());
			if (!_loops && newSelected != _selected + direction)
				return;

			var node = _nodeMenus.GetChild<BaseMenu>(newSelected);
			node.OpenMenu();
			_nodesNavLabels[1].Text = node.Title;

			_nodesNavLabels[0].Text = _nodeMenus.GetChild<BaseMenu>(Mathf.PosMod(newSelected - 1, _nodeMenus.GetChildCount())).Title;
			_nodesNavLabels[2].Text = _nodeMenus.GetChild<BaseMenu>(Mathf.PosMod(newSelected + 1, _nodeMenus.GetChildCount())).Title;

			_nodeTween.InterpolateProperty(GetNode("Top"), "margin_top",
				24, 16,
				0.15f, Tween.TransitionType.Quart, Tween.EaseType.Out
				);

			_nodeTween.InterpolateProperty(node, "anchor_left",
				direction, 0,
				0.15f, Tween.TransitionType.Quart, Tween.EaseType.Out
				);
			_nodeTween.InterpolateProperty(node, "anchor_right",
				direction + 1, 1,
				0.15f, Tween.TransitionType.Quart, Tween.EaseType.Out
				);

			node = _nodeMenus.GetChild<BaseMenu>(_selected);
			_nodeTween.InterpolateProperty(node, "anchor_left",
				0, -direction,
				0.15f, Tween.TransitionType.Quart, Tween.EaseType.Out
				);
			_nodeTween.InterpolateProperty(node, "anchor_right",
				1, 1 - direction,
 				0.15f, Tween.TransitionType.Quart, Tween.EaseType.Out
				);

			_nodeTween.Start();

			_selected = newSelected;
		}

		private void OpenMenu()
		{
			_selected = 0;
			_isOpen = true;
			GetTree().Paused = true;
			_nodeMenus.GetChild<BaseMenu>(0).OpenMenu();

			Visible = true;
			for (int i = 0; i < _nodeMenus.GetChildCount(); i++)
			{
				var node = _nodeMenus.GetChild<BaseMenu>(i);
				node.AnchorLeft = i;
				node.AnchorRight = i + 1;
			}

			_nodeTween.Stop(this);
			_nodeTween.InterpolateProperty(this, "modulate",
				new Color(1, 1, 1, 0), new Color(1, 1, 1, 1),
				0.25f
				);
			_nodeTween.Start();

		}

		private void CloseMenu()
		{
			_nodeMenus.GetChild<BaseMenu>(_selected).CloseMenu();
			GetTree().Paused = false;
			_isOpen = false;

			_nodeTween.InterpolateProperty(this, "modulate",
				new Color(1, 1, 1, 1), new Color(1, 1, 1, 0),
				0.25f
				);
			_nodeTween.Start();
			_nodeTween.InterpolateCallback(this, 0.25f, "set_visible", false);
		}

		public override void _Input(InputEvent @event)
		{
			if (@event is InputEventMouse)
				return;
			if (@event is InputEventKey && @event.IsEcho())
				return;

			if (@event.IsAction("pause") && @event.GetActionStrength("pause") > 0)
				if (!_isOpen)
					OpenMenu();
				else
					CloseMenu();
			if (!_isOpen)
				return;

			if (@event.IsAction("ui_page_up") && @event.GetActionStrength("ui_page_up") > 0)
			{
				SwitchMenu(-1);
			}
			if (@event.IsAction("ui_page_down") && @event.GetActionStrength("ui_page_down") > 0)
			{
				SwitchMenu(1);
			}
			if (@event.IsAction("ui_cancel") && @event.GetActionStrength("ui_cancel") > 0)
			{
				CloseMenu();
			}
		}
	}
}





