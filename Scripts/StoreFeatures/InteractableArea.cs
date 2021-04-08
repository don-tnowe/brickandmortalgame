using Godot;

namespace BrickAndMortal.Scripts.StoreFeatures
{
    class InteractableArea : Area2D
    {
        [Export]
        public string Message = "Interact";
        [Signal]
        public delegate void Interacted();

        private void HeroEntered(HeroComponents.HeroStore hero)
        {
            hero.NodeInteractableArea.HeroExited(hero);
            hero.NodeInteractableArea = this;
        }

        private void HeroExited(HeroComponents.HeroStore hero)
        {
            if (hero.NodeInteractableArea == this)
                hero.NodeInteractableArea = null;
        }

        private void Interact()
        {

        }
    }
}