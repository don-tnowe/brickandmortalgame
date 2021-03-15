using Godot;
using System;

namespace BrickAndMortal.Scripts.HeroStates
{
	class HeroStateLedgeGrab : HeroStateWall
	{
		private int _wallDirection;
		private bool _jumpBuffered = false;
		private bool _grabbed = false;

		public HeroStateLedgeGrab(Hero hero) : base(hero)
		{
			_wallDirection = Hero.VelocityXSign;
			Hero.NodeRayLedgeGrabV.Enabled = true;
		}

		public override void MoveBody(float delta)
		{
			if (!_grabbed)
				base.MoveBody(delta);
		}

		protected override void CheckGrabRaycast()
		{
			if (Hero.NodeRayLedgeGrab.IsColliding())
			{
				Hero.GlobalPosition = new Vector2(Hero.Position.x, Hero.NodeRayLedgeGrabV.GetCollisionPoint().y + 9);
				_grabbed = true;
			}
		}

		public override void InputMove(float direction)
		{
			if (_wallDirection == -Math.Sign(direction))
            {
				Hero.VelocityX = -HeroParameters.JumpWallHorizontal * _wallDirection;
				Hero.NodeFlipH.Scale = new Vector2(-_wallDirection, 1);
				Hero.NodeTimerCoyote.Start();
				Hero.SwitchState(Hero.States.Wall);
			}
		}

		public override void InputJump(bool pressed)
		{
			_jumpBuffered = false;
			if (pressed)
				if (Hero.InputMoveDirection == -_wallDirection)
					base.InputJump(true);
				else
				{
					Hero.VelocityY = HeroParameters.JumpWall;
					Hero.SwitchState(Hero.States.Air);
				}
			else if (Hero.VelocityY < HeroParameters.JumpInterrupted)
				Hero.VelocityY = HeroParameters.JumpInterrupted;
		}

	}
}
