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
		[Export]
		public bool AttackerBackfire = false;
		
		public bool HasAttacker() 
		{
			return Attacker != null && HasNode(Attacker) && GetNode(Attacker) is CombatActor;
		}
		
		public CombatActor GetAttacker() 
		{
			if (!HasAttacker())
				return null; 
				
			else 
				return GetNode<CombatActor>(Attacker);
		}

		public virtual void HitTarget(CombatActor target) { }

		public virtual void Launch(Vector2 dir) { }
		
		public virtual void Launch(float angle) { }
	}
}


