using BrickAndMortal.Scripts.Combat;
using Godot;
using System;

public class HazardBase : Area2D
{
	public CombatAttack attack;

	[Export]
	public int[] Damage = new int[] { 5, 0, 0, 0, 0 };

	public override void _Ready()
	{
		attack = new CombatAttack()
		{
			Damage = Damage
		};
	}
}
