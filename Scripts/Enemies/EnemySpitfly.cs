using BrickAndMortal.Scripts.Combat;
using System;
using Godot;

namespace BrickAndMortal.Scripts.Enemies
{
	class EnemySpitfly : BaseEnemy
	{
		[Export]
		private PackedScene _sceneAtk;

		private RayCast2D _nodeRayGround;

		public override void _Ready()
		{
			base._Ready();
			_nodeRayGround = GetNode<RayCast2D>("RayGround");
			
		}

		public override void PhysMoveBody(float delta)
		{
			if (IsOnWall() || !_nodeRayGround.IsColliding())
			{
				if (CurState == 0)
				{
					PhysVelocityXFlip = -PhysVelocityXFlip;
					Scale *= new Vector2(-1, 1);
				}
				else
					PhysVelocityX = 0;
			}
			base.PhysMoveBody(delta);
			
		}

		public void StartMoving()
		{
			CurState = 0; 
			PlayAnim("Move");
		}

		public void HeroProximity(object area)
		{
			CurState = 1; // Flee
		}

		private void HeroProximityExited(object body)
		{
			if (CurState == 2)
				CurState = 0; // Wander
		}

		public void Attack()
		{
			SpawnAtk(_sceneAtk);
		}
	}
}


