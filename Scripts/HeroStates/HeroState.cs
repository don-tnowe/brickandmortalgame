using System;
using Godot;

namespace BrickAndMortal.Scripts.HeroStates
{
	public abstract class HeroState
	{
		public BrickAndMortal.Scripts.HeroComponents.Hero Hero;

		public HeroState(BrickAndMortal.Scripts.HeroComponents.Hero hero)
		{
			Hero = hero;
		}

		public virtual void MoveBody(float delta)
		{
			Hero.LastVelocity = new Vector2(Hero.VelocityX, Hero.VelocityY);
			Hero.VelocityY += delta * (
							Hero.VelocityY < 0 ?
							HeroParameters.GravityJump : HeroParameters.GravityFall
						);
			
			var NewVelocity = Hero.MoveAndSlide(new Vector2(Hero.VelocityX, Hero.VelocityY), Vector2.Up);
			Hero.VelocityY = NewVelocity.y;
			if (Hero.VelocityX != 0)
				Hero.VelocityXSign = Math.Sign(Hero.LastVelocity.x);
		}

		public virtual void ExitState() { }

		public virtual void InputMove(float direction) { }

		public virtual void InputJump(bool pressed) { }

		public virtual void InputAttack() { }

		public virtual void AnimationAction(int action) { }
		
	}
}
