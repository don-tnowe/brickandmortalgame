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
			Hero.NodeAnim.Play("LedgeGrabPre");
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
				Hero.NodeAnim.Play("LedgeGrab");
			}
		}

		public override void InputMove(float direction)
		{
			if (_wallDirection == -Math.Sign(direction))
			{
				_grabbed = false;
				base.InputMove(direction);
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
					_grabbed = false;
					Hero.NodeAnim.Play("LedgeGrabPre");
				}
			else if (Hero.VelocityY < HeroParameters.JumpInterrupted)
				Hero.VelocityY = HeroParameters.JumpInterrupted;
		}

        public override void InputAttack()
        {
			Hero.NodeAnim.Seek(0);
			if (_grabbed)
			{
				Hero.NodeWeapon.Scale = new Vector2(-Hero.NodeFlipH.Scale.x, 1);
				Hero.NodeAnim.Play("AttackLedge");
			}
			else 
				Hero.NodeAnim.Play("AttackAir");
		}
    }
}
