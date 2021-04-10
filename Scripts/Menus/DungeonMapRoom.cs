using BrickAndMortal.Scripts.DungeonFeatures;
using Godot;

namespace BrickAndMortal.Scripts.Menus
{
	class DungeonMapRoom : Control
	{
		private const int _iconMaxCount = 4;

		private Sprite[] _nodesIcons = new Sprite[4];
		private int[] _displayedMods = new int[_iconMaxCount];

		public override void _Ready()
		{
			for (int i = 0; i < _iconMaxCount; i++)
				_nodesIcons[i] = GetNode("Icons").GetChild<Sprite>(i);
		}

		public void DisplayRoom(RoomData room, bool isCurrent)
		{
			DisplayMods(room, isCurrent);
			ArrangeIcons();
		}

		private void DisplayMods(RoomData room, bool isCurrent)
		{
			_displayedMods = new int[_iconMaxCount];

			if (isCurrent)
				_displayedMods[0] = 1;
			var iOffset = _displayedMods[0];
			for (int i = iOffset; i < _iconMaxCount; i++)
			{
				if (room.MapMods.Length <= i)
					_displayedMods[i] = 0;
				else
					_displayedMods[i] = room.MapMods[i - iOffset];
			}
		}

		private void ArrangeIcons()
		{
			var count = 0;
			for (int i = 0; i < _iconMaxCount; i++)
				if (_displayedMods[i] != 0)
				{
					_nodesIcons[i].Frame = _displayedMods[i];
					_nodesIcons[i].Visible = true;  
					count++;
				}
				else
					_nodesIcons[i].Visible = false;

			switch (count)
			{
				case 0:
					break;
				case 1:
					_nodesIcons[0].Position = new Vector2();
					break;
				case 2:
					_nodesIcons[0].Position = new Vector2(-4, -4);
					_nodesIcons[1].Position = new Vector2(4, 4);
					break;
				case 3:
					_nodesIcons[0].Position = new Vector2(-5, -4);
					_nodesIcons[1].Position = new Vector2(5, -4);
					_nodesIcons[2].Position = new Vector2(0, 4);
					break;
				default:
					_nodesIcons[0].Position = new Vector2(0, -7);
					_nodesIcons[1].Position = new Vector2(-7, 0);
					_nodesIcons[2].Position = new Vector2(7, 0);
					_nodesIcons[3].Position = new Vector2(0, 7);
					break;
			}
		}
	}
}


