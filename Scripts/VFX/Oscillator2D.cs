using Godot;

namespace BrickAndMortal.Scripts.VFX
{
	class Oscillator2D : Node2D
	{
		[Export]
		public float Magnitude = 10;
		[Export]
		public Vector2 InitialPosition;

		private System.Random _random = new System.Random();

		public override void _Ready()
		{
			InitialPosition = Position;
		}

		public override void _Process(float delta)
		{
			if (Magnitude == 0)
				Position = InitialPosition;
			else
				Position = new Vector2(
					InitialPosition.x + ((float)_random.NextDouble() - 0.5f) * Magnitude,
					InitialPosition.y + ((float)_random.NextDouble() - 0.5f) * Magnitude
					);
		}
	}
}
