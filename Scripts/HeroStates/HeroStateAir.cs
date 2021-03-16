using System;

namespace BrickAndMortal.Scripts.HeroStates
{
    class HeroStateAir : HeroState
    {
        public HeroStateAir(Hero hero) : base(hero) { }

        public override void MoveBody(float delta)
        {
            base.MoveBody(delta);

            if (Hero.IsOnFloor())
                Hero.SwitchState(Hero.States.Ground);
            if (Hero.IsOnWall())
                Hero.SwitchState(Hero.States.Wall);
            if (Hero.InputMoveDirection != 0)
                if (Hero.VelocityX * Hero.InputMoveDirection < HeroParameters.MaxSpeed)
                    Hero.VelocityX += HeroParameters.AccelAir * Hero.InputMoveDirection * delta;
            if (Hero.VelocityY > 0)
            {
                if (Hero.VelocityY > HeroParameters.MaxFall)
                    Hero.VelocityY = HeroParameters.MaxFall;
                if (Hero.LastVelocity.y <= 0)
                    Hero.NodeAnim.Play("Fall");
            }
        }

        public override void InputJump(bool pressed)
        {
            if (!pressed && Hero.VelocityY < HeroParameters.JumpInterrupted)
                Hero.VelocityY = HeroParameters.JumpInterrupted;
        }
    }
}
