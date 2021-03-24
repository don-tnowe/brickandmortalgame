
namespace BrickAndMortal.Scripts.HeroStates
{
    class HeroStateImmobile : HeroState
    {
        public HeroStateImmobile(Hero hero) : base(hero) { }

        public override void MoveBody(float delta)
        {
            // Stop right there!!!
        }
    }
}
