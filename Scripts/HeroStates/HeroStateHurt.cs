using BrickAndMortal.Scripts.HeroComponents;
using Godot;
using System;

namespace BrickAndMortal.Scripts.HeroStates
{
	class HeroStateHurt : HeroStateMoving
	{
		public HeroStateHurt(Hero hero) : base(hero) { }
		
		public override void EnterState() 
		{
			_hero.NodeTimerAttack.Stop();
			_hero.NodeFlipH.Scale = new Vector2(-Math.Sign(_hero.VelocityX), 1);
			_hero.NodeAnim.Play("Hurt");
		}

		public override void MoveBody(float delta)
		{
			base.MoveBody(delta);
			if (_hero.IsOnFloor())
				if (_hero.VelocityX > 0)
				{
					_hero.VelocityX -= HeroParameters.Brake * delta;
					if (_hero.VelocityX < 0)
						_hero.VelocityX = 0;
				}
				else if (_hero.VelocityX < 0)
				{
					_hero.VelocityX += HeroParameters.Brake * delta;
					if (_hero.VelocityX > 0)
						_hero.VelocityX = 0;
				}
		}
	}
}
