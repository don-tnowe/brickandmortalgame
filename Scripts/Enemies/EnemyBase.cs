using Godot;
using BrickAndMortal.Scripts.Combat;

public class EnemyBase : KinematicBody2D
{
	[Export]
	private readonly PackedScene ScenePtclHit;

	[Signal]
	private delegate void Defeated();
	
	private Tween _nodeTween;
	private Sprite _nodeSprite;
	private CollisionShape2D _nodeShape;

	public int CurState = 0;
	public bool AttackInvuln = false;

	public bool PhysicsEnabled = false;
	public float PhysVelocityX = 0;
	public float PhysVelocityY = 0;
	public float PhysGravity = BrickAndMortal.Scripts.HeroParameters.GravityJump; 
	private int _lastHitDir = 1;

	public override void _Ready()
	{
		_nodeTween = GetNode<Tween>("Tween");
		_nodeSprite = GetNode<Sprite>("Sprite");
		_nodeShape = GetNode<CollisionShape2D>("Shape");
	}

	public override void _Process(float delta)
	{
		if (PhysicsEnabled)
			PhysMoveBody(delta);
	}

	public virtual void PhysMoveBody(float delta)
	{
		PhysVelocityY += PhysGravity * delta;
		var newVec = MoveAndSlide(new Vector2(PhysVelocityX, PhysVelocityY));
		PhysVelocityX = newVec.x;
		PhysVelocityY = newVec.y;
	}

	private void Hurt(CombatAttack byAttack)
	{
		_lastHitDir = (GlobalPosition.x > byAttack.GlobalPosition.x) ? -1 : 1;
		if (!AttackInvuln)
		{
			var ptcl = (Particles2D)ScenePtclHit.Instance();
			GetParent().AddChild(ptcl);
			_nodeTween.InterpolateProperty(
					_nodeSprite.Material, "shader_param/blend",
					0.5, 0, 0.25f,
					Tween.TransitionType.Quad, Tween.EaseType.Out
				);
			_nodeTween.Start();
			ptcl.GlobalPosition = GlobalPosition;
			ptcl.Scale = new Vector2(_lastHitDir, 1);
		}
	}

	public void Vanquished()
	{
		CurState = -1;
		EmitSignal("Defeated");

		PhysicsEnabled = true;
		PhysVelocityY = -PhysGravity * 0.25f;
		PhysVelocityX = -_lastHitDir * PhysGravity * 0.15f;
		Modulate = new Color(0.2f, 0.2f, 0.2f, 0.8f);
		_nodeShape.SetDeferred("disabled", true);
		_nodeTween.StopAll();
		_nodeTween.InterpolateCallback(
				this, 1, "queue_free"
			);
		_nodeTween.Start();
	}
}



