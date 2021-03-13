
namespace BrickAndMortal.Scripts
{
    static class HeroParameters
    {
        public const float MaxSpeed = 128;
        public const float AccelGround = MaxSpeed * 3.0f;
        public const float AccelAir = MaxSpeed * 1.5f;
        public const float Brake = MaxSpeed * 0.5f;

        public const float GravityJump = 256;
        public const float GravityFall = GravityJump * 2.0f;
        public const float Jump = -GravityJump * 0.6f;
        public const float JumpInterrupted = -GravityJump * 0.15f;
        public const float MaxFall = GravityJump * 2.0f;

        public const float JumpWall = -GravityJump * 0.6f;
        public const float MaxFallWall = GravityJump * 0.25f;
        public const float JumpWallHorizontal = MaxSpeed * 0.75f;

        public const float BrakeInstantMult = 0.33f;
        public const float WallFrictionInstantMult = 0.2f;

        public const uint MsecJumpBuffer = 300;
    }
}
