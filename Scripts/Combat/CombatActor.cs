using Godot;
using System.Collections;


namespace BrickAndMortal.Scripts.Combat
{
    class CombatActor : Reference
    {
        public int HitpointsMax;
        public int Hitpoints;
        public float[] Attack;
        public float[] Defense;

        [Signal]
        public delegate void Defeated();

        public CombatActor(int hitpoints, float[] attack, float[] defense)
        {
            Hitpoints = hitpoints;
            HitpointsMax = hitpoints;
            Attack = attack;
            Defense = defense;
        }

        public void Hurt(CombatAttack byAttack)
        {
            for (int i = 0; i < byAttack.Damage.Length; ++i)
                if (byAttack.Damage[i] != 0)
                    Hitpoints -= (int)(byAttack.Damage[i] / Defense[i]);
            if (Hitpoints <= 0)
                EmitSignal("Defeated");
        }
    }
}
