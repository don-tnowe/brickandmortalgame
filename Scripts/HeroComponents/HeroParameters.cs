
namespace BrickAndMortal.Scripts
{
	static class HeroParameters
	{
		public const float AccelGround = 384;
		public const float AccelAir = AccelGround * 0.52f;
		public const float Brake = AccelGround * 0.16f;
		public const float MaxSpeed = AccelGround * 0.33f;

		public const float GravityJump = 256;
		public const float GravityFall = GravityJump * 2.0f;
		public const float Jump = -GravityJump * 0.6f;
		public const float JumpInterrupted = -GravityJump * 0.15f;
		public const float MaxFall = GravityJump * 2.0f;

		public const float JumpWall = -GravityJump * 0.6f;
		public const float MaxFallWall = GravityJump * 0.25f;
		public const float JumpWallHorizontal = AccelGround * 0.25f;
		public const float InstaTurnSpeed = AccelGround * 0.25f;

		public const float BrakeInstantMult = 0.33f;
		public const float WallFrictionInstantMult = 0.2f;

		public const uint MsecJumpBuffer = 300;
		public const uint MsecInstaTurn = 200;
	}
}
