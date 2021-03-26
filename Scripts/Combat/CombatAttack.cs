using Godot;

namespace BrickAndMortal.Scripts.Combat
{
	public class CombatAttack : Area2D
	{
		public enum Elements 
		{
			Phys,
			Fire,
			Ice,
			Magic,
			Ghost
		}
		public const int ElementCount = 5;
		
		[Export]
		public int[] Damage = new int[ElementCount];
		[Export]
		public NodePath Attacker;
	}
}
