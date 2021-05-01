using Godot;

namespace BrickAndMortal.Scripts.Menus
{
	class MenuSettings : BaseMenu
	{
		private void _on_Button_pressed()
		{
			GetTree().Paused = false;
			GetTree().ChangeScene("res://Scenes/Screens/Town.tscn");
		}
	}
}


