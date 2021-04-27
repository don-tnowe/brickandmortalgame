using Godot;

namespace BrickAndMortal.Scripts.Menus
{
	class TownToDungeon : BaseMenu
	{
		public override void _Ready()
		{
			GetNode<Label>("AnchorCenter/HideOnClick/Day").Text = Tr("TownDay") + " " + SaveData.CurCrawl;
		}
		
		public override void OpenMenu()
		{
			GetNode<Control>("AnchorCenter/HideOnClick/ButtonDescend").GrabFocus();
		}
		
		private void Descend()
		{
			GetNode<AnimationPlayer>("Anim").Play("Go");
		}
		
		private void ExitGame()
		{
			GetTree().Quit();
		}
		
		private void ToDungeon()
		{
			GetTree().ChangeScene("res://Scenes/Screens/Dungeon.tscn");
		}
	}
}




