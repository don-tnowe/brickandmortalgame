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
			SaveData.Screen = 2;
			NodeAnim.Play("StoreEnter");
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
		
		public void EndDay()
		{			
			if (SaveData.BestDayEarned < SaveData.LastDayEarned) 
				SaveData.BestDayEarned = SaveData.LastDayEarned;

			SaveData.SaveGame();

			GetTree().ChangeScene("res://Scenes/Screens/Town.tscn");
		}
	}
}
