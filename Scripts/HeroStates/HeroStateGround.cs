using BrickAndMortal.Scripts.HeroComponents;
using System;
using Godot;


namespace BrickAndMortal.Scripts.HeroStates
{
	class HeroStateGround : HeroStateMoving
	{
		public HeroStateGround(Hero hero) : base(hero)
		{
			_hero.NodeTimerCoyote.Stop();
			
			if (_hero.InputMoveDirection != 0 && (_hero.VelocityX < 0) != (_hero.InputMoveDirection < 0))
				InputMove(0);
				
			InputMove(_hero.InputMoveDirection);
				
			if (OS.GetTicksMsec() - HeroParameters.MsecJumpBuffer < _hero.InputJumpStart)
				_hero.CallDeferred("InputJump", true);
				
			_hero.NodeAnim.Play("Land");
		}

		public override void MoveBody(float delta)
		{
			base.MoveBody(delta);
			_hero.Position = new Vector2(_hero.Position.x, (float)Math.Ceiling(_hero.Position.y));

			if (!_hero.IsOnFloor() && _hero.NodeTimerCoyote.TimeLeft == 0)
			{
				_hero.NodeTimerCoyote.Start();
				_hero.InstaTurnStart = OS.GetTicksMsec();
				_hero.NodeAnim.Play("Fall");
			}
			if (_hero.InputMoveDirection != 0 && _hero.VelocityX * _hero.InputMoveDirection < HeroParameters.MaxSpeed)
			{
				_hero.VelocityX += HeroParameters.AccelGround * _hero.InputMoveDirection * delta;
			}
			else if (_hero.NodeTimerCoyote.TimeLeft == 0)
			{
				if (_hero.VelocityX > 0)
				{
					_hero.VelocityX -= HeroParameters.Brake * delta;
					if (_hero.VelocityX < 0)
						BrakeStop();
				}
				else if (_hero.VelocityX < 0)
				{
					_hero.VelocityX += HeroParameters.Brake * delta;
					if (_hero.VelocityX > 0)
						BrakeStop();
				}
				if (_hero.InputMoveDirection == 0 && !_hero.NodeRayGround.IsColliding())
					BrakeStop();
			}
		}

		private void BrakeStop()
		{
			_hero.VelocityX = 0;
			if (_hero.AnimationAllowed)
				_hero.NodeAnim.Play("Idle");
		}

		public override void InputMove(float direction)
		{
			var directionSign = Math.Sign(direction);
			if (direction != 0)
			{
				_hero.NodeFlipH.Scale = new Vector2(directionSign, 1);
				if (_hero.InputMoveDirection == 0 && _hero.NodeAnim.CurrentAnimation != "Land")
					if (_hero.VelocityXSign != directionSign)
					{
						if (_hero.InstaTurnStart != 0 && _hero.InstaTurnStart > OS.GetTicksMsec() - HeroParameters.MsecInstaTurn)
							_hero.VelocityX = directionSign * HeroParameters.InstaTurnSpeed;
						_hero.InstaTurnStart = 0;
						if (_hero.AnimationAllowed)
							_hero.NodeAnim.Play("RunTurn");
					}
					else if (_hero.AnimationAllowed)
						_hero.NodeAnim.Play("RunStart");
			}
			else
			{
				_hero.VelocityX *= HeroParameters.BrakeInstantMult;
				if (_hero.AnimationAllowed)
					_hero.NodeAnim.Play("RunStop");
			}
		}

		public override void InputJump(bool pressed) 
		{
			if (pressed)
			{
				_hero.VelocityX += _hero.VelocityXSign;
				_hero.VelocityY = HeroParameters.Jump;
				_hero.SwitchState(Hero.States.Air);

				_hero.AnimationAllowed = true;
				_hero.NodeAnim.Play("Jump");
			}
		}

		public override void InputAttack()
		{
			_hero.NodeFlipH.Scale = _hero.NodeWeapon.Scale;
			_hero.NodeAnim.Seek(0);
			_hero.NodeAnim.Play("AttackGround");
		}
		
		public override void AnimationAction(int action)
		{
			if (action == 0)
				if (_hero.InputMoveDirection != 0)
					_hero.NodeAnim.Play("RunStart");
				else if (_hero.VelocityX != 0)
					_hero.NodeAnim.Play("RunStop");
				else
					_hero.NodeAnim.Play("Idle");
		}
	}
}
