using Godot;
using System;


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

		public bool IsPlayer = false;
		public bool Invincible = false;

		private PackedScene _sceneDamageNum = ResourceLoader.Load<PackedScene>("res://Scenes/DungeonFeatures/DamageNum.tscn");
		private Random random = new Random();

		public virtual void Hurt(CombatAttack byAttack)
		{
			var damage = 0;
			var multi = 1f;
			var effectivenessLevel = 0;
			if (!Invincible)
			{
				for (int i = 0; i < byAttack.Damage.Length; ++i)
					if (byAttack.Damage[i] != 0 && Defense[i] != 0)
					{
						damage += (int)(byAttack.Damage[i] / Defense[i]);
						multi /= Defense[i];
					}
			}
			else
			{
				multi = 0;
				damage = 0;
			}

			Health -= damage;
			EmitSignal("HealthSet", Health);

			if (Health <= 0)
				EmitSignal("Defeated");

			if (IsPlayer)
				if (damage <= 0)
					effectivenessLevel = -2;
				else 
					effectivenessLevel = -1;
			else if (multi < 1)
				effectivenessLevel = 0;
			else if (multi < 2)
				effectivenessLevel = 1;
			else if (multi < 4)
				effectivenessLevel = 2;
			else 
				effectivenessLevel = 3;

			var nodeDamageNum = (DamageNumbers)_sceneDamageNum.Instance();
			nodeDamageNum.DisplayNumber(damage, effectivenessLevel);
			GetParent().GetParent().AddChild(nodeDamageNum);
			nodeDamageNum.GlobalPosition = GlobalPosition + new Vector2(((float)random.NextDouble() - 0.5f) * 16, 0);
		}

		public void UpdateMaxHp()
		{
			EmitSignal("HealthSetMax", HealthMax);
			EmitSignal("HealthSet", Health);
		}
	}
}



