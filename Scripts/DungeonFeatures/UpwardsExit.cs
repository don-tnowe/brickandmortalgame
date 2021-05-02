using Godot;

namespace BrickAndMortal.Scripts.DungeonFeatures
{
	public class UpwardsExit : Area2D
	{
		public override void _Ready()
		{
			GetNode("../../..").Connect(nameof(Room.UnblockBottomExits), this, "queue_free");
		}
		
		public override void _Input(InputEvent @event)
		{
			if (@event.GetActionStrength("jump") > 0)
				QueueFree();
		}

		private void HeroExited(object body)
		{
			QueueFree();
		}
	}
}



