using Godot;
using System.Collections;


namespace BrickAndMortal.Scripts.Combat
{
	public class CombatActor : Area2D
	{
		[Export]
		public int HealthMax = 20;
		[Export]
		public int Health = 20;
		[Export]
		public float[] Defense = new float[CombatAttack.ElementCount];

		[Signal]
		public delegate void Defeated();
		[Signal]
		public delegate void HealthSetMax(int value);
		[Signal]
		public delegate void HealthSet(int value);

		public virtual void Hurt(CombatAttack byAttack)
		{
			var damage = 0;
			for (int i = 0; i < byAttack.Damage.Length; ++i)
				if (byAttack.Damage[i] != 0 && Defense[i] != 0)
					damage += (int)(byAttack.Damage[i] / Defense[i]);
			
			Health -= damage;
			EmitSignal("HealthSet", Health);

			if (Health <= 0)
				EmitSignal("Defeated");
		}

		public void UpdateMaxHp()
		{
			EmitSignal("HealthSetMax", HealthMax);
			EmitSignal("HealthSet", Health);
		}
	}
}



