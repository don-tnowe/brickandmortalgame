using BrickAndMortal.Scripts.HeroComponents;
using Godot;
using System;

namespace BrickAndMortal.Scripts.HeroStates
{
	class HeroStateWall : HeroStateMoving
	{
		protected int _wallDirection;
		private bool _jumpBuffered = false;

		public HeroStateWall(Hero hero) : base(hero)
		{
			_wallDirection = _hero.VelocityXSign;
			_hero.NodeFlipH.Scale = new Vector2(_wallDirection, 1);
			_hero.NodeRayLedgeGrab.Enabled = true;
			_hero.NodeRayLedgeGrabHigher.Enabled = true;

			if (_hero.VelocityY > 0)
				_hero.VelocityY *= HeroParameters.WallFrictionInstantMult;
			if (_wallDirection == -Math.Sign(_hero.InputMoveDirection))
				_hero.CallDeferred("InputMove", -_wallDirection);
			if (
					OS.GetTicksMsec() - HeroParameters.MsecJumpBuffer < _hero.InputJumpStart
					&& _hero.VelocityY >= HeroParameters.JumpInterrupted
				)
				_jumpBuffered = true;
			_hero.NodeAnim.Play("Wall");
		}


		public override void MoveBody(float delta)
		{
			base.MoveBody(delta);
			
			if (_hero.IsOnFloor())
			{
				_hero.SwitchState(Hero.States.Ground);
				return;
			}
			if (_hero.NodeTimerCoyote.TimeLeft == 0)
			{
				if (_hero.VelocityY > HeroParameters.MaxFallWall)
					_hero.VelocityY = HeroParameters.MaxFallWall;
				if (!_hero.IsOnWall())
				{
					_hero.MoveAndSlide(new Vector2(-_hero.VelocityX, _hero.VelocityY), Vector2.Up);
					_hero.VelocityX = _wallDirection;
					_hero.SwitchState(Hero.States.Air);
					if (_hero.VelocityY < 0)
						_hero.NodeAnim.Play("Jump");
					else
						_hero.NodeAnim.Play("Fall");
					return;
				}
				CheckGrabRaycast();
			}
			if (_jumpBuffered)
			{
				_hero.CallDeferred("InputJump", true);
				return;
			}
		}

		protected virtual void CheckGrabRaycast()
		{
			if (!_hero.NodeRayLedgeGrab.IsColliding())
			{
				_hero.VelocityXSign = _wallDirection;
				_hero.SwitchState(Hero.States.LedgeGrab);
			}
		}

		public override void ExitState()
		{
			_hero.NodeRayLedgeGrab.Enabled = false;
			_hero.NodeRayLedgeGrabV.Enabled = false;
			_hero.NodeRayLedgeGrabHigher.Enabled = false;
		}

		public override void InputMove(float direction)
		{
			if (_wallDirection == -Math.Sign(direction))
			{
				_hero.VelocityX = -HeroParameters.JumpWallHorizontalWeak * _wallDirection;
				_hero.NodeFlipH.Scale = new Vector2(-_wallDirection, 1);
				_hero.NodeTimerCoyote.Start();
				if (_hero.VelocityY < 0)
					_hero.NodeAnim.Play("Jump");
				else
					_hero.NodeAnim.Play("Fall");
			}
		}

		public override void InputJump(bool pressed)
		{
			_jumpBuffered = false;
			if (pressed)
				if (_hero.InputMoveDirection != _wallDirection || _hero.NodeRayLedgeGrabHigher.IsColliding())
				{
					_hero.NodeFlipH.Scale = new Vector2(-_wallDirection, 1);
					_hero.NodeTimerCoyote.Stop();
					_hero.VelocityX = -HeroParameters.JumpWallHorizontal * _wallDirection;
					_hero.VelocityY = HeroParameters.JumpWall;
					_hero.SwitchState(Hero.States.Air);
					_hero.NodeAnim.Play("Jump");
				}
				else
				{
					_hero.VelocityY = HeroParameters.JumpWall;
					_hero.NodeAnim.Play("LedgeGrabPre");
				}
			else if (_hero.VelocityY < HeroParameters.JumpInterrupted)
				_hero.VelocityY = HeroParameters.JumpInterrupted;
		}
		public override void InputAttack()
		{
			_hero.NodeWeapon.Scale = new Vector2(-_hero.NodeFlipH.Scale.x, 1);
			_hero.NodeAnim.Seek(0);
			_hero.NodeAnim.Play("AttackWall");
		}
	}
}
