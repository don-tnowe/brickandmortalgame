using System;

namespace BrickAndMortal.Scripts.HeroStates
{
    class HeroStateAir : HeroState
    {
        public HeroStateAir(Hero hero) : base(hero) { }

        public override void MoveBody(float delta)
        {
            base.MoveBody(delta);

            if (HeroRef.IsOnFloor())
                HeroRef.SwitchState(Hero.States.Ground);
            if (HeroRef.IsOnWall())
                HeroRef.SwitchState(Hero.States.Wall);
            if (HeroRef.InputMoveDirection != 0)
                if (HeroRef.VelocityX * HeroRef.InputMoveDirection < HeroParameters.MaxSpeed)
                    HeroRef.VelocityX += HeroParameters.AccelAir * HeroRef.InputMoveDirection * delta;
            if (HeroRef.VelocityY > HeroParameters.MaxFall)
                HeroRef.VelocityY = HeroParameters.MaxFall;
        }

        public override void InputJump(bool pressed)
        {
            if (!pressed && HeroRef.VelocityY < HeroParameters.JumpInterrupted)
                HeroRef.VelocityY = HeroParameters.JumpInterrupted;
        }
    }
}
