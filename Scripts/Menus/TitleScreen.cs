using Godot;
using System;

namespace BrickAndMortal.Scripts
{
	public class TitleScreen : Node
	{
		public override void _Input(InputEvent @event)
		{
			if (@event is InputEventMouse)
				return;
				
			if (@event is InputEventJoypadMotion)
				return;
			
			GetNode<AnimationPlayer>("Anim").Play("Go");
		}
		
		private void Go()
		{
			SaveData.LoadGame();
			
			if (SaveData.Screen == 0) 
				GetTree().ChangeScene("res://Scenes/Screens/Dungeon.tscn");
			
		//	else if (SaveData.Screen == 1) Not adding dungeon saves now. TODO
		//		GetTree().ChangeScene("res://Scenes/Screens/Dungeon.tscn");
			
			else  
				GetTree().ChangeScene("res://Scenes/Screens/Town.tscn");
		}
	}
}

