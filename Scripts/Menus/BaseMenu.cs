using Godot;

namespace BrickAndMortal.Scripts.Menus
{
	abstract class BaseMenu : Control
	{
		[Export]
		public string Title;

		public bool IsOpen;

		[Signal]
		private delegate void Opened();
		[Signal]
		private delegate void Closed();

		public virtual void OpenMenu()
		{
			EmitSignal("Opened");
			IsOpen = true;
		}

		public virtual void CloseMenu()
		{
			EmitSignal("Closed");
			IsOpen = false;
		}

	}
}
