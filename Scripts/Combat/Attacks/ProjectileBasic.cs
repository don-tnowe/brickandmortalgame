using Godot;

namespace BrickAndMortal.Scripts.Combat.Attacks
{
    class ProjectileBasic : CombatAttack
    {
        [Export]
        public Vector2 Velocity = new Vector2(0, 0);
        [Export]
        public Vector2 Acceleration = new Vector2(0, 0);
        [Export]
        public bool IsPiercing = true;
        [Export]
        public bool IsSpectral = false;
        [Export]
        public int BounceCount = 0;
        [Export]
        public PackedScene SpawnOnDestroy = null;
        [Export]
        private NodePath _pathRayWall = "RayWall";

        private RayCast2D _nodeRayWall;

        public override void _Ready()
        {
            if (HasNode(_pathRayWall))
                _nodeRayWall = GetNode<RayCast2D>(_pathRayWall);
        }

        public override void _PhysicsProcess(float delta)
        {
            Velocity += Acceleration * delta;
            Translate(Velocity * delta);
            if (_nodeRayWall != null)
                DetectWallCollision(delta);
        }

        private void DetectWallCollision(float delta)
        {
            _nodeRayWall.CastTo = Velocity * delta;
            if (_nodeRayWall.IsColliding())
            {
                if (BounceCount > 0)
                {
                    Velocity = Velocity.Bounce(_nodeRayWall.GetCollisionNormal());
                    BounceCount--;
                }
                else if (!IsSpectral)
                    Destroy();
            }
        }

        public override void HitTarget(CombatActor target) 
        {
            if (!IsPiercing)
                Destroy();
        }

        public void Destroy()
        {
            if (SpawnOnDestroy != null)
            {
                var scene = (Node2D)SpawnOnDestroy.Instance();
                GetParent().AddChild(scene);
                scene.GlobalPosition = GlobalPosition;
            }
            QueueFree();
        }
    }
}
