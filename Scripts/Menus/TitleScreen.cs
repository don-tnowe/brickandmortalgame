using Godot;
using System;

namespace BrickAndMortal.Scripts
{
	public class TitleScreen : Node
	{
		public override void _Input(InputEvent @event)
		{
			if (@event is InputEventMouseMotion)
				return;
				
			if (@event is InputEventJoypadMotion)
				return;
			
			GetNode<AnimationPlayer>("Anim").Play("Go");
		}
		
		private void Go()
		{
			SaveData.LoadGame();
			
			if (SaveData.Screen == 0) // Quit the game after town
				GetTree().ChangeScene("res://Scenes/Screens/Dungeon.tscn");
			
		//	else if (SaveData.Screen == 1) Not adding dungeon saves now. TODO
		//		GetTree().ChangeScene("res://Scenes/Screens/Dungeon.tscn");
			
			else  // Quit the game in the shop or in town
				GetTree().ChangeScene("res://Scenes/Screens/Town.tscn");
		}
	}
}

