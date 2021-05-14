using System;
using Godot;

namespace BrickAndMortal.Scripts.Combat.Enemies
{
	class EnemySpitfly : BaseEnemy
	{
		[Export]
		private PackedScene _sceneAtk;

		private const float _speed = 48;
		private const float _proximityDistanceSquared = 32 * 32;

		private RayCast2D _nodeRayGround;
		private RayCastLOS2D _nodeRayLOS;
		private Node2D _nodeTarget;
		
		public override void _Ready()
		{
			base._Ready();
			_nodeRayGround = GetNode<RayCast2D>("RayGround");
			_nodeRayLOS = GetNode<RayCastLOS2D>("LOS");
			_nodeTarget = (Node2D)GetNode("/root/Node/Hero");
		}

		public override void PhysMoveBody(float delta)
		{
			base.PhysMoveBody(delta);
			if (GetSlideCount() > 0)
			{
				var vec = LastFramePhysVelocity.Bounce(GetSlideCollision(0).Normal);
				PhysVelocityX = vec.x;
				PhysVelocityY = vec.y;
				SetXFlipped(PhysVelocityX < 0);
			}
		}

		public void StartMoving()
		{
			if (!_nodeRayLOS.HasLOS(_nodeTarget))
			{
				SetXFlipped(_nodeRayLOS.CastTo.x < 0);

				if (CurState != 2)
				{
					CurState = 2; //Attack

					PhysVelocityX = 0;
					PhysVelocityY = 0;

					PlayAnim("Attack");
				}

				else if (_nodeRayGround.CastTo.LengthSquared() < _proximityDistanceSquared)
				{
					CurState = 1; //Flee

					var vec = new Vector2(-XFlip, -2).Normalized();
					PhysVelocityX = -vec.x * _speed;
					PhysVelocityY = -vec.y * _speed;

					PlayAnim("Move");
				}

				else
				{
					CurState = 0; //Wander

					var vec = GetRandomVec();
					PhysVelocityX = vec.x * _speed;

					if (_nodeRayGround.IsColliding())
						PhysVelocityY = vec.y * _speed;

					else
						PhysVelocityY = Math.Abs(vec.y * _speed);

					PlayAnim("Move");
				}
			}

			else
			{
				CurState = 0; //Wander

				var vec = GetRandomVec();
				PhysVelocityX = vec.x * _speed;
				PhysVelocityY = vec.y * _speed;
				SetXFlipped(PhysVelocityX < 0);

				PlayAnim("Move");
			}
		}

		private Vector2 GetRandomVec()
		{
			return new Vector2(
				(float)(_random.NextDouble() - 0.5), 
				(float)(_random.NextDouble() - 0.5)
				).Normalized();
		}

		public void Attack()
		{
			SpawnAtk(_sceneAtk, true, _nodeRayLOS.CastTo);
		}
		
		public override void Hurt(CombatAttack byAttack)
		{
			base.Hurt(byAttack);
			
			if (byAttack.HasNode(byAttack.Attacker))
				_nodeTarget = (Node2D)byAttack.GetNode(byAttack.Attacker);
		}
	}
}




