using Godot;
using System;

namespace BrickAndMortal.Scripts.HeroStates
{
	class HeroStateHurt : HeroState
	{
		public HeroStateHurt(Hero hero) : base(hero) 
		{
			var newVelocity = -new Vector2(Hero.VelocityX, Hero.VelocityY).Normalized() * HeroParameters.HurtBounce;
			Hero.VelocityX = newVelocity.x;
			Hero.VelocityY = newVelocity.y;

			Hero.CanAttack = false;
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

        public override void AnimationAction(int action)
		{
			Hero.CanAttack = true;
			if (Hero.IsOnFloor())
				Hero.SwitchState(Hero.States.Ground);
			else if (Hero.IsOnWall())
				Hero.SwitchState(Hero.States.Wall);
			else
				Hero.SwitchState(Hero.States.Air);
		}
	}
}
