using Godot;

namespace BrickAndMortal.Scripts.Menus
{
    abstract class BaseMenu : Control
    {
        [Export]
        private NodePath _pathPageLeft;
        [Export]
        private NodePath _pathPageRight;
        [Export]
        private string _shortcutAction;

        private bool _isOpen;

        [Signal]
        private delegate void Opened();
        [Signal]
        private delegate void Closed();

        public virtual void OpenMenu()
        {
            EmitSignal("Opened");
            _isOpen = true;
            GetTree().Paused = true;
        }

        public virtual void CloseMenu()
        {
            HideMenu();
            EmitSignal("Closed");
            GetTree().Paused = false;
        }

        public virtual void HideMenu()
        {
            _isOpen = false;
        }

        public override void _Input(InputEvent @event)
        {
            if (@event.IsAction(_shortcutAction) && @event.GetActionStrength(_shortcutAction) > 0)
                if (!_isOpen)
                    OpenMenu();
                else
                    CloseMenu();
            if (!_isOpen)
                return;
            else if (@event.IsAction("ui_cancel") && @event.GetActionStrength("ui_cancel") > 0)
            {
                CloseMenu();
            }
        }
    }
}
