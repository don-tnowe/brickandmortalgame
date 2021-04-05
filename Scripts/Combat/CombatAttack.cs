using System.Collections.Generic;
using Godot;

namespace BrickAndMortal.Scripts.Combat
{
	public enum Elements 
	{
		Phys,
		Fire,
		Poison,
		Iron,
		Magic,
		Ghost,
		Ice,
		Concussion
	}

	public class CombatAttack : Area2D
	{
		[Export]
		public Dictionary<Elements, float> Damage;
		[Export]
		public NodePath Attacker;

		public virtual void HitTarget(CombatActor target) { }
	}
}


