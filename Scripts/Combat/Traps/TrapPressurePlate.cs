using Godot;
using System;

namespace BrickAndMortal.Scripts.Combat.Traps
{
	public class TrapPressurePlate : Area2D
	{
		private void Trigger(KinematicBody2D body)
		{
			GetNode<AnimationPlayer>("Anim").Play("Trigger");
		}
	}
}





