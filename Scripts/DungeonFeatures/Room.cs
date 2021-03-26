using System;
using Godot;

namespace BrickAndMortal.Scripts.DungeonFeatures
{
	class Room : Node2D
	{
		[Export]
		private readonly NodePath _nodePathDungeonBuilder = "..";
		[Export]
		private readonly NodePath _nodePathHero = "../Hero";
		[Signal]
		private delegate void AllEnemiesDefeated();

		private DungeonBuilder _nodeDungeonBuilder;
		private Hero _nodeHero;
		private Node _nodeEnemies;
		private Node _nodeDoors;
		private Position2D _nodeScrollBottomRight;

		private bool[] _slainEnemies; 
		private int _remainingEnemies = 0;
		private bool[] _blockedExits;

		public override void _Ready()
		{
			_nodeDungeonBuilder = GetNode<DungeonBuilder>(_nodePathDungeonBuilder);
			_nodeHero = GetNode<Hero>(_nodePathHero);
			_nodeEnemies = GetNode("Enemies");
			_nodeDoors = GetNode("Doors");
			_nodeScrollBottomRight = GetNode<Position2D>("ScrollBottomRight");

			var cam = GetNode<Hero>(_nodePathHero).NodeCam;
			cam.LimitRight = (int)_nodeScrollBottomRight.Position.x;
			cam.LimitBottom = (int)_nodeScrollBottomRight.Position.y;
		}

		private void EnemyDefeated(int id)
		{
			_slainEnemies[id] = true;
			_remainingEnemies--;
			if (_remainingEnemies <= 0)
				EmitSignal("AllEnemiesDefeated");
		}

		public void LoadRoom(RoomData from)
		{
			_slainEnemies = from.SlainEnemies;
			_blockedExits = from.BlockedExits;
			_remainingEnemies = 0;
			for (int i = 0; i < _slainEnemies.Length; ++i)
			{
				if (_slainEnemies[i])
					_nodeEnemies.GetChild(i).QueueFree();
				else
					_remainingEnemies++;
			}
		}

		public void LoadRoom()
		{
			_slainEnemies = new bool[_nodeEnemies.GetChildCount()];
			_remainingEnemies = _nodeEnemies.GetChildCount();
		}

		public void PlaceHeroAtDoor(int toMapX, int toMapY, Vector2 positionOffset)
		{
			foreach (Node ii in _nodeDoors.GetChildren())
			{
				var i = (RoomDoor)ii;
				if (i.ToMapX == toMapX && i.ToMapY == toMapY)
				{
					_nodeHero.GlobalPosition = i.GlobalPosition + positionOffset;
					switch (i.ExitDir)
					{
						case RoomDoor.Direction.Right:
							_nodeHero.Translate(new Vector2(40, 0));
							break;
						case RoomDoor.Direction.Left:
							_nodeHero.Translate(new Vector2(-40, 0));
							break;
						case RoomDoor.Direction.Up:
							_nodeHero.Translate(new Vector2(0, -56));
							_nodeHero.VelocityY = HeroParameters.Jump;
							break;
						case RoomDoor.Direction.Down:
							_nodeHero.Translate(new Vector2(0, 56));
							_nodeHero.VelocityY = 0;
							break;
					}
					_nodeHero.NodeCam.ForceUpdateScroll();
					_nodeHero.NodeCam.ResetSmoothing();
					/*
					_nodeHero.NodeCam.CallDeferred("reset_smoothing");*/
					return;
				}
			}
		}

		public void TransitionActivated(int toMapX, int toMapY, Vector2 positionOffset)
		{
			_nodeDungeonBuilder.SwitchRoom(toMapX, toMapY, positionOffset);
		}

		public RoomData GetSerialized()
		{
			return new RoomData(Filename, _slainEnemies, _blockedExits);
		}
	}
}
