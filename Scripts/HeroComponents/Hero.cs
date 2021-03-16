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

	public float VelocityX = 0;
	public float VelocityY = 0;
	public Vector2 LastVelocity = new Vector2();
	public int VelocityXSign = 1;
	public States CurState;

	public float InputMoveDirection = 0;
	public uint InputJumpStart = 0;
	public uint InputAttackStart = 0;

	private HeroState _state;

	public CollisionShape2D NodeShape;
	public Node2D NodeFlipH;
	public Sprite NodeSprite;
	public RayCast2D NodeRayGround;
	public RayCast2D NodeRayLedgeGrab;
	public RayCast2D NodeRayLedgeGrabV;
	public Camera2D NodeCam;
	public AnimationPlayer NodeAnim;
	public Timer NodeTimerCoyote;

	private void InitializeNodeReferences()
	{
		NodeShape = GetNode<CollisionShape2D>("Shape");
		NodeFlipH = GetNode<Node2D>("FlipH");
		NodeSprite = GetNode<Sprite>("FlipH/Sprite");
		NodeRayGround = GetNode<RayCast2D>("FlipH/RayGround");
		NodeRayLedgeGrab = GetNode<RayCast2D>("FlipH/RayLedgeGrab");
		NodeRayLedgeGrabV = GetNode<RayCast2D>("FlipH/RayLedgeGrabV");
		NodeCam = GetNode<Camera2D>("Cam");
		NodeAnim = GetNode<AnimationPlayer>("Anim");
		NodeTimerCoyote = GetNode<Timer>("TimerCoyote");
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
		_state.InputMove(direction);
		InputMoveDirection = direction;
	}

	public void InputJump(bool pressed)
	{
		_state.InputJump(pressed);
		InputJumpStart = pressed ? OS.GetTicksMsec() : 0;
	}

	public void InputAttack(bool pressed)
	{
		if (pressed)
			_state.InputAttack();
		InputAttackStart = pressed ? OS.GetTicksMsec() : 0;
	}

	public void Hurt()
	{
		_state = new HeroStateHurt(this);
	}
	
	private void CoyoteFall()
	{
		SwitchState(States.Air);
	}
}



