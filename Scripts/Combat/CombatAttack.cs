

namespace BrickAndMortal.Scripts.Combat
{
    public class CombatAttack
    {
        public enum Elements 
        {
            Phys,
            Fire,
            Ice,
            Magic,
            Ghost
        }

        public CombatAttack() { }

        public int[] Damage = new int[5] { 4, 0, 0, 0, 0 };
    }
}
