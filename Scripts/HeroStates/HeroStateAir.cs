using Godot;
using BrickAndMortal.Scripts.HeroComponents;

namespace BrickAndMortal.Scripts.HeroStates
{
	class HeroStateAir : HeroStateMoving
	{
		public HeroStateAir(Hero hero) : base(hero) { }

		public override void MoveBody(float delta)
		{
			base.MoveBody(delta);

			if (_hero.IsOnFloor())
				_hero.SwitchState(Hero.States.Ground);
			if (_hero.IsOnWall())
				_hero.SwitchState(Hero.States.Wall);
			if (_hero.InputMoveDirection != 0)
				if (_hero.VelocityX * _hero.InputMoveDirection < HeroParameters.MaxSpeed)
					_hero.VelocityX += HeroParameters.AccelAir * _hero.InputMoveDirection * delta;
			if (_hero.VelocityY > 0)
			{
				if (_hero.AnimationAllowed)
					if (_hero.LastVelocity.y <= 0)
						_hero.NodeAnim.Play("Fall");
				if (_hero.VelocityY > HeroParameters.MaxFall)
					_hero.VelocityY = HeroParameters.MaxFall;
			}
		}

		public override void InputJump(bool pressed)
		{
			if (!pressed && _hero.VelocityY < HeroParameters.JumpInterrupted)
				_hero.VelocityY = HeroParameters.JumpInterrupted;
		}

		public override void InputAttack()
		{
			_hero.NodeFlipH.Scale = _hero.NodeWeapon.Scale;
			_hero.NodeAnim.Seek(0);
			_hero.NodeAnim.Play("AttackAir");
		}
	}
}
