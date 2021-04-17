using BrickAndMortal.Scripts.DungeonFeatures;
using Godot;

namespace BrickAndMortal.Scripts.Menus
{
	class MenuDungeonMapFake : BaseMenu
	{
		private Tween _nodeTween;

		public override void _Ready()
		{
			Title = "MenuMap";
			_nodeTween = GetNode<Tween>("../../../Tween");
		}


		public override void OpenMenu()
		{
			base.OpenMenu();

			GrabFocus();
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
	}
}
