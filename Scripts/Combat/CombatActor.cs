using System.Collections.Generic;
using System;
using Godot;

namespace BrickAndMortal.Scripts.Combat
{
	public class CombatActor : Area2D
	{
		[Export]
		public Dictionary<Elements, float> Defense;
		[Export]
		private int _health = 20;

		[Signal]
		private delegate void Hit(CombatAttack byAttack);
		[Signal]
		private delegate void HealthSet(int value);
		[Signal]
		private delegate void HealthSetMax(int value);
		[Signal]
		private delegate void Defeated();

		public bool IsPlayer = false;
		public bool Invincible = false;

		public int HealthMax
		{
			set
			{
				_healthMax = value;
				EmitSignal(nameof(HealthSetMax), _healthMax);
				EmitSignal(nameof(HealthSet), _health);
			}
			get
			{
				return _healthMax;
			}
		}

		public int Health
		{
			set
			{
				_health = value;
				EmitSignal(nameof(HealthSet), _health);
				if (_health <= 0)
					EmitSignal(nameof(Defeated));
			}
			get
			{
				return _health;
			}
		}

		private int _healthMax;

		private PackedScene _sceneDamageNum = ResourceLoader.Load<PackedScene>("res://Scenes/DungeonFeatures/DamageNum.tscn");
		private Random _random = new Random();

		public virtual void Hurt(CombatAttack byAttack)
		{
			if (!byAttack.AttackerBackfire && byAttack.GetAttacker() == (CombatActor)this)
				return;
			
			EmitSignal(nameof(Hit), byAttack);
			byAttack.HitTarget(this);
			var damage = 0;
			var multi = 1f;
			var effectivenessLevel = 0;
			if (!Invincible)
			{
				foreach (Elements i in byAttack.Damage.Keys)
					if (!Defense.ContainsKey(i))
					{
						damage += (int)byAttack.Damage[i];
					}
					else if (byAttack.Damage[i] != 0 && Defense[i] != 0)
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

			var nodeDamageNum = (Node2D)_sceneDamageNum.Instance();
			GetParent().GetParent().AddChild(nodeDamageNum);
			nodeDamageNum.GetNode<Label>("Node/Label").Text = damage.ToString();
			nodeDamageNum.GetNode<AnimationPlayer>("Anim").Play("Eff" + effectivenessLevel);
			nodeDamageNum.GlobalPosition = GlobalPosition + new Vector2(((float)_random.NextDouble() - 0.5f) * 16, 0);
		}
	}
}



