using System;
using Godot;

namespace BrickAndMortal.Scripts.HeroStates
{
    public abstract class HeroState
    {
        public Hero HeroRef;

        public HeroState(Hero hero)
        {
            HeroRef = hero;
        }

        public virtual void MoveBody(float delta)
        {
            HeroRef.LastVelocity = new Vector2(HeroRef.VelocityX, HeroRef.VelocityY);
            HeroRef.VelocityY += delta * (
                            HeroRef.VelocityY < 0 ?
                            HeroParameters.GravityJump : HeroParameters.GravityFall
                        );
            
            var NewVelocity = HeroRef.MoveAndSlide(new Vector2(HeroRef.VelocityX, HeroRef.VelocityY), Vector2.Up);
            HeroRef.VelocityY = NewVelocity.y;
            if (HeroRef.VelocityX != 0)
                HeroRef.VelocityXSign = Math.Sign(HeroRef.VelocityX);
        }

        public virtual void ExitState() { }

        public virtual void InputMove(float direction) { }

        public virtual void InputJump(bool pressed) { }

        public virtual void InputAttack() { }

    }
}
