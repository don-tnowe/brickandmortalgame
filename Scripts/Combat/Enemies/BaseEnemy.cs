using Godot;

namespace BrickAndMortal.Scripts.Combat.Enemies
{
	public class BaseEnemy : KinematicBody2D
	{
		[Export]
		public float PhysVelocityX = 0;
		[Export]
		public float PhysVelocityY = 0;
		[Export]
		public bool PhysicsEnabled = false;
		[Export]
		public int CurState = 0;
		[Export]
		public float PhysGravity = HeroParameters.GravityJump;
		[Export]
		private PackedScene _sceneDefeated;
		[Export]
		private PackedScene _sceneHit;
		
		[Signal]
		private delegate void Defeated();

		public bool AttackInvuln = false;

		public int PhysVelocityXFlip = 1;
		public int XFlip = 1;

		public Vector2 LastFramePhysVelocity;
		public int LastHitDir = 1;

		private Tween _nodeTween;
		private Sprite _nodeSprite;
		private CollisionShape2D _nodeShape;
		private AnimationPlayer _nodeAnim;

		protected System.Random _random;

		public override void _Ready()
		{
			_nodeTween = GetNode<Tween>("Tween");
			_nodeSprite = GetNode<Sprite>("Sprite");
			_nodeShape = GetNode<CollisionShape2D>("Shape");
			_nodeAnim = GetNode<AnimationPlayer>("Anim");
			
			_random = new System.Random(GetPositionInParent());
			Connect("Defeated", GetNode("../.."), "EnemyDefeated",
					new Godot.Collections.Array() { GetPositionInParent() }
				);

			XFlip = (int)Scale.x;
		}

		public override void _PhysicsProcess(float delta)
		{
			if (PhysicsEnabled)
				PhysMoveBody(delta);
		}

		public virtual void PhysMoveBody(float delta)
		{
			LastFramePhysVelocity = new Vector2(PhysVelocityX, PhysVelocityY);
			PhysVelocityY += PhysGravity * delta;
			var newVec = MoveAndSlide(new Vector2(PhysVelocityX * PhysVelocityXFlip, PhysVelocityY), Vector2.Up);
			PhysVelocityX = newVec.x * PhysVelocityXFlip;
			PhysVelocityY = newVec.y;
		}

		public void SetXFlipped(bool flipped)
		{
			if ((XFlip > 0) == flipped)
				ApplyScale(new Vector2(-1, 1));
			XFlip = flipped ? -1 : 1;
		}

		public void PlayAnim(string anim)
		{
			_nodeAnim.Play(anim);
		}

		public CombatAttack SpawnAtk(PackedScene fromScene, bool isGlobal = false, Vector2 dir = new Vector2())
		{
			var atk = (CombatAttack)fromScene.Instance();

			if (isGlobal)
			{
				GetParent().GetParent().AddChild(atk);
				atk.GlobalPosition = GlobalPosition;
				atk.Attacker = atk.GetPathTo(this);
			}
			else
			{
				AddChild(atk);
				atk.Attacker = "..";
			}

			atk.Launch(dir);

			return atk;
		}

		public void Hurt(CombatAttack byAttack)
		{
			LastHitDir = (GlobalPosition.x > byAttack.GlobalPosition.x) ? -1 : 1;
			if (!AttackInvuln)
			{
				var ptcl = (Particles2D)_sceneHit.Instance();
				GetParent().AddChild(ptcl);
				ptcl.GlobalPosition = GlobalPosition;
				ptcl.Scale = new Vector2(-LastHitDir, 1);
				_nodeTween.InterpolateProperty(
						_nodeSprite.Material, "shader_param/blend",
						0.5, 0, 0.25f,
						Tween.TransitionType.Quad, Tween.EaseType.Out
					);
				_nodeTween.Start();
			}
		}

		public void Vanquished()
		{
			EmitSignal(nameof(Defeated));

			var fx = (Node2D)_sceneDefeated.Instance();
			GetParent().CallDeferred("add_child",fx);
			fx.GlobalPosition = GlobalPosition;
			fx.Scale = new Vector2(-LastHitDir, 1);
			
			QueueFree();
		}
	}
}
