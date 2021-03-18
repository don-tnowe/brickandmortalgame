using Godot;

namespace BrickAndMortal.Scripts.Combat
{
	public class HeroAttack : Area2D
	{
		[Export]
		public int[] Damage = new int[] { 3, 0, 0, 0, 0 };
		
		public CombatAttack Attack;

		public HeroAttack()
		{
			Attack = new CombatAttack(Damage);
		}

		public virtual void HitEnemy(EnemyBase enemy)
		{
			enemy.Hurt(this);
		}

	}
}




