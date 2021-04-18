using System;
using Godot;

namespace BrickAndMortal.Scripts.ItemOperations
{
	class ItemStatDisplay : VBoxContainer
	{
		private const int _maxDisplayedEnch = 4;

		private Label _nodeName;
		private Label _nodeStatPower;
		private Label _nodeStatShine;
		private Label _nodeStatMagic;
		private Label _nodePrompt;
		private Tween _nodeTween;

		private CanvasItem[] _nodesHideWhenNoItem;
		private CanvasItem[] _nodesEnch;

		public override void _Ready()
		{
			_nodeName = GetNode<Label>("Header/Label");
			_nodeStatPower = GetNode<Label>("Content/Box/Box/BoxP/Text");
			_nodeStatShine = GetNode<Label>("Content/Box/Box/BoxS/Text");
			_nodeStatMagic = GetNode<Label>("Content/Box/Box/BoxM/Text");
			_nodePrompt = GetNode<Label>("Prompt/Prompt/Label");
			_nodeTween = GetNode<Tween>("Tween");
			
			_nodesHideWhenNoItem = new CanvasItem[] { 
				GetNode<CanvasItem>("Header"), GetNode<CanvasItem>("Content") 
			};

			var nodeEnchs = GetNode("Content/Box/BG/MarginContainer/Enchants");
			_nodesEnch = new CanvasItem[nodeEnchs.GetChildCount()];
			for (int i = 0; i < _nodesEnch.Length; ++i)
				_nodesEnch[i] = nodeEnchs.GetChild<CanvasItem>(i);
		}

		public void DisplayItemData(string prompt, Item item = null)
		{
			DisplayItemData(item);
			_nodePrompt.Text = prompt;
			_nodePrompt.RectSize = Vector2.Zero;
			_nodePrompt.Visible = true;
		}
		
		public void DisplayItemData(Item item)
		{
			Visible = true;
			_nodePrompt.Visible = false;

			foreach(CanvasItem i in _nodesHideWhenNoItem)
				i.Visible = item != null;
			
			if (item == null)
				return;
			
			_nodeName.Text = "ItemType" + item.ItemType;
			_nodeStatPower.Text = item.Power.ToString();
			_nodeStatShine.Text = item.Shine.ToString();
			_nodeStatMagic.Text = item.Magic.ToString();

			for (int i = 0; i < _maxDisplayedEnch; ++i)
			{
				if (item.HeldEnchantments[1].Length <= i || item.HeldEnchantments[1][i] == 0)
				{
					_nodesEnch[i].Visible = false;
					continue;
				}
				_nodesEnch[i].Visible = true;
				_nodesEnch[i].GetChild<Sprite>(0).Frame = item.HeldEnchantments[0][i];
				_nodesEnch[i].GetChild<Label>(1).Text = item.HeldEnchantments[1][i].ToString();
			}
		}

		public void ShowPopup()
		{
			_nodeTween.Stop(this);
			_nodeTween.InterpolateProperty(this, "rect_position",
				RectPosition, new Vector2(-33, -87),
				0.5f, Tween.TransitionType.Quad, Tween.EaseType.Out
				);
			_nodeTween.InterpolateProperty(this, "modulate",
				new Color(0, 0, 0, 0), new Color(1, 1, 1, 1),
				0.5f
				);

			_nodeTween.Start();
		}

		public void HidePopup()
		{
			_nodeTween.Stop(this);
			_nodeTween.InterpolateProperty(this, "rect_position",
				RectPosition, new Vector2(-33, -75),
				0.5f, Tween.TransitionType.Quad, Tween.EaseType.In
				);
			_nodeTween.InterpolateProperty(this, "modulate",
				Modulate, new Color(0, 0, 0, 0),
				0.5f
				);

			_nodeTween.Start();
		}

	}
}
