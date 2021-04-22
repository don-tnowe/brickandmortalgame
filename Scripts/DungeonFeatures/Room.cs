using BrickAndMortal.Scripts.HeroComponents;
using System;
using Godot;

namespace BrickAndMortal.Scripts.DungeonFeatures
{
	class Room : Node
	{
		[Export]
		public Color LightColor = new Color(1, 1, 1, 1);
		[Export]
		private int[] _mapMods = new int[4];
		[Signal]
		private delegate void AllEnemiesDefeated();
		[Signal]
		private delegate void UnblockBottomExits();

		private DungeonBuilder _nodeDungeonBuilder;
		private Hero _nodeHero;
		private Node _nodeEnemies;
		private Node _nodeDoors;
		private Node _nodePersistentObjects;
		private Position2D _nodeScrollBottomRight;

		private bool[] _slainEnemies;
		private int _remainingEnemies = 0;
		private bool[] _blockedExits;
		private string[] _persistentObjects;

		public override void _Ready()
		{
			_nodeDungeonBuilder = GetNode<DungeonBuilder>("..");
			_nodeHero = GetNode<Hero>("../Hero");
			_nodeEnemies = GetNode("Enemies");
			_nodeDoors = GetNode("Doors");
			_nodePersistentObjects = GetNode("PersistentObjects");
			_nodeScrollBottomRight = GetNode<Position2D>("ScrollBottomRight");

			var cam = _nodeHero.NodeCam;
			cam.LimitRight = (int)_nodeScrollBottomRight.Position.x;
			cam.LimitBottom = (int)_nodeScrollBottomRight.Position.y;
		}

		private void EnemyDefeated(int id)
		{
			_slainEnemies[id] = true;
			_remainingEnemies--;
			if (_remainingEnemies <= 0)
			{
				EmitSignal(nameof(AllEnemiesDefeated));
				for (int i = 0; i < _mapMods.Length; i++)
					if (_mapMods[i] == 0 || _mapMods[i] >= 8 && _mapMods[i] < 16)
					{
						_mapMods[i] = 2;
						break;
					}
			}
		}

		public void LoadRoom(RoomData from)
		{
			_slainEnemies = from.SlainEnemies;
			_blockedExits = from.BlockedExits;
			_persistentObjects = from.PersistentObjects;
			_remainingEnemies = 0;
			for (int i = 0; i < _slainEnemies.Length; ++i)
			{
				if (_slainEnemies[i])
					_nodeEnemies.GetChild(i).QueueFree();
				else
					_remainingEnemies++;
			}
			for (int i = 0; i < _persistentObjects.Length; ++i)
			{
				((IDungeonPersistent)_nodePersistentObjects.GetChild(i)).DeserializeFrom(_persistentObjects[i]);
			}
		}

		public void LoadRoom()
		{
			_slainEnemies = new bool[_nodeEnemies.GetChildCount()];
			_remainingEnemies = _nodeEnemies.GetChildCount();
			_persistentObjects = new string[_nodePersistentObjects.GetChildCount()];
			for (int i = 0; i < _persistentObjects.Length; ++i)
			{
				var node = ((IDungeonPersistent)_nodePersistentObjects.GetChild(i));
				node.Initialize();
			}
		}

		public void PlaceHeroAtDoor(int toMapX, int toMapY, Vector2 positionOffset)
		{
			foreach (Node ii in _nodeDoors.GetChildren())
			{
				var i = (RoomDoor)ii;
				if (i.ToMapX == toMapX && i.ToMapY == toMapY)
				{
					_nodeHero.GlobalPosition = i.GlobalPosition;
					switch (i.ExitDir)
					{
						case RoomDoor.Direction.Right:
							_nodeHero.Translate(new Vector2(12, positionOffset.y));
							break;
						case RoomDoor.Direction.Left:
							_nodeHero.Translate(new Vector2(-12, positionOffset.y));
							break;
						case RoomDoor.Direction.Up:
							_nodeHero.Translate(new Vector2(positionOffset.x, -24));
							_nodeHero.VelocityY = HeroParameters.Jump;
							_nodeHero.VelocityX = 0;
							break;
						case RoomDoor.Direction.Down:
							_nodeHero.Translate(new Vector2(positionOffset.x, 24));
							_nodeHero.VelocityY = 0;
							_nodeHero.VelocityX = 0;
							break;
					}
					_nodeHero.NodeCam.ForceUpdateScroll();
					_nodeHero.NodeCam.ResetSmoothing();
					if (i.ExitDir != RoomDoor.Direction.Up)
						EmitSignal(nameof(UnblockBottomExits));
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
			for (int i = 0; i < _persistentObjects.Length; ++i)
			{
				_persistentObjects[i] = ((IDungeonPersistent)_nodePersistentObjects.GetChild(i)).GetSerialized();
			}
			return new RoomData()
			{
				ScenePath = Filename,
				SlainEnemies = _slainEnemies,
				BlockedExits = _blockedExits,
				PersistentObjects = _persistentObjects,
				MapMods = _mapMods
			};
		}
	}
}
