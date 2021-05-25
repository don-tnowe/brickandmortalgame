using System;
using Godot;

namespace BrickAndMortal.Scripts.Combat.Enemies
{
	class EnemyRockCrab : BaseEnemy
	{
		private RayCast2D _nodeRayGround;

		public override void _Ready()
		{
			base._Ready();
			_nodeRayGround = GetNode<RayCast2D>("RayGround");
			PhysVelocityXFlip = XFlip;
		}

		public override void PhysMoveBody(float delta)
		{
			if (IsOnWall() || !_nodeRayGround.IsColliding())
			{
				if (CurState == 0)
				{
					PhysVelocityXFlip = -PhysVelocityXFlip;
					SetXFlipped(PhysVelocityXFlip < 0);
				}
				else
					PhysVelocityX = 0;
			}
			base.PhysMoveBody(delta);
			
		}

		private void StartMoving()
		{
			CurState = 0; // Crawl
			PlayAnim("Move");
		}

		private void HeroProximity(Area2D area)
		{
			CurState = 2; // Attack
			PlayAnim("Attack");
		}
	}
}




