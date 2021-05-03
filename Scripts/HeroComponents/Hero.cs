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

		public float InputMoveDirection = 0;
		public uint InputJumpStart = 0;
		public uint InputAttackStart = 0;
		protected bool _inStore = false;
		private bool _canAttack = true;

		private HeroState _controller;

		public InteractableProps.InteractableArea NodeInteractableArea;

		public Node2D NodeFlipH;
		public RayCast2D NodeRayGround;
		public RayCast2D NodeRayLedgeGrab;
		public RayCast2D NodeRayLedgeGrabHigher;
		public RayCast2D NodeRayLedgeGrabV;
		public RayCast2D NodeRayEnemyDetector;
		public Camera2D NodeCam;
		public CombatAttack NodeWeapon;
		public AnimationPlayer NodeAnim;
		public AnimationPlayer NodeAnimWeapon;
		public Tween NodeTween;
		public Timer NodeTimerCoyote;
		public Timer NodeTimerAttack;

		private void InitializeNodeReferences()
		{
			NodeFlipH = GetNode<Node2D>("FlipH");
			NodeRayGround = GetNode<RayCast2D>("FlipH/RayGround");
			NodeRayLedgeGrab = GetNode<RayCast2D>("FlipH/RayLedgeGrab");
			NodeRayLedgeGrabHigher = GetNode<RayCast2D>("FlipH/RayLedgeGrabHigher");
			NodeRayLedgeGrabV = GetNode<RayCast2D>("FlipH/RayLedgeGrabV");
			NodeRayEnemyDetector = GetNode<RayCast2D>("FlipH/RayEnemyDetector");
			NodeCam = GetNode<Camera2D>("Cam");
			NodeWeapon = GetNode<CombatAttack>("Weapon");
			NodeAnim = GetNode<AnimationPlayer>("Anim");
			NodeAnimWeapon = GetNode<AnimationPlayer>("Weapon/AnimWeapon");
			NodeTween = GetNode<Tween>("Tween");
			NodeTimerCoyote = GetNode<Timer>("TimerCoyote");
			NodeTimerAttack = GetNode<Timer>("TimerAttack");

			if (HasNode("/root/Node/UI/HUDHearts"))
			{
				var heartHUD = GetNode("/root/Node/UI/HUDHearts");
				var hitDetector = GetNode<HeroCombatCollision>("CombatCollision");
				hitDetector.Connect("HealthSetMax", heartHUD, "ResetHearts");
				hitDetector.Connect("HealthSet", heartHUD, "UpdateHearts");

				hitDetector.HealthMax = 30 + SaveData.Upgrades[0] * 10;
				hitDetector.Health = hitDetector.HealthMax;
				NodeWeapon.Damage[Elements.Phys] = 2 + SaveData.Upgrades[1];
				hitDetector.Defense[Elements.Fire] = 1 + SaveData.Upgrades[2] * 0.25f;
				hitDetector.Defense[Elements.Ice] = 1 + SaveData.Upgrades[2] * 0.25f;
			}
		}

		public override void _Ready()
		{
			InitializeNodeReferences();
			SwitchState(States.Immobile);
			SaveData.Screen = 1;
		}

		public override void _PhysicsProcess(float delta)
		{
			_controller.MoveBody(delta);
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
			if (@event.IsAction("pause"))
			{
				InputMove(0);
				InputJump(false);
				InputAttack(false);
			}
			if (@event.IsAction("ui_down"))
			{
				if (@event.GetActionStrength("ui_down") > 0)
					NodeCam.Position = new Vector2(0, 64);
					
				else
					NodeCam.Position = new Vector2(0, 0);
			}
		}

		public HeroState SwitchState(States state)
		{
			_controller?.ExitState();
			CurState = state;
			switch (state)
			{
				case States.Immobile:
					_controller = new HeroStateImmobile(this);
					break;
				case States.Ground:
					_controller = new HeroStateGround(this);
					break;
				case States.Air:
					_controller = new HeroStateAir(this);
					break;
				case States.Wall:
					_controller = new HeroStateWall(this);
					break;
				case States.LedgeGrab:
					_controller = new HeroStateLedgeGrab(this);
					break;
			}
			return _controller;
		}

		public void SwitchStateAuto()
		{
			_canAttack = true;
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
				_controller.InputMove(direction);
			InputMoveDirection = direction;
		}

		public void InputJump(bool pressed)
		{
			InputJumpStart = pressed ? OS.GetTicksMsec() : 0;
			_controller.InputJump(pressed);
		}

		public void InputAttack(bool pressed)
		{
			if (pressed)
				InputAttackStart = OS.GetTicksMsec();
			else
				return;

			if (NodeInteractableArea != null)
			{
				NodeInteractableArea.Interact(this);
				return;
			}

			if (_inStore || !_canAttack)
				return;

			_canAttack = false;
			NodeTimerAttack.Start();
			if (NodeRayEnemyDetector.IsColliding())
				NodeWeapon.Scale = new Vector2(Math.Sign(NodeRayEnemyDetector.GetCollisionPoint().x - GlobalPosition.x), 1);
			else if (InputMoveDirection == 0)
				NodeWeapon.Scale = new Vector2(NodeFlipH.Scale.x, 1);
			else
				NodeWeapon.Scale = new Vector2(InputMoveDirection, 1);
			_controller.InputAttack();
			NodeAnimWeapon.Stop();
			NodeAnimWeapon.Play("Swing");
		}

		public void AnimationAction(int action)
		{
			_controller.AnimationAction(action);
		}

		private void Hurt(CombatAttack attack)
		{
			var newVelocity = new Vector2(64, -96);
			_canAttack = false;
			if (attack.GlobalPosition < GlobalPosition)
				VelocityX = newVelocity.x;
			else
				VelocityX = -newVelocity.x;
			VelocityY = newVelocity.y;
			_controller = new HeroStateHurt(this);
		}

		private void CoyoteFall()
		{
			SwitchState(States.Air);
		}

		private void AttackReady()
		{
			_canAttack = true;
			if (InputAttackStart > OS.GetTicksMsec() - (NodeTimerAttack.WaitTime * 800))
				InputAttack(true);
		}

		private void HitEnemy(object area)
		{
			VelocityX *= 0.5f;
		}
		
		
		private void Defeated()
		{
			NodeAnim.Play("Defeat");
			SetProcessInput(false);
			NodeAnim.PauseMode = PauseModeEnum.Process;
			GetTree().Paused = true;

			GetTree().CallGroup("DestroyOnDefeat", "free");

			SaveData.Screen = 1;
			SaveData.CurCrawl++;
			SaveData.SaveGame();
		}
		
		private void EndCrawl()
		{
			GetTree().Paused = false;
			GetTree().ChangeScene("res://Scenes/Screens/Store.tscn");
		}
	}
}













