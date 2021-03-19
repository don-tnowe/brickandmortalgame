using Godot;
using System;

public class HazardSensor : Area2D
{
	private CollisionShape2D _nodeShape;
	private Timer _nodeTimerInvuln;
	
	public override void _Ready()
	{
		_nodeShape = GetNode<CollisionShape2D>("Shape");
		_nodeTimerInvuln = GetNode<Timer>("TimerInvuln");
	}
	
	private void HitByHazard(Area2D hazard)
	{
		_nodeShape.SetDeferred("disabled", true);
		_nodeTimerInvuln.Start();
	}
	
	private void InvulnEnd()
	{
		_nodeShape.Disabled = false;
	}
}





