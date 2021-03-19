using Godot;
using BrickAndMortal.Scripts.Combat;

public class EnemyBase : KinematicBody2D
{
	[Export]
	private readonly int _hitpoints = 12;
	[Export]
	private readonly float[] _attack = new float[5] { 0, 0, 0, 0, 0 };
	[Export]
	private readonly float[] _defense = new float[5] { 1, 1, 1, 1, 1 };
	[Export]
	private readonly PackedScene ScenePtclHit;

	private Tween _nodeTween;
	private Sprite _nodeSprite;
	private CollisionShape2D _nodeShape;

	private CombatActor _combat;

	public int CurState = 0;
	public bool AttackInvuln = false;

	public bool PhysicsEnabled = false;
	public float PhysVelocityX = 0;
	public float PhysVelocityY = 0;
	public float PhysGravity = BrickAndMortal.Scripts.HeroParameters.GravityJump; 

	public override void _Ready()
	{
		_combat = new CombatActor(_hitpoints, _attack, _defense);
		_nodeTween = GetNode<Tween>("Tween");
		_nodeSprite = GetNode<Sprite>("Sprite");
		_nodeShape = GetNode<CollisionShape2D>("Shape");
		_combat.Connect("Defeated", this, "Defeated");
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

	public void Hurt(HeroAttack byHeroAttack)
	{
		if (!AttackInvuln)
		{
			_combat.Hurt(byHeroAttack.Attack);

			var ptcl = (Particles2D)ScenePtclHit.Instance();
			GetParent().AddChild(ptcl);
			_nodeTween.InterpolateProperty(
					_nodeSprite.Material, "shader_param/blend",
					0.5, 0, 0.25f,
					Tween.TransitionType.Quad, Tween.EaseType.Out
				);
			_nodeTween.Start();
			ptcl.GlobalPosition = GlobalPosition;
			if (GlobalPosition.x < byHeroAttack.GlobalPosition.x)
				ptcl.Scale = new Vector2(-1, 1);
		}
	}

	public void Defeated()
	{
		CurState = -1;
		PhysicsEnabled = true;
		PhysVelocityY = -PhysGravity * 0.25f;
		Modulate = new Color(0, 0, 0, 0.8f);
		_nodeShape.SetDeferred("disabled", true);
		_nodeTween.StopAll();
		_nodeTween.InterpolateCallback(
				this, 1, "queue_free"
			);
		_nodeTween.Start();
	}
/*
	new private void QueueFree()
	{
		base.QueueFree();
	}*/
}
