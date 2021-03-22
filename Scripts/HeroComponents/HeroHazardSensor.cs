using Godot;
using BrickAndMortal.Scripts.Combat;
using System;

class HeroHazardSensor : CombatActor
{
	private CollisionShape2D _nodeShape;
	private Timer _nodeTimerInvuln;
	
	public override void _Ready()
	{
		_nodeShape = GetNode<CollisionShape2D>("Shape");
		_nodeTimerInvuln = GetNode<Timer>("TimerInvuln");
	}
	
	public override void Hurt(CombatAttack attack)
	{
		base.Hurt(attack);
		_nodeShape.SetDeferred("disabled", true);
		_nodeTimerInvuln.Start();
	}
	
	private void InvulnEnd()
	{
		_nodeShape.Disabled = false;
	}
}







