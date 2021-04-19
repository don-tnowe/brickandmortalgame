using Godot;

namespace BrickAndMortal.Scripts.Menus
{
	class MenuItemChoose : MenuItemBag
	{
		public override void CloseMenu()
		{
			GetTree().Paused = false;
			GetTree().SetInputAsHandled();
			SetProcessInput(false);

			_nodeTween.InterpolateCallback(this, 0.5f, "queue_free");
			_nodeTween.InterpolateProperty(this, "modulate",
				new Color(1, 1, 1, 1), new Color(1, 1, 1, 0),
				0.5f
				);
			_nodeTween.Start();
		}

		protected override void ItemSelected(TextureButton node, int idx)
		{
			base.ItemSelected(node, idx);
			node?.ReleaseFocus();
			CloseMenu();
		}

		private void ItemClear()
		{
			ItemSelected(null, -1);
		}

		public override void _Input(InputEvent @event)
		{
			if (@event is InputEventMouse)
				return;
			if (@event is InputEventKey && @event.IsEcho())
				return;

			if (@event.IsAction("attack") && @event.GetActionStrength("attack") > 0)
				ItemClear();
			if (@event.IsAction("pause") && @event.GetActionStrength("pause") > 0)
				CloseMenu();
			if (@event.IsAction("ui_cancel") && @event.GetActionStrength("ui_cancel") > 0)
				CloseMenu();
		}
	}
}






