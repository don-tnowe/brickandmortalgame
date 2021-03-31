using System.Collections.Generic;
using Godot;

namespace BrickAndMortal.Scripts.DungeonFeatures
{
	class DungeonBuilder : Node
	{
		public Dictionary<Vector2, RoomData> rooms = new Dictionary<Vector2,RoomData>();

		private int _onMapX = 0;
		private int _onMapY = 0;

		private Room _curRoom;
		public DungeonAreaPool CurPool;

		public override void _Ready()
		{
			_curRoom = GetNode<Room>("Room");
			_curRoom.LoadRoom();
			rooms.Add(new Vector2(_onMapX, _onMapY), new RoomData());
			GetNode<CanvasModulate>("LightColor").Color = GetNode<Room>("Room").LightColor;

			CurPool = ResourceLoader.Load<DungeonAreaPool>("res://Resources/DungeonAreaPools/Area0.tres");
		}

		public void SwitchRoom(int toMapX, int toMapY, Vector2 positionOffset)
		{
			rooms[new Vector2(_onMapX, _onMapY)] = _curRoom.GetSerialized();

			Room newRoom;
			RoomData newRoomData = null;

			if (rooms.ContainsKey(new Vector2(_onMapX + toMapX, _onMapY + toMapY)))
			{
				newRoomData = rooms[new Vector2(_onMapX + toMapX, _onMapY + toMapY)];
				newRoom = (Room)ResourceLoader.Load<PackedScene>(newRoomData.ScenePath).Instance();
			}
			else
			{
				rooms.Add(new Vector2(_onMapX + toMapX, _onMapY + toMapY), new RoomData());
				newRoom = (Room)ResourceLoader.Load<PackedScene>(CurPool.GetRandomRoomPath()).Instance();
			}

			AddChild(newRoom);
			if (newRoomData == null)
				newRoom.LoadRoom();
			else
				newRoom.LoadRoom(newRoomData);

			newRoom.PlaceHeroAtDoor(-toMapX, -toMapY, positionOffset);
			_curRoom.QueueFree();

			_curRoom = newRoom;
			_onMapX += toMapX;
			_onMapY += toMapY;
			GetNode<CanvasModulate>("LightColor").Color = newRoom.LightColor;
		}
	}
}
