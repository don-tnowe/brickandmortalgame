using Godot;

namespace BrickAndMortal.Scripts.Combat.Traps
{
	public class TrapSpitfire : Sprite
	{
		[Export]
		private float _firingDelay = 1;
		[Export]
		private float _firingCooldown = 1;
		[Export]
		private PackedScene _sceneProjectile;
		
		private AnimationPlayer _nodeAnim;
		
		public override void _Ready() 
		{
			_nodeAnim = GetNode<AnimationPlayer>("Anim");
			GetNode<Timer>("Timer").Start(_firingDelay + 0.1f);
		}
		
		private void PrepareFire()
		{
			_nodeAnim.Play("Init");
			GetNode<Timer>("Timer").Start(_firingCooldown);
			
			if (_firingCooldown < 0.5f)
				_nodeAnim.Seek(0.5f);
				
			else
				_nodeAnim.Seek(0);
		}
		
		private void Fire() 
		{
			var node = (CombatAttack)_sceneProjectile.Instance();
			GetNode<DungeonFeatures.DungeonBuilder>("/root/Node").CurRoom.AddChild(node);
			node.GlobalPosition = GlobalPosition;
			node.Launch(GlobalRotation);
		}
	}
}


