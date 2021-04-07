using BrickAndMortal.Scripts.HeroComponents;
using Godot;
using System;

namespace BrickAndMortal.Scripts.HeroStates
{
	class HeroStateHurt : HeroState
	{
		public HeroStateHurt(Hero hero) : base(hero) 
		{
			Hero.NodeTimerAttack.Stop();
			Hero.NodeFlipH.Scale = new Vector2(-Math.Sign(Hero.VelocityX), 1);
			Hero.NodeAnim.Play("Hurt");
		}

		public override void MoveBody(float delta)
		{
			base.MoveBody(delta);
			if (Hero.IsOnFloor())
				if (Hero.VelocityX > 0)
				{
					Hero.VelocityX -= HeroParameters.Brake * delta;
					if (Hero.VelocityX < 0)
						Hero.VelocityX = 0;
				}
				else if (Hero.VelocityX < 0)
				{
					Hero.VelocityX += HeroParameters.Brake * delta;
					if (Hero.VelocityX > 0)
						Hero.VelocityX = 0;
				}
		}
	}
}
