using BrickAndMortal.Scripts.HeroComponents;
using System;
using Godot;

namespace BrickAndMortal.Scripts.HeroStates
{
	abstract class HeroStateMoving : HeroStateBase
	{
		public HeroStateMoving(Hero hero) : base(hero) { }
		
		public override void MoveBody(float delta)
		{
			_hero.LastVelocity = new Vector2(_hero.VelocityX, _hero.VelocityY);
			_hero.VelocityY += delta * (
							_hero.VelocityY < 0 ?
							HeroParameters.GravityJump : HeroParameters.GravityFall
						);
			
			var NewVelocity = _hero.MoveAndSlide(new Vector2(_hero.VelocityX, _hero.VelocityY), Vector2.Up);
			_hero.VelocityY = NewVelocity.y;
			if (_hero.VelocityX != 0)
				_hero.VelocityXSign = Math.Sign(_hero.LastVelocity.x);
		}
	}
}
