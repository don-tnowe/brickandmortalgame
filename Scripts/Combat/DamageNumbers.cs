using Godot;

namespace BrickAndMortal.Scripts.Combat
{
	public class DamageNumbers : Node2D
	{
		public void DisplayNumber(int damage, int effectivenessLevel)
		{
			GetNode<Label>("Node/Label").Text = damage.ToString();
			GetNode<AnimationPlayer>("Anim").Play("Eff" + effectivenessLevel);
		}
	}
}
