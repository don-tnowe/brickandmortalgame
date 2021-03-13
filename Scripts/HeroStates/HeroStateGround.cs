using System;
using Godot;


namespace BrickAndMortal.Scripts.HeroStates
{
	class HeroStateGround : HeroState
	{
		public HeroStateGround(Hero hero) : base(hero)
		{
			HeroRef.NodeTimerCoyote.Stop();
			InputMove(HeroRef.InputMoveDirection);
			if (OS.GetTicksMsec() - HeroParameters.MsecJumpBuffer < HeroRef.InputJumpStart)
				HeroRef.CallDeferred("InputJump", true);
		}

		public override void MoveBody(float delta)
		{
			base.MoveBody(delta);

			if (!HeroRef.IsOnFloor() && HeroRef.NodeTimerCoyote.TimeLeft == 0)
				HeroRef.NodeTimerCoyote.Start();
			if (HeroRef.InputMoveDirection != 0)
			{
				if (HeroRef.VelocityX * HeroRef.InputMoveDirection < HeroParameters.MaxSpeed)
					HeroRef.VelocityX += HeroParameters.AccelGround * HeroRef.InputMoveDirection * delta;
			}
			else
			{
				if (HeroRef.VelocityX > 0)
				{
					HeroRef.VelocityX -= HeroParameters.Brake * delta;
					if (HeroRef.VelocityX < 0)
						HeroRef.VelocityX = 0;
				}
				else if (HeroRef.VelocityX < 0)
				{
					HeroRef.VelocityX += HeroParameters.Brake * delta;
					if (HeroRef.VelocityX > 0)
						HeroRef.VelocityX = 0;
				}
				if (!HeroRef.NodeRayGround.IsColliding())
					HeroRef.VelocityX = 0;
			}
		}

		public override void InputMove(float direction)
		{
			if (direction == 0)
				HeroRef.VelocityX *= HeroParameters.BrakeInstantMult;
			else
				HeroRef.NodeFlipH.Scale = new Vector2(Math.Sign(direction), 1);
		}

		public override void InputJump(bool pressed) 
		{
			if (pressed)
			{
				HeroRef.VelocityX += HeroRef.VelocityXSign;
				HeroRef.VelocityY = HeroParameters.Jump;
				HeroRef.SwitchState(Hero.States.Air);
			}
		}
	}
}
