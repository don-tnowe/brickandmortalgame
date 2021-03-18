using BrickAndMortal.Scripts.HeroStates;
using Godot;
using System;

public class Hero : KinematicBody2D
{
	public enum States
	{
		Immobile,
		Ground,
		Air,
		Wall,
		LedgeGrab,
		AttackGround,
		Hurt
	}
	
	[Export] 
	public bool IsInDungeon = true;
	
	public float VelocityX = 0;
	public float VelocityY = 0;
	public Vector2 LastVelocity = new Vector2();
	public int VelocityXSign = 1;
	public States CurState;

	public float InputMoveDirection = 0;
	public uint InputJumpStart = 0;
	public uint InputAttackStart = 0;

	private BrickAndMortal.Scripts.Combat.CombatActor _combat;
	private HeroState _state;

	public CollisionShape2D NodeShape;
	public Node2D NodeFlipH;
	public Sprite NodeSprite;
	public RayCast2D NodeRayGround;
	public RayCast2D NodeRayLedgeGrab;
	public RayCast2D NodeRayLedgeGrabV;
	public RayCast2D NodeRayEnemyDetector;
	public Camera2D NodeCam;
	public Area2D NodeWeapon;
	public AnimationPlayer NodeAnim;
	public AnimationPlayer NodeAnimWeapon;
	public Timer NodeTimerCoyote;
	public Timer NodeTimerAttack;

	private void InitializeNodeReferences()
	{
		NodeShape = GetNode<CollisionShape2D>("Shape");
		NodeFlipH = GetNode<Node2D>("FlipH");
		NodeSprite = GetNode<Sprite>("FlipH/Sprite");
		NodeRayGround = GetNode<RayCast2D>("FlipH/RayGround");
		NodeRayLedgeGrab = GetNode<RayCast2D>("FlipH/RayLedgeGrab");
		NodeRayLedgeGrabV = GetNode<RayCast2D>("FlipH/RayLedgeGrabV");
		NodeRayEnemyDetector = GetNode<RayCast2D>("FlipH/RayEnemyDetector");
		NodeCam = GetNode<Camera2D>("Cam");
		NodeWeapon = GetNode<Area2D>("Weapon");
		NodeAnim = GetNode<AnimationPlayer>("Anim");
		NodeAnimWeapon = GetNode<AnimationPlayer>("Weapon/AnimWeapon");
		NodeTimerCoyote = GetNode<Timer>("TimerCoyote");
		NodeTimerAttack = GetNode<Timer>("TimerAttack");
	}
	public override void _Ready()
	{
		InitializeNodeReferences();
		SwitchState(States.Air);
	}

	public override void _Process(float delta)
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
			InputJump(@event.IsPressed());
		}
		if (@event.IsAction("attack"))
		{
			InputAttack(@event.IsPressed());
		}
	}
	
	public HeroState SwitchState(States state)
	{
		_state?.ExitState();
		CurState = state;
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
			case States.AttackGround:
				_state = new HeroStateAttackGround(this);
				break;
		}
		return _state;
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
		InputAttackStart = pressed ? OS.GetTicksMsec() : 0;
		if (pressed && NodeTimerAttack.TimeLeft == 0)
		{
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

	public void Hurt()
	{
		_state = new HeroStateHurt(this);
	}
	
	private void CoyoteFall()
	{
		SwitchState(States.Air);
	}
	
	private void AttackReady()
	{
		if (InputAttackStart > OS.GetTicksMsec() - (NodeTimerAttack.WaitTime * 800))
			InputAttack(true);
	}
}





