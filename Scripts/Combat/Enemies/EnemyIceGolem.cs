using System;
using Godot;

namespace BrickAndMortal.Scripts.Combat.Enemies
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
					_nodeRayGround.ForceRaycastUpdate();
				}
				else
					PhysVelocityX = 0;
			}
			base.PhysMoveBody(delta);
			
		}

		private void StartMoving()
		{
			CurState = 0; // Walk
			PlayAnim("Move");
		}

		private void HeroProximity(object area)
		{
			CurState = 2; // Attack
			PlayAnim("Attack");
		}

		private void HeroProximityBehind(object area)
		{
			if (CurState == 0)
			{
				PhysVelocityXFlip = -PhysVelocityXFlip;
				SetXFlipped(PhysVelocityXFlip < 0);
			}
		}

		private void Attack()
		{
			SpawnAtk(_sceneAtk);
		}
	}
}


