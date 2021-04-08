using BrickAndMortal.Scripts.HeroComponents;
using Godot;

namespace BrickAndMortal.Scripts.DungeonFeatures
{
	class RoomDoor : Area2D
	{
		public enum Direction 
		{
			Right,
			Down,
			Left,
			Up
		}

		[Export]
		public int ToMapX = 1;
		[Export]
		public int ToMapY = 0;
		[Export]
		public Direction ExitDir;
		[Signal]
		public delegate void TransitionActivated(int toMapX, int toMapY, Vector2 positionOffset);

		private Vector2 _heroGlobalPosition;

		private void EnableCollision()
		{
			GetNode<CollisionShape2D>("Shape").Disabled = false;
		}
		
		private void TouchedHero(Hero hero)
		{
			_heroGlobalPosition = hero.GlobalPosition;
			GetNode<Timer>("Timer").Start();
			hero.SwitchState(Hero.States.Immobile);
			hero.NodeAnim.Play("RoomTransition");
		}
		
		private void StartTransition()
		{
			EmitSignal("TransitionActivated", ToMapX, ToMapY, _heroGlobalPosition - GlobalPosition);
		}
	}
}







