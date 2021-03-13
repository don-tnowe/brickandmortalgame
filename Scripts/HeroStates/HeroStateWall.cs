using Godot;
using System;

namespace BrickAndMortal.Scripts.HeroStates
{
    class HeroStateWall : HeroState
    {
        private int _wallDirection;

        public HeroStateWall(Hero hero) : base(hero)
        {
            _wallDirection = HeroRef.VelocityXSign;
            HeroRef.NodeFlipH.Scale = new Vector2(_wallDirection, 1);

            if (HeroRef.VelocityY > 0)
                HeroRef.VelocityY *= HeroParameters.WallFrictionInstantMult;

            if (_wallDirection == -Math.Sign(HeroRef.InputMoveDirection))
                InputMove(-_wallDirection);
            else if (
                    OS.GetTicksMsec() - HeroParameters.MsecJumpBuffer < HeroRef.InputJumpStart 
                    && HeroRef.VelocityY >= HeroParameters.JumpInterrupted
                )
                HeroRef.CallDeferred("InputJump", true);
        }

        public override void MoveBody(float delta)
        {
            base.MoveBody(delta);
            
            if (HeroRef.VelocityY > HeroParameters.MaxFallWall)
                HeroRef.VelocityY = HeroParameters.MaxFallWall;
            if (HeroRef.IsOnFloor())
                HeroRef.SwitchState(Hero.States.Ground);

            if (!HeroRef.IsOnWall() && HeroRef.NodeTimerCoyote.TimeLeft == 0)
            {
                if (HeroRef.InputMoveDirection == 0)
                    HeroRef.VelocityX = 0;
                HeroRef.VelocityX = -_wallDirection;
                HeroRef.SwitchState(Hero.States.Air);
            }

        }

        public override void InputMove(float direction)
        {
            if (_wallDirection == -Math.Sign(direction))
            {
                HeroRef.VelocityX = -HeroParameters.JumpWallHorizontal * _wallDirection;
                HeroRef.NodeFlipH.Scale = new Vector2(-_wallDirection, 1);
                HeroRef.NodeTimerCoyote.Start();
            }
        }

        public override void InputJump(bool pressed)
        {
            if (pressed)
            {
                HeroRef.NodeFlipH.Scale = new Vector2(-_wallDirection, 1);
                HeroRef.NodeTimerCoyote.Stop();
                HeroRef.VelocityX = -HeroParameters.JumpWallHorizontal * _wallDirection;
                HeroRef.VelocityY = HeroParameters.JumpWall;
                HeroRef.SwitchState(Hero.States.Air);
            }
            else if (HeroRef.VelocityY < HeroParameters.JumpInterrupted)
                HeroRef.VelocityY = HeroParameters.JumpInterrupted;
        }
    }
}
