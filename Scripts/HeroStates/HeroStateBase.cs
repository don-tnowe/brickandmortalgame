
namespace BrickAndMortal.Scripts.HeroStates
{
	public class HeroStateBase
	{
		protected BrickAndMortal.Scripts.HeroComponents.Hero _hero;

		public HeroStateBase(BrickAndMortal.Scripts.HeroComponents.Hero hero)	
		{
			_hero = hero;
		}

		public virtual void EnterState() { }

		public virtual void MoveBody(float delta) { }

		public virtual void ExitState() { }

		public virtual void InputMove(float direction) { }

		public virtual void InputJump(bool pressed) { }

		public virtual void InputAttack() { }

		public virtual void AnimationAction(int action) { }
		
	}
}
