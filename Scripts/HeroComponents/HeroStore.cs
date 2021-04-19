using BrickAndMortal.Scripts.StoreFeatures;
using Godot;

namespace BrickAndMortal.Scripts.HeroComponents
{
	class HeroStore : Hero
	{
		[Export]
		private StreamTexture _loadTexture;

		public override void _Ready()
		{
			base._Ready();
			SwitchState(States.Immobile);
			NodeAnim.Play("StoreEnter");
			//NodeCam.LimitLeft = -10000000;
			//NodeCam.LimitRight = -10000000;
			GetNode<Sprite>("FlipH/Sprite").Texture = _loadTexture;
			_inStore = true;
		}

		public void StartDay()
		{
			NodeTween.InterpolateProperty(NodeCam, "zoom",
				Vector2.One, Vector2.One * 1.5f,
				1, Tween.TransitionType.Quart
				);
		}
	}
}
