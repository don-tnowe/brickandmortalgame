using System;
using Godot;


namespace BrickAndMortal.Scripts.HeroStates
{
    class HeroStateGround : HeroState
    {
        public HeroStateGround(Hero hero) : base(hero)
        {
            Hero.NodeTimerCoyote.Stop();
            InputMove(Hero.InputMoveDirection);
            if (OS.GetTicksMsec() - HeroParameters.MsecJumpBuffer < Hero.InputJumpStart)
                Hero.CallDeferred("InputJump", true);
            Hero.NodeAnim.Play("Land");
        }

        public override void MoveBody(float delta)
        {
            base.MoveBody(delta);
            Hero.Position = new Vector2(Hero.Position.x, (float)Math.Ceiling(Hero.Position.y));

            if (!Hero.IsOnFloor() && Hero.NodeTimerCoyote.TimeLeft == 0)
            {
                Hero.NodeTimerCoyote.Start();
                Hero.NodeAnim.Play("Fall");
            }
            if (Hero.InputMoveDirection != 0 && Hero.VelocityX * Hero.InputMoveDirection < HeroParameters.MaxSpeed)
            {
                Hero.VelocityX += HeroParameters.AccelGround * Hero.InputMoveDirection * delta;
            }
            else if (Hero.NodeTimerCoyote.TimeLeft == 0)
            {
                if (Hero.VelocityX > 0)
                {
                    Hero.VelocityX -= HeroParameters.Brake * delta;
                    if (Hero.VelocityX < 0)
                        BrakeStop();
                }
                else if (Hero.VelocityX < 0)
                {
                    Hero.VelocityX += HeroParameters.Brake * delta;
                    if (Hero.VelocityX > 0)
                        BrakeStop();
                }
                if (!Hero.NodeRayGround.IsColliding())
                    BrakeStop();
            }
        }

        private void BrakeStop()
        {
            Hero.VelocityX = 0;
            Hero.NodeAnim.Play("Idle");
        }

        public override void InputMove(float direction)
        {
            if (direction != 0)
            {
                Hero.NodeFlipH.Scale = new Vector2(Math.Sign(direction), 1);
                if (Hero.InputMoveDirection == 0 && Hero.NodeAnim.CurrentAnimation != "Land")
                    Hero.NodeAnim.Play("RunStart");
            }
            else
            {
                Hero.VelocityX *= HeroParameters.BrakeInstantMult;
                if (Hero.NodeAnim.CurrentAnimation != "Land")
                    Hero.NodeAnim.Play("RunStop");
            }
        }

        public override void InputJump(bool pressed) 
        {
            if (pressed)
            {
                Hero.VelocityX += Hero.VelocityXSign;
                Hero.VelocityY = HeroParameters.Jump;
                Hero.SwitchState(Hero.States.Air);
                Hero.NodeAnim.Play("Jump");
            }
        }

        public override void InputAttack()
        {
            Hero.NodeFlipH.Scale = Hero.NodeWeapon.Scale;
        }
    }
}
