using Godot;

namespace BrickAndMortal.Scripts.Combat
{
	public class CombatAttack : Area2D
	{
		[Export]
		public int[] Damage = new int[ElementCount];
		public CombatActor Attacker;

		public enum Elements 
		{
			Phys,
			Fire,
			Ice,
			Magic,
			Ghost
		}

		public const int ElementCount = 5;
	}
}
