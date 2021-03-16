using Godot;
using System;

namespace BrickAndMortal.Scripts.HeroStates
{
	class HeroStateWall : HeroState
	{
		private int _wallDirection;
		private bool _jumpBuffered = false;

		public HeroStateWall(Hero hero) : base(hero)
		{
			_wallDirection = Hero.VelocityXSign;
			Hero.NodeFlipH.Scale = new Vector2(_wallDirection, 1);
			Hero.NodeRayLedgeGrab.Enabled = true;

			if (Hero.VelocityY > 0)
				Hero.VelocityY *= HeroParameters.WallFrictionInstantMult;
			if (_wallDirection == -Math.Sign(Hero.InputMoveDirection))
				InputMove(-_wallDirection);
			if (
					OS.GetTicksMsec() - HeroParameters.MsecJumpBuffer < Hero.InputJumpStart
					&& Hero.VelocityY >= HeroParameters.JumpInterrupted
                )
				_jumpBuffered = true;
			Hero.NodeAnim.Play("Wall");
		}


		public override void MoveBody(float delta)
		{
			base.MoveBody(delta);
			
			if (Hero.IsOnFloor())
			{
				Hero.SwitchState(Hero.States.Ground);
				return;
			}
			if (Hero.NodeTimerCoyote.TimeLeft == 0)
			{
				if (Hero.VelocityY > HeroParameters.MaxFallWall)
                                    Hero.VelocityY = HeroParameters.MaxFallWall;
				if (!Hero.IsOnWall())
				{
					Hero.MoveAndSlide(new Vector2(-Hero.VelocityX, Hero.VelocityY), Vector2.Up);
					Hero.VelocityX = _wallDirection;
					Hero.SwitchState(Hero.States.Air);
					if (Hero.VelocityY < 0)
						Hero.NodeAnim.Play("Jump");
					else
						Hero.NodeAnim.Play("Fall");
					return;
				}
				CheckGrabRaycast();
			}
			if (_jumpBuffered)
			{
				Hero.CallDeferred("InputJump", true);
				return;
			}
		}

		protected virtual void CheckGrabRaycast()
		{
			if (!Hero.NodeRayLedgeGrab.IsColliding())
			{
				Hero.VelocityXSign = _wallDirection;
				Hero.SwitchState(Hero.States.LedgeGrab);
			}
		}

		public override void ExitState()
		{
			Hero.NodeRayLedgeGrab.Enabled = false;
			Hero.NodeRayLedgeGrabV.Enabled = false;
		}

		public override void InputMove(float direction)
		{
			if (_wallDirection == -Math.Sign(direction))
			{
				Hero.VelocityX = -HeroParameters.JumpWallHorizontal * _wallDirection;
				Hero.NodeFlipH.Scale = new Vector2(-_wallDirection, 1);
				Hero.NodeTimerCoyote.Start();
				Hero.NodeAnim.Play("Fall");
			}
		}

		public override void InputJump(bool pressed)
		{
			_jumpBuffered = false;
			if (pressed)
			{
				Hero.NodeFlipH.Scale = new Vector2(-_wallDirection, 1);
				Hero.NodeTimerCoyote.Stop();
				Hero.VelocityX = -HeroParameters.JumpWallHorizontal * _wallDirection;
				Hero.VelocityY = HeroParameters.JumpWall;
				Hero.SwitchState(Hero.States.Air);
				Hero.NodeAnim.Play("Jump");
			}
			else if (Hero.VelocityY < HeroParameters.JumpInterrupted)
				Hero.VelocityY = HeroParameters.JumpInterrupted;
		}
	}
}
