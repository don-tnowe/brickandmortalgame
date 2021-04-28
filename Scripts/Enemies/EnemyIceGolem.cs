using BrickAndMortal.Scripts.Combat;
using System;
using Godot;

namespace BrickAndMortal.Scripts.Enemies
{
	class EnemyIceGolem : BaseEnemy
	{
		[Export]
		private PackedScene _sceneAtk;

		private RayCast2D _nodeRayGround;

		public override void _Ready()
		{
			base._Ready();
			_nodeRayGround = GetNode<RayCast2D>("RayGround");
			PhysicsEnabled = true;
			PhysVelocityXFlip = (int)Scale.x;
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
			CurState = 0; // Walk
			PlayAnim("Move");
		}

		public void HeroProximity(object area)
		{
			CurState = 2; // Attack
			PlayAnim("Attack");
		}

		public void Attack()
		{
			//SpawnAtk(_sceneAtk);
		}
	}
}
