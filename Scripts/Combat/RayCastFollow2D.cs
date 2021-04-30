using BrickAndMortal.Scripts.HeroComponents;
using Godot;

namespace BrickAndMortal.Scripts.Combat
{
	class RayCastFollow2D : RayCast2D
	{
		[Export]
		private bool _targetsHero = true;

		private Node2D _nodeTarget;

		public override void _Ready()
		{
			if(_targetsHero)
				_nodeTarget = (Hero)GetNode("/root/Node/Hero");
		}

		new public bool IsColliding()
		{
			if (_nodeTarget != null)
			{
				CastTo = (_nodeTarget.GlobalPosition - GlobalPosition);
				GlobalScale = Vector2.One;
				ForceRaycastUpdate();
				return base.IsColliding();
			}
			else
			{
				return false;
			}
		}
	}
}
