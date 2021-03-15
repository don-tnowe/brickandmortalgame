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
        }

        public override void MoveBody(float delta)
        {
            base.MoveBody(delta);

            if (!Hero.IsOnFloor() && Hero.NodeTimerCoyote.TimeLeft == 0)
                Hero.NodeTimerCoyote.Start();
            if (Hero.InputMoveDirection != 0)
            {
                if (Hero.VelocityX * Hero.InputMoveDirection < HeroParameters.MaxSpeed)
                    Hero.VelocityX += HeroParameters.AccelGround * Hero.InputMoveDirection * delta;
            }
            else if (Hero.NodeTimerCoyote.TimeLeft == 0)
            {
                if (Hero.VelocityX > 0)
                {
                    Hero.VelocityX -= HeroParameters.Brake * delta;
                    if (Hero.VelocityX < 0)
                        Hero.VelocityX = 0;
                }
                else if (Hero.VelocityX < 0)
                {
                    Hero.VelocityX += HeroParameters.Brake * delta;
                    if (Hero.VelocityX > 0)
                        Hero.VelocityX = 0;
                }
                if (!Hero.NodeRayGround.IsColliding())
                    Hero.VelocityX = 0;
            }
        }

        public override void InputMove(float direction)
        {
            if (direction == 0)
                Hero.VelocityX *= HeroParameters.BrakeInstantMult;
            else
                Hero.NodeFlipH.Scale = new Vector2(Math.Sign(direction), 1);
        }

        public override void InputJump(bool pressed) 
        {
            if (pressed)
            {
                Hero.VelocityX += Hero.VelocityXSign;
                Hero.VelocityY = HeroParameters.Jump;
                Hero.SwitchState(Hero.States.Air);
            }
        }
    }
}
