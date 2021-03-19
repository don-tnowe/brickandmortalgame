using Godot;
using System.Collections;


namespace BrickAndMortal.Scripts.Combat
{
    class CombatActor : Reference
    {
        public int HealthMax;
        public int Health;
        public float[] Attack;
        public float[] Defense;

        [Signal]
        public delegate void Defeated();
        [Signal]
        public delegate void HealthSetMax(int value);
        [Signal]
        public delegate void HealthSet(int value);

        public CombatActor(int health, float[] attack, float[] defense)
        {
            Health = health;
            HealthMax = health;
            Attack = attack;
            Defense = defense;
        }

        public void Hurt(CombatAttack byAttack)
        {
            var damage = 0;
            for (int i = 0; i < byAttack.Damage.Length; ++i)
                if (byAttack.Damage[i] != 0)
                    damage += (int)(byAttack.Damage[i] / Defense[i]);

            Health -= damage;
            EmitSignal("HealthSet", Health);

            if (Health <= 0)
                EmitSignal("Defeated");
        }

        public void UpdateMaxHp()
        {
            CallDeferred("emit_signal","HealthSetMax", HealthMax);
        }
    }
}
