using System;
using Godot;

namespace BrickAndMortal.Scripts.ItemOperations
{
	class ItemStatDisplay : VBoxContainer
	{
		public void DisplayItemData(Item item)
		{
			Visible = true;
			GetNode<Label>("Header/Label").Text = "ItemType" + item.ItemType;
			GetNode<Label>("Content/Box/Box/BoxP/Text").Text = item.Power.ToString();
			GetNode<Label>("Content/Box/Box/BoxS/Text").Text = item.Shine.ToString();
			GetNode<Label>("Content/Box/Box/BoxM/Text").Text = item.Magic.ToString();

			for(int i = 0; i < item.HeldEnchantments.GetLength(0); ++i)
			{
				if (item.HeldEnchantments[i, 1] > 0)
				{
					GetNode<CanvasItem>("Content/Box/BG/MarginContainer/Enchants/Ench" + i).Visible = true;
					GetNode<Sprite>("Content/Box/BG/MarginContainer/Enchants/Ench" + i + "/Icon").Frame = item.HeldEnchantments[i, 0];
					GetNode<Label>("Content/Box/BG/MarginContainer/Enchants/Ench" + i + "/Text").Text = item.HeldEnchantments[i, 1].ToString();
				}
				else
					GetNode<CanvasItem>("Content/Box/BG/MarginContainer/Enchants/Ench" + i).Visible = false;
			}
		}
	}
}
