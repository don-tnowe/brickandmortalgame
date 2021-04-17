using Godot;
using BrickAndMortal.Scripts.StoreFeatures;

namespace BrickAndMortal.Scripts.HeroComponents
{
	class HeroStore : Hero
	{
		[Export]
		private StreamTexture _loadTexture;
		
		public InteractableArea NodeInteractableArea;
		
		public override void _Ready()
		{
			base._Ready();
			GetNode<Sprite>("FlipH/Sprite").Texture = _loadTexture;
			NodeCam.Zoom = new Vector2(1.5f, 1.5f);
		}
		
		public override void InputAttack(bool pressed)
		{

		}
	}
}
