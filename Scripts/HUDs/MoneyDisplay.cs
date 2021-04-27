using Godot;
using System;

namespace BrickAndMortal.Scripts.HUDs
{
	public class MoneyDisplay : Label
	{
		public override void _Ready()
		{
			Text = SaveData.Money.ToString();
		}
	}
}
