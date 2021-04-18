using Godot;

namespace BrickAndMortal.Scripts.InteractableProps
{
	public class InteractableArea : Area2D
	{
		[Export]
		public string Message = "";
		[Export]
		private bool _closesOnInteract = false;
		[Export]
		private bool _destroysOnInteract = false;
		[Export]
		private bool _showsOwnMessage = true;

		[Signal]
		public delegate void Interacted(InteractableArea with);

		public bool Interactable {
			set 
			{
				Interactable = value;
				GetChild<CollisionShape2D>(0).Disabled = !value;
			}
		}

		public void HeroEntered(HeroComponents.Hero hero)
		{
			hero.NodeInteractableArea?.HeroExited(hero);
			hero.NodeInteractableArea = this;

			if (!_showsOwnMessage)
				return;

			var display = hero.GetNode<ItemOperations.ItemStatDisplay>("ItemStatDisplay");
			display.DisplayItemData(Message);
			display.ShowPopup();
		}

		public void HeroExited(HeroComponents.Hero hero)
		{
			if (hero.NodeInteractableArea != this)
				return;

			hero.NodeInteractableArea = null;
			hero.GetNode<ItemOperations.ItemStatDisplay>("ItemStatDisplay").HidePopup();
		}

		public void Interact(HeroComponents.Hero hero)
		{
			EmitSignal(nameof(Interacted), this);
			if (_closesOnInteract)
				HeroExited(hero);
			else
				CallDeferred("emit_signal", "body_entered", hero);
		}
	}
}




