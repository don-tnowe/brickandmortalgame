using System.Collections.Generic;
using Godot;

namespace BrickAndMortal.Scripts.DungeonFeatures
{
	class DungeonBuilder : Node
	{
		public Dictionary<int, RoomData> Rooms = new Dictionary<int,RoomData>();
		public Room CurRoom;

		private const int _mapWidth = 64;
		
		private int _mapPos = 4128;

		private DungeonAreaPool _curPool;

		public override void _Ready()
		{
			CurRoom = GetNode<Room>("Room");
			CurRoom.LoadRoom();
			Rooms.Add(_mapPos, CurRoom.GetSerialized());
			GetNode<CanvasModulate>("LightColor").Color = GetNode<Room>("Room").LightColor;

			_curPool = ResourceLoader.Load<DungeonAreaPool>("res://Resources/DungeonAreaPools/Area0.tres");
		}

		public void SwitchRoom(int toMapX, int toMapY, Vector2 positionOffset)
		{
			Rooms[_mapPos] = CurRoom.GetSerialized();

			Room newRoom;
			RoomData newRoomData = null;
			var newPos = _mapPos + toMapX + (toMapY * _mapWidth);

			if (Rooms.ContainsKey(newPos))
			{
				newRoomData = Rooms[newPos];
				newRoom = (Room)ResourceLoader.Load<PackedScene>(newRoomData.ScenePath).Instance();
			}
			else
			{
				Rooms.Add(newPos, new RoomData());
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
			_mapPos = newPos;
			GetNode<CanvasModulate>("LightColor").Color = newRoom.LightColor;
		}

		public Vector2 GetPosOnMap()
		{
			return IntToPos(_mapPos);
		}
		
		public Vector2 IntToPos(int from)
		{
			return new Vector2(from % _mapWidth, from / _mapWidth);
		}

		public ItemOperations.Item GetRandomItem()
		{
			return _curPool.GetRandomItem();
		}
		
//		public override void _Input(InputEvent @event)
//		{
//			if (@event.IsAction("debug") && @event.GetActionStrength("debug") > 0)
//				SaveData.ItemBag.CollectItem( GetRandomItem());
//		}
		
		public string ToJSON()
		{
			return "{}"; //TODO
		}
		
		public void DeserializeFrom(string json)
		{
			//Also TODO
		}
	}
}


