using Godot;

namespace BrickAndMortal.Scripts.Menus
{
	abstract class BaseMenu : Control
	{
		[Export]
		public string Title;

		public bool IsOpen = false;

		[Signal]
		private delegate void Opened();
		[Signal]
		private delegate void Closed();

		public virtual void OpenMenu()
		{
			FocusMode = FocusModeEnum.All;
			EmitSignal(nameof(Opened));
			IsOpen = true;
			GrabFocus();
		}

		public virtual void CloseMenu()
		{
			EmitSignal(nameof(Closed));
			IsOpen = false;
		}

	}
}
