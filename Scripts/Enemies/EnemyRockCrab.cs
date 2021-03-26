using System;
using BrickAndMortal.Scripts.Combat;
using Godot;

namespace BrickAndMortal.Scripts.Enemies
{
	class EnemyRockCrab : BaseEnemy
	{
		private PackedScene _scenesAtk = ResourceLoader.Load<PackedScene>("res://Scenes/Hazards/Enemies/RockCrabMelee.tscn");

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
			CurState = 0;
			PlayAnim("Move");
		}

		public void HeroProximity(object area)
		{
			CurState = 2;
			PlayAnim("Attack");
		}


		public void Attack()
		{
			SpawnAtk(_scenesAtk);
		}
	}
}
