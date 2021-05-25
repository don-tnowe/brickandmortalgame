using BrickAndMortal.Scripts.HeroComponents;
using Godot;
using System;

namespace BrickAndMortal.Scripts.HeroStates
{
	class HeroStateLedgeGrab : HeroStateWall
	{
		private bool _grabbed;

		public HeroStateLedgeGrab(Hero hero) : base(hero) { }
		
		public override void EnterState() 
		{
			base.EnterState();
			_grabbed = false;
			_hero.NodeRayLedgeGrabV.Enabled = true;
			_hero.NodeAnim.Play("LedgeGrabPre");
		}
		
		public override void MoveBody(float delta)
		{
			if (!_grabbed)
				base.MoveBody(delta);
		}

		protected override void CheckGrabRaycast()
		{
			if (_hero.NodeRayLedgeGrab.IsColliding())
			{
				_hero.GlobalPosition = new Vector2(_hero.Position.x, _hero.NodeRayLedgeGrabV.GetCollisionPoint().y + 9);
				_grabbed = true;
				_hero.NodeAnim.Play("LedgeGrab");
			}
		}

		public override void InputMove(float direction)
		{
			if (_wallDirection == -Math.Sign(direction))
			{
				_grabbed = false;
				
				if (_hero.VelocityY < 0)
					base.InputJump(true);
					
				else
					base.InputMove(direction);
			}
		}

		public override void InputJump(bool pressed)
		{
			if (pressed)
				if (_hero.InputMoveDirection == -_wallDirection)
					base.InputJump(true);
				else
				{
					_hero.VelocityY = HeroParameters.JumpWall;
					_grabbed = false;
					_hero.NodeAnim.Play("LedgeGrabPre");
				}
			else if (_hero.VelocityY < HeroParameters.JumpInterrupted)
				_hero.VelocityY = HeroParameters.JumpInterrupted;
		}

		public override void InputAttack()
		{
			_hero.NodeAnim.Seek(0);
			if (_grabbed)
			{
				_hero.NodeWeapon.Scale = new Vector2(-_hero.NodeFlipH.Scale.x, 1);
				_hero.NodeAnim.Play("AttackLedge");
			}
			else 
				_hero.NodeAnim.Play("AttackAir");
		}
	}
}
