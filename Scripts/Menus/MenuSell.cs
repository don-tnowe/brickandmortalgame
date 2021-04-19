using BrickAndMortal.Scripts.StoreFeatures;
using Godot;

namespace BrickAndMortal.Scripts.Menus
{
	class MenuSell : Control
	{
		private StoreManager _nodeStoreManager;
		private Label _nodePriceLabel;
		private VFX.Oscillator2D _nodeOpinionOsc;
		private ItemOperations.ItemStatDisplay _nodeItemStats;
		private AnimationPlayer _nodeAnim;

		private System.Random _random = new System.Random();

		public override void _Ready()
		{
			_nodeStoreManager = GetNode<StoreManager>("../..");
			_nodePriceLabel = GetNode<Label>("AnchorL/Panel/Price/Sprite/Label");
			_nodeOpinionOsc = GetNode<VFX.Oscillator2D>("AnchorL/Panel/Opinion");
			_nodeItemStats = GetNode<ItemOperations.ItemStatDisplay>("AnchorR/ItemStatDisplay");
			_nodeAnim = GetNode<AnimationPlayer>("Anim");

			foreach (Node i in GetNode("Buttons").GetChildren())
				i.Connect("focus_entered", this, nameof(ButtonFocused), new Godot.Collections.Array() { i.GetPositionInParent() });
		}

		public void OpenMenu(ItemOperations.Item item)
		{
			_nodeItemStats.DisplayItemData(item);
			_nodeAnim.Stop();
			_nodeAnim.Play("Open");
			UpdateInfo();
			CallDeferred("grab_focus");
			_nodeOpinionOsc.GetNode<Sprite>("Sprite").Frame = 0;
		}

		private void ButtonFocused(int idx)
		{
			switch (idx)
			{
				case 0:
					UpdateInfo();
					_nodeAnim.Stop();
					_nodeAnim.Play("RaisePrice");
					_nodeStoreManager.SuperRaisePrice();
					break;
				case 1:
					UpdateInfo();
					_nodeAnim.Stop();
					_nodeAnim.Play("RaisePrice");
					_nodeStoreManager.RaisePrice();
					break;
				case 2:
					_nodeStoreManager.Deny();
					CloseMenu();
					break;
				case 3:
					_nodeStoreManager.Accept();
					CloseMenu();
					break;
			}
			GrabFocus();
		}

		public void CloseMenu()
		{
			GrabFocus();
			ReleaseFocus();
			_nodeAnim.Play("Close");
		}

		private void UpdateInfo()
		{
			int price = _nodeStoreManager.GetNegotiationPrice();
			int opinion = _nodeStoreManager.GetPriceOpinion();

			_nodePriceLabel.Text = price.ToString();
			_nodeOpinionOsc.GetNode<Sprite>("Sprite").Frame = opinion;
			_nodeOpinionOsc.Magnitude = opinion <= 2 ? 0 : opinion - 2;
		}
	}
}
