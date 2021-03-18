using Godot;
using System;

public class ParticlesBase : Particles2D
{
	public override void _Ready()
	{
		Emitting = true;
	}

  public override void _Process(float delta)
  {
	  if (!Emitting)
		QueueFree();
  }
}
