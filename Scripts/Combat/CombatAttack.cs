

namespace BrickAndMortal.Scripts.Combat
{
    public struct CombatAttack
    {
        public enum Elements 
        {
            Phys,
            Fire,
            Ice,
            Magic,
            Ghost
        }

        public CombatAttack(int[] damage)
        {
            Damage = damage;
        }

        public int[] Damage;
    }
}
