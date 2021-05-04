using Godot;

namespace BrickAndMortal.Scripts.StoreFeatures
{
	class MenuSell : Control
	{
		private StoreManager _nodeStoreManager;
		private Label _nodePriceLabel;
		private Label _nodeOpinionLabel;
		private VFX.Oscillator2D _nodeOpinionOsc;
		private ItemOperations.ItemStatDisplay _nodeItemStats;
		private AnimationPlayer _nodeAnim;

		private System.Random _random = new System.Random();

		public override void _Ready()
		{
			_nodeStoreManager = GetNode<StoreManager>("../..");
			_nodePriceLabel = GetNode<Label>("AnchorL/Panel/Price/Sprite/Label");
			_nodeOpinionLabel = GetNode<Label>("AnchorL/Panel/Label");
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
			_nodeOpinionOsc.GetNode<Sprite>("Sprite").Frame = 0;
			_nodeOpinionLabel.Text = "StoreDialogueRequest" + _nodeStoreManager.GetOpinionPersonality();
		}

		private void ButtonFocused(int idx)
		{
			_nodeAnim.Advance(60);
			switch (idx)
			{
				case 0:
					_nodeAnim.Stop();
					_nodeAnim.Play("RaisePrice");
					_nodeStoreManager.SuperRaisePrice();
					UpdateInfo();
					break;
				case 1:
					_nodeAnim.Stop();
					_nodeAnim.Play("RaisePrice");
					_nodeStoreManager.RaisePrice();
					UpdateInfo();
					break;
				case 2:
					_nodeStoreManager.DenyByPlayer();
					CloseMenu();
					break;
				case 3:
					_nodeStoreManager.AcceptSale();
					CloseMenu();
					break;
			}
			GrabFocus();
		}

		public void CloseMenu()
		{
			ReleaseFocus();
			_nodeAnim.Play("Close");
		}

		private void UpdateInfo()
		{
			int price = _nodeStoreManager.GetNegotiationPrice();
			int opinion = _nodeStoreManager.GetPriceOpinion();

			_nodePriceLabel.Text = price.ToString();
			_nodeOpinionOsc.GetNode<Sprite>("Sprite").Frame = opinion;
			_nodeOpinionOsc.Magnitude = opinion <= 6 ? 0 : opinion / 2 - 3;
			
			var opinionText = "";
			
			if (opinion >= 12)
				opinionText = "Worried";
				
			else if (opinion >= 8)
				opinionText = "Angry";
				
			else if (opinion >= 4)
				opinionText = "High";
			
			_nodeOpinionLabel.Text = "StoreDialogueRequest" + opinionText + _nodeStoreManager.GetOpinionPersonality();
		}
	}
}
