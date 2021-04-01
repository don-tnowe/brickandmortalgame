using BrickAndMortal.Scripts.HeroStates;
using BrickAndMortal.Scripts.Combat;
using Godot;
using System;

namespace BrickAndMortal.Scripts.HeroComponents
{
	public class Hero : KinematicBody2D
	{
		public enum States
		{
			Immobile,
			Ground,
			Air,
			Wall,
			LedgeGrab,
			Hurt
		}
		[Export]
		public bool AnimationAllowed = true;

		public float VelocityX = 0;
		public float VelocityY = 0;
		public Vector2 LastVelocity = new Vector2();
		public int VelocityXSign = 1;
		public States CurState;
		public uint InstaTurnStart = 0;
		public bool CanAttack = true;

		public float InputMoveDirection = 0;
		public uint InputJumpStart = 0;
		public uint InputAttackStart = 0;

		private HeroState _state;

		public CollisionShape2D NodeShape;
		public Node2D NodeFlipH;
		public Sprite NodeSprite;
		public RayCast2D NodeRayGround;
		public RayCast2D NodeRayLedgeGrab;
		public RayCast2D NodeRayLedgeGrabHigher;
		public RayCast2D NodeRayLedgeGrabV;
		public RayCast2D NodeRayEnemyDetector;
		public Camera2D NodeCam;
		public CombatAttack NodeWeapon;
		public CombatActor NodeHitDetector;
		public AnimationPlayer NodeAnim;
		public AnimationPlayer NodeAnimWeapon;
		public Tween NodeTween;
		public Timer NodeTimerCoyote;
		public Timer NodeTimerAttack;

		private void InitializeNodeReferences()
		{
			NodeShape = GetNode<CollisionShape2D>("Shape");
			NodeFlipH = GetNode<Node2D>("FlipH");
			NodeSprite = GetNode<Sprite>("FlipH/Sprite");
			NodeRayGround = GetNode<RayCast2D>("FlipH/RayGround");
			NodeRayLedgeGrab = GetNode<RayCast2D>("FlipH/RayLedgeGrab");
			NodeRayLedgeGrabHigher = GetNode<RayCast2D>("FlipH/RayLedgeGrabHigher");
			NodeRayLedgeGrabV = GetNode<RayCast2D>("FlipH/RayLedgeGrabV");
			NodeRayEnemyDetector = GetNode<RayCast2D>("FlipH/RayEnemyDetector");
			NodeCam = GetNode<Camera2D>("Cam");
			NodeWeapon = GetNode<CombatAttack>("Weapon");
			NodeHitDetector = GetNode<CombatActor>("CombatCollision");
			NodeAnim = GetNode<AnimationPlayer>("Anim");
			NodeAnimWeapon = GetNode<AnimationPlayer>("Weapon/AnimWeapon");
			NodeTween = GetNode<Tween>("Tween");
			NodeTimerCoyote = GetNode<Timer>("TimerCoyote");
			NodeTimerAttack = GetNode<Timer>("TimerAttack");
		}

		public override void _Ready()
		{
			InitializeNodeReferences();
			SwitchState(States.Air);

			var heartHUD = GetNode("/root/Node/UI/HUDHearts");
			NodeHitDetector.Connect("HealthSetMax", heartHUD, "ResetHearts");
			NodeHitDetector.Connect("HealthSet", heartHUD, "UpdateHearts");
			NodeHitDetector.CallDeferred("UpdateMaxHp");
		}

		public override void _PhysicsProcess(float delta)
		{
			_state.MoveBody(delta);
		}

		public override void _Input(InputEvent @event)
		{
			if (@event is InputEventMouse)
				return;
			if (@event is InputEventKey && @event.IsEcho())
				return;

			if (@event.IsAction("ui_left") || @event.IsAction("ui_right"))
			{
				var direction = Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left");
				InputMove(direction);
			}
			if (@event.IsAction("jump"))
			{
				InputJump(@event.GetActionStrength("jump") > 0);
			}
			if (@event.IsAction("attack"))
			{
				InputAttack(@event.GetActionStrength("attack") > 0);
			}
		}

		public HeroState SwitchState(States state)
		{
			_state?.ExitState();
			CurState = state;
			//GD.Print(state);
			switch (state)
			{
				case States.Immobile:
					_state = new HeroStateImmobile(this);
					break;
				case States.Ground:
					_state = new HeroStateGround(this);
					break;
				case States.Air:
					_state = new HeroStateAir(this);
					break;
				case States.Wall:
					_state = new HeroStateWall(this);
					break;
				case States.LedgeGrab:
					_state = new HeroStateLedgeGrab(this);
					break;
			}
			return _state;
		}

		public void SwitchStateAuto()
		{
			CanAttack = true;
			if (IsOnFloor())
				SwitchState(States.Ground);
			else if (IsOnWall())
				SwitchState(States.Wall);
			else
				SwitchState(States.Air);
		}

		public void InputMove(float direction)
		{
			if (!(direction == 0 && InputMoveDirection == 0))
				_state.InputMove(direction);
			InputMoveDirection = direction;
		}

		public void InputJump(bool pressed)
		{
			InputJumpStart = pressed ? OS.GetTicksMsec() : 0;
			_state.InputJump(pressed);
		}

		public void InputAttack(bool pressed)
		{
			if (pressed)
				InputAttackStart = OS.GetTicksMsec();
			if (pressed && CanAttack)
			{
				CanAttack = false;
				NodeTimerAttack.Start();
				if (NodeRayEnemyDetector.IsColliding())
					NodeWeapon.Scale = new Vector2(Math.Sign(NodeRayEnemyDetector.GetCollisionPoint().x - GlobalPosition.x), 1);
				else if (InputMoveDirection == 0)
					NodeWeapon.Scale = new Vector2(NodeFlipH.Scale.x, 1);
				else
					NodeWeapon.Scale = new Vector2(InputMoveDirection, 1);
				_state.InputAttack();
				NodeAnimWeapon.Stop();
				NodeAnimWeapon.Play("Swing");
			}
		}

		public void AnimationAction(int action)
		{
			_state.AnimationAction(action);
		}

		private void Hurt(CombatAttack attack)
		{
			var newVelocity = new Vector2(64, -96);
			if (attack.GlobalPosition < GlobalPosition)
				VelocityX = newVelocity.x;
			else
				VelocityX = -newVelocity.x;
			VelocityY = newVelocity.y;
			_state = new HeroStateHurt(this);
		}

		private void CoyoteFall()
		{
			SwitchState(States.Air);
		}

		private void AttackReady()
		{
			CanAttack = true;
			if (InputAttackStart > OS.GetTicksMsec() - (NodeTimerAttack.WaitTime * 800))
				InputAttack(true);
		}

		private void HitEnemy(object area)
		{
			VelocityX *= 0.5f;
		}
	}
}











