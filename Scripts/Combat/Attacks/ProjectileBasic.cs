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
        private bool _isPiercing = true;
        [Export]
        private bool _isSpectral = false;
        [Export]
        private int _bounceCount = 0;
        [Export]
        private PackedScene _spawnOnDestroy = null;
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
                if (_bounceCount > 0)
                {
                    Velocity = Velocity.Bounce(_nodeRayWall.GetCollisionNormal());
                    _bounceCount--;
                }
                else if (!_isSpectral)
                    Destroy();
            }
        }

        public override void HitTarget(CombatActor target) 
        {
            if (!_isPiercing)
                Destroy();
        }

        public void Destroy()
        {
            if (_spawnOnDestroy != null)
            {
                var scene = (Node2D)_spawnOnDestroy.Instance();
                GetParent().AddChild(scene);
                scene.GlobalPosition = GlobalPosition;
            }
            QueueFree();
        }
    }
}
