using Godot;
using System;

public class HeroInputHandler : Node
{
	private Hero _hero;

	public override void _Ready()
	{
		_hero = GetParent<Hero>();
	}

	public override void _Input(InputEvent @event) {
		if (@event is InputEventMouse)
			return;
		if (@event is InputEventKey && @event.IsEcho())
			return;
		if (@event.IsAction("ui_left") || @event.IsAction("ui_right"))
		{
			var direction = Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left");
			_hero.InputMove(direction);
		}
		if (@event.IsAction("jump"))
        {
			_hero.InputJump(@event.IsPressed());
		}
		if (@event.IsAction("attack"))
		{
			_hero.InputAttack(@event.IsPressed());
		}
	}
}
