using BrickAndMortal.Scripts.DungeonFeatures;
using Godot;

namespace BrickAndMortal.Scripts.Menus
{
	class MenuDungeonMap : BaseMenu
	{
		private DungeonBuilder _nodeDungeonBuilder;
		private Tween _nodeTween;

		public override void _Ready()
		{
			_nodeDungeonBuilder = GetNode<DungeonBuilder>("../../..");
			_nodeTween = GetNode<Tween>("../Tween");
		}


		public override void OpenMenu()
		{
			base.OpenMenu();
		}

		public override void CloseMenu()
		{
			base.CloseMenu();
			Visible = false;
		}
	}
}
