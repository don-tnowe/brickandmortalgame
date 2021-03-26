using System;
using Godot;

namespace BrickAndMortal.Scripts.ItemOperations
{
	class ItemDungeon : Sprite
	{
		public bool CanLook = true;
		public bool CanPickup = false;

		public override void _Ready()
		{
			GetNode<Sprite>("CaseFront").Frame = Frame;
			GetNode<Sprite>("CaseFront/CaseBack").Frame = Frame;
		}

		public void UnlockItem()
		{
			GetNode<AnimationPlayer>("Anim").Play("Open");
		}
	}
}


