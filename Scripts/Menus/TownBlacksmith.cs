using System;
using Godot;

namespace BrickAndMortal.Scripts.Menus
{
	class TownBlacksmith : BaseMenu
	{
		[Export]
		private NodePath _pathMoneyDisplay;
		
		private Label _nodeCurName;
		private Label _nodeCurDesc;
		private Label _nodeUpgradePrice;
		private Label _nodeUpgradeLevel;
		private Tween _nodeTween;

		private int _curUpgradePrice;

		public override void _Ready()
		{
			_nodeCurName = GetNode<Label>("AnchorR/Header/Label");
			_nodeCurDesc = GetNode<Label>("AnchorR/Description/Label");
			_nodeUpgradePrice = GetNode<Label>("AnchorR/Price/Sprite/Label");
			_nodeUpgradeLevel = GetNode<Label>("AnchorR/Level");
			_nodeTween = GetNode<Tween>("/root/Node/UI/Tween");

			var nodeUpgrades = GetNode("AnchorL/Upgrades");
			for (int i = 0; i < nodeUpgrades.GetChildCount(); i++)
			{
				var node = nodeUpgrades.GetChild<Button>(i);
				
				node.GetNode<Label>("Level").Text = Tr("TownLevel") + " " + SaveData.Upgrades[i];

				node.FocusNeighbourLeft = ".";
				node.FocusNeighbourRight = ".";

				node.Connect("focus_entered", this, nameof(UpgradeFocused), new Godot.Collections.Array { node });
				node.Connect("mouse_entered", this, nameof(UpgradeFocused), new Godot.Collections.Array { node });
				node.Connect("pressed", this, nameof(UpgradeClicked), new Godot.Collections.Array { node });

				if (i == nodeUpgrades.GetChildCount() - 1)
				{
					var linkNode = nodeUpgrades.GetChild<Button>(0);
					linkNode.FocusPrevious = linkNode.GetPathTo(node);
					linkNode.FocusNeighbourTop = linkNode.GetPathTo(node);
					node.FocusNext = node.GetPathTo(linkNode);
					node.FocusNeighbourBottom = node.GetPathTo(linkNode);
				}
			}
		}
		
		public override void OpenMenu()
		{
			GetNode<Control>("AnchorL/Upgrades/Item0").GrabFocus();
		}
		
		public void UpgradeFocused(Button node)
		{
			node.GrabFocus();

			var upgradeIdx = node.GetPositionInParent();
			var upgradeLvl = SaveData.Upgrades[upgradeIdx];

			switch (upgradeIdx)
			{
				case 0:
					_curUpgradePrice = 200 + upgradeLvl * upgradeLvl * 150;
					break;
				case 1:
					_curUpgradePrice = 500 + upgradeLvl * 750;
					break;
				case 2:
					_curUpgradePrice = 1300 + upgradeLvl * upgradeLvl * upgradeLvl * 123;
					break;
			}

			_nodeCurName.Text = "UpgradeName" + upgradeIdx;
			_nodeCurDesc.Text = Tr("UpgradeDesc" + upgradeIdx).CUnescape();
			_nodeUpgradePrice.Text = _curUpgradePrice.ToString();
			_nodeUpgradeLevel.Text = Tr("TownLevel") + " " + upgradeLvl;
		}

		public void UpgradeClicked(Button node)
		{
			if (SaveData.Money >= _curUpgradePrice)
			{
				SaveData.Money -= _curUpgradePrice;
				SaveData.Upgrades[node.GetPositionInParent()] += 1;
				SaveData.SaveGame();
				node.GetNode<Label>("Level").Text = Tr("TownLevel") + " " + SaveData.Upgrades[node.GetPositionInParent()];
				GetNode(_pathMoneyDisplay)._Ready();
				
				UpgradeFocused(node);
			}
		}
	}
}
