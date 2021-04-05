using System;
using Godot;

namespace BrickAndMortal.Scripts.ItemOperations
{
	class ItemStatDisplay : VBoxContainer
	{
		private Label _nodeName; 
		private Label _nodeStatPower;
		private Label _nodeStatShine;
		private Label _nodeStatMagic;
		
		private CanvasItem[] _nodesEnch;
		
		public override void _Ready() 
		{
			_nodeName = GetNode<Label>("Header/Label");
			_nodeStatPower = GetNode<Label>("Content/Box/Box/BoxP/Text");
			_nodeStatShine = GetNode<Label>("Content/Box/Box/BoxS/Text");
			_nodeStatMagic = GetNode<Label>("Content/Box/Box/BoxM/Text");
			var nodeEnchs = GetNode("Content/Box/BG/MarginContainer/Enchants");
			_nodesEnch = new CanvasItem[nodeEnchs.GetChildCount()];
			for (int i = 0; i < _nodesEnch.Length; ++i)
				_nodesEnch[i] = nodeEnchs.GetChild<CanvasItem>(i);
		}
		
		public void DisplayItemData(Item item)
		{
			Visible = true;
			_nodeName.Text = "ItemType" + item.ItemType;
			_nodeStatPower.Text = item.Power.ToString();
			_nodeStatShine.Text = item.Shine.ToString();
			_nodeStatMagic.Text = item.Magic.ToString();

			for(int i = 0; i < item.HeldEnchantments.GetLength(0); ++i)
			{
				if (item.HeldEnchantments[i, 1] > 0)
				{
					_nodesEnch[i].Visible = true;
					_nodesEnch[i].GetChild<Sprite>(0).Frame = item.HeldEnchantments[i, 0];
					_nodesEnch[i].GetChild<Label>(1).Text = item.HeldEnchantments[i, 1].ToString();
				}
				else
					_nodesEnch[i].Visible = false;
			}
		}
	}
}
