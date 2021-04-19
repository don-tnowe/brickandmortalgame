using Godot;
using BrickAndMortal.Scripts.Combat;

namespace BrickAndMortal.Scripts.Enemies
{
	public class BaseEnemy : KinematicBody2D
	{
		[Export]
		private NodePath _nodePathRoom = "../..";
		[Export]
		public float PhysVelocityX = 0;
		[Export]
		public float PhysVelocityY = 0;
		[Export]
		public bool PhysicsEnabled = false;
		[Export]
		public int CurState = 0;

		[Signal]
		private delegate void Defeated(int idx);

		public readonly PackedScene ScenePtclHit = ResourceLoader.Load<PackedScene>("res://Scenes/VFX/PtclHit.tscn");
        private Tween _nodeTween;
		private Sprite _nodeSprite;
		private CollisionShape2D _nodeShape;
		private AnimationPlayer _nodeAnim;

		public bool AttackInvuln = false;

		public float PhysGravity = BrickAndMortal.Scripts.HeroParameters.GravityJump;
		public int PhysVelocityXFlip = 1;
		public int LastHitDir = 1;

		public override void _Ready()
		{
			_nodeTween = GetNode<Tween>("Tween");
			_nodeSprite = GetNode<Sprite>("Sprite");
			_nodeShape = GetNode<CollisionShape2D>("Shape");
			_nodeAnim = GetNode<AnimationPlayer>("Anim");
			Connect("Defeated", GetNode(_nodePathRoom), "EnemyDefeated",
					new Godot.Collections.Array() { GetPositionInParent() }
				);
		}

		public override void _PhysicsProcess(float delta)
		{
			if (PhysicsEnabled)
				PhysMoveBody(delta);
		}

		public virtual void PhysMoveBody(float delta)
		{
			PhysVelocityY += PhysGravity * delta;
			var newVec = MoveAndSlide(new Vector2(PhysVelocityX * PhysVelocityXFlip, PhysVelocityY), Vector2.Up);
			PhysVelocityX = newVec.x * PhysVelocityXFlip;
			PhysVelocityY = newVec.y;
		}

		public void PlayAnim(string anim)
		{
			_nodeAnim.Play(anim);
		}

		public CombatAttack SpawnAtk(PackedScene fromScene, bool isGlobal = false)
		{
			var atk = (CombatAttack)fromScene.Instance();
			if (isGlobal)
			{
				GetParent().GetParent().AddChild(atk);
				atk.GlobalPosition = GlobalPosition;
			}
			else
				AddChild(atk);
			return atk;
		}

		public void Hurt(CombatAttack byAttack)
		{
			LastHitDir = (GlobalPosition.x > byAttack.GlobalPosition.x) ? -1 : 1;
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
				ptcl.Scale = new Vector2(-LastHitDir, 1);
			}
		}

		public void Vanquished()
		{
			CurState = -1;
			EmitSignal(nameof(Defeated));

			PhysicsEnabled = true;
			PhysVelocityY = -PhysGravity * 0.25f;
			PhysVelocityX = -LastHitDir * PhysGravity * 0.15f;
			Modulate = new Color(0.2f, 0.2f, 0.2f, 1);
			_nodeShape.SetDeferred("disabled", true);
			_nodeTween.StopAll();
			_nodeTween.InterpolateCallback(
					this, 1, "queue_free"
				);
			_nodeTween.Start();
			_nodeAnim.Play(nameof(Defeated));
		}
	}
}