using BrickAndMortal.Scripts.DungeonFeatures;
using Godot;

namespace BrickAndMortal.Scripts.Menus
{
	class MenuDungeonMap : BaseMenu
	{
		[Export]
		private PackedScene _sceneRoom;

		private float _roomSize = 18;

		private DungeonBuilder _nodeDungeonBuilder;
		private Control _nodeRooms;
		private Tween _nodeTween;

		private Sprite _nodeHeroMarker;

		public override void _Ready()
		{
			_nodeDungeonBuilder = GetNode<DungeonBuilder>("../../../..");
			_nodeRooms = GetNode<Control>("Box/Center/Frame/MapClip/MapCenter/Rooms");
			_nodeTween = GetNode<Tween>("../../../Tween");

			GetNode(FocusNeighbourLeft)		.Connect("focus_entered", this, "MoveMap", new Godot.Collections.Array { -1, 0 });
			GetNode(FocusNeighbourRight)	.Connect("focus_entered", this, "MoveMap", new Godot.Collections.Array { 1, 0 });
			GetNode(FocusNeighbourTop)		.Connect("focus_entered", this, "MoveMap", new Godot.Collections.Array { 0, -1 });
			GetNode(FocusNeighbourBottom)	.Connect("focus_entered", this, "MoveMap", new Godot.Collections.Array { 0, 1 });
		}


		public override void OpenMenu()
		{
			base.OpenMenu();

			var idx = 0;
			var posOnMap = _nodeDungeonBuilder.GetPosOnMap();

			GrabFocus();
			_nodeRooms.RectPosition = new Vector2(-0.5f - posOnMap.x, -0.5f - posOnMap.y) * _roomSize;

			foreach (Vector2 i in _nodeDungeonBuilder.Rooms.Keys)
			{
				DungeonMapRoom newRoom;
				if (_nodeRooms.GetChildCount() <= idx)
				{
					newRoom = (DungeonMapRoom)_sceneRoom.Instance();
					_nodeRooms.AddChild(newRoom);
				}
				else
				{
					newRoom = _nodeRooms.GetChild<DungeonMapRoom>(idx);
				}

				newRoom.RectPosition = i * _roomSize;

				if (i == posOnMap)
				{
					_nodeHeroMarker = newRoom.GetNode("Icons").GetChild<Sprite>(0);
					newRoom.DisplayRoom(_nodeDungeonBuilder.CurRoom.GetSerialized(), true);
				}
				else
				{
					newRoom.DisplayRoom(_nodeDungeonBuilder.Rooms[i], false);
				}

				idx++;
			}

			while(idx < _nodeRooms.GetChildCount())
			{
				_nodeRooms.GetChild(idx).QueueFree();
				idx++;
			}

			_nodeTween.InterpolateProperty(GetNode("Box"), "anchor_left",
				0.5f, 0,
				0.25f, Tween.TransitionType.Back, Tween.EaseType.Out
				);
			_nodeTween.InterpolateProperty(GetNode("Box"), "anchor_right",
				0.5f, 1,
				0.25f, Tween.TransitionType.Back, Tween.EaseType.Out
				);
			_nodeTween.Start();
		}

		private void MoveMap(int x, int y)
		{
			if (!IsOpen)
				return;

			GrabFocus();

			if (x != 0)
				_nodeTween.InterpolateProperty(_nodeRooms, "margin_left",
					   _nodeRooms.MarginLeft, _nodeRooms.MarginLeft - _roomSize * x,
					   0.15f, Tween.TransitionType.Cubic, Tween.EaseType.Out
					   );
			if (y != 0)
				_nodeTween.InterpolateProperty(_nodeRooms, "margin_top",
					   _nodeRooms.MarginTop, _nodeRooms.MarginTop - _roomSize * y,
					   0.15f, Tween.TransitionType.Cubic, Tween.EaseType.Out
					   );

			_nodeTween.Start();
		}

		private void PingHero()
		{
			if (!IsOpen || _nodeHeroMarker == null)
				return;

			_nodeTween.InterpolateProperty(_nodeHeroMarker, "scale",
				new Vector2(1.6f, 1.6f), Vector2.One,
				1, Tween.TransitionType.Elastic, Tween.EaseType.Out
				);
			_nodeTween.Start();
		}
	}
}
