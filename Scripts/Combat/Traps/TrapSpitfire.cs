using Godot;

namespace BrickAndMortal.Scripts.Combat.Traps
{
	public class TrapSpitfire : Sprite
	{
		[Export]
		private float _firingCooldown = 1;
		[Export]
		private PackedScene _sceneProjectile;
		
		private AnimationPlayer _nodeAnim;
		
		public override void _Ready() 
		{
			_nodeAnim = GetNode<AnimationPlayer>("Anim");
			GetNode<Timer>("Timer").Start(_firingCooldown);
		}
		
		private void PrepareFire()
		{
			if (_firingCooldown < 0.5f)
				_nodeAnim.Seek(0.5f);
				
			else
				_nodeAnim.Seek(0);
		}
		
		private void Fire() 
		{
			var node = (CombatAttack)_sceneProjectile.Instance();
			GetNode("/root/Node/Room").AddChild(node);
			node.GlobalPosition = GlobalPosition;
			node.Launch(GlobalRotation);
		}
	}
}


