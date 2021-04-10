using System.Collections.Generic;
using Godot;

namespace BrickAndMortal.Scripts.DungeonFeatures
{
	class DungeonBuilder : Node
	{
		public Dictionary<Vector2, RoomData> Rooms = new Dictionary<Vector2,RoomData>();
		public Room CurRoom;

		private int _onMapX = 0;
		private int _onMapY = 0;

		private DungeonAreaPool _curPool;

		public override void _Ready()
		{
			SaveData.LoadGame();
			CurRoom = GetNode<Room>("Room");
			CurRoom.LoadRoom();
			Rooms.Add(new Vector2(_onMapX, _onMapY), CurRoom.GetSerialized());
			GetNode<CanvasModulate>("LightColor").Color = GetNode<Room>("Room").LightColor;

			_curPool = ResourceLoader.Load<DungeonAreaPool>("res://Resources/DungeonAreaPools/Area0.tres");
		}

		public void SwitchRoom(int toMapX, int toMapY, Vector2 positionOffset)
		{
			Rooms[new Vector2(_onMapX, _onMapY)] = CurRoom.GetSerialized();

			Room newRoom;
			RoomData newRoomData = null;

			if (Rooms.ContainsKey(new Vector2(_onMapX + toMapX, _onMapY + toMapY)))
			{
				newRoomData = Rooms[new Vector2(_onMapX + toMapX, _onMapY + toMapY)];
				newRoom = (Room)ResourceLoader.Load<PackedScene>(newRoomData.ScenePath).Instance();
			}
			else
			{
				Rooms.Add(new Vector2(_onMapX + toMapX, _onMapY + toMapY), new RoomData());
				newRoom = (Room)ResourceLoader.Load<PackedScene>(_curPool.GetRandomRoomPath()).Instance();
			}

			AddChild(newRoom);
			if (newRoomData == null)
				newRoom.LoadRoom();
			else
				newRoom.LoadRoom(newRoomData);

			newRoom.PlaceHeroAtDoor(-toMapX, -toMapY, positionOffset);
			CurRoom.QueueFree();

			CurRoom = newRoom;
			_onMapX += toMapX;
			_onMapY += toMapY;
			GetNode<CanvasModulate>("LightColor").Color = newRoom.LightColor;
		}

		public Vector2 GetPosOnMap()
		{
			return new Vector2(_onMapX, _onMapY);
		}

		public object GetRandomItem()
		{
			return _curPool.GetRandomItem();
		}
	}
}


