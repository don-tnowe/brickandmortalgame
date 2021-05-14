using BrickAndMortal.Scripts.HeroComponents;
using Godot;

namespace BrickAndMortal.Scripts.Combat
{
	class RayCastLOS2D : RayCast2D
	{
		public bool HasLOS(Node2D to) 
		{
			CastTo = (to.GlobalPosition - GlobalPosition);
			GlobalScale = Vector2.One;
			ForceRaycastUpdate();
			return base.IsColliding();
		}
	}
}
