using System;
using System.Collections;
using Godot;

namespace BrickAndMortal.Scripts.DungeonFeatures
{
	class DungeonBuilder : Node
	{
		public Hashtable rooms = new Hashtable();

		private int _onMapX = 0;
		private int _onMapY = 0;

		private Room _curRoom;

		public override void _Ready()
		{
			_curRoom = GetNode<Room>("Room");
			_curRoom.LoadRoom();
			rooms.Add(new Vector2(_onMapX, _onMapY), new RoomData());
		}

		public void SwitchRoom(int toMapX, int toMapY, Vector2 positionOffset)
		{
			rooms[new Vector2(_onMapX, _onMapY)] = _curRoom.GetSerialized();

			Room newRoom;
			RoomData newRoomData = null;

			if (rooms.ContainsKey(new Vector2(_onMapX + toMapX, _onMapY + toMapY)))
			{
				newRoomData = (RoomData)rooms[new Vector2(_onMapX + toMapX, _onMapY + toMapY)];
				newRoom = (Room)ResourceLoader.Load<PackedScene>(newRoomData.ScenePath).Instance();
			}
			else
			{
				rooms.Add(new Vector2(_onMapX + toMapX, _onMapY + toMapY), new RoomData());
				newRoom = (Room)ResourceLoader.Load<PackedScene>("res://Scenes/Rooms/TestRoom2.tscn").Instance();
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
		}
	}
}
