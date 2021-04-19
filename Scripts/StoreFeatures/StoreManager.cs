using BrickAndMortal.Scripts.Menus;
using System.Collections.Generic;
using Godot;

namespace BrickAndMortal.Scripts.StoreFeatures
{
	class StoreManager : Node
	{
		[Export]
		private PackedScene _sceneMenuItemChoose;
		[Export]
		private PackedScene _sceneCustomer;

		public bool[] ItemsOnShelves;

		public CustomerInStore CurrentCustomer { get; private set; }

		private int _customersLeft = 0;
		private ItemShelf _nodeNegotiatedShelf;

		private HeroComponents.HeroStore _nodeHero;
		private Node _nodeShelves;
		private Node _nodeCustomers;
		private Control _nodeCustomerRequestBubble;
		private MenuSell _nodeMenuSell;
		private Tween _nodeTween;

		private System.Random _random = new System.Random();

		public override void _Ready()
		{
			SaveData.LoadGame();

			_nodeHero = GetNode<HeroComponents.HeroStore>("Hero");
			_nodeShelves = GetNode("Room/Shelves");
			_nodeCustomers = GetNode("Customers");
			_nodeCustomerRequestBubble = GetNode<Control>("ZAbove/CustomerRequest");
			_nodeMenuSell = GetNode<MenuSell>("UI/MenuSell");
			_nodeTween = GetNode<Tween>("UI/Tween");

			ItemsOnShelves = new bool[SaveData.ItemBag.GetItemCount()];

			var dayCustomerNames = new List<string>();
			foreach (string i in SaveData.PossibleCustomers)
				if (_random.NextDouble() < 0.5)
					dayCustomerNames.Insert(0, i);
				else
					dayCustomerNames.Add(i);

			for (int i = 0; i < 5; i++)
			{
				var newCustomer = (CustomerInStore)_sceneCustomer.Instance();
				_nodeCustomers.AddChild(newCustomer);
				newCustomer.Initialize(
					ResourceLoader.Load<CustomerData>(
						"res://Resources/StoreCustomers/"
						+ dayCustomerNames[i]
						+ ".tres"
						)
					);
			}

			_customersLeft = _nodeCustomers.GetChildCount() + 2;
		}
		
		public void ChooseShelfItem(ItemShelf shelf)
		{
			var a = (MenuItemChoose)_sceneMenuItemChoose.Instance();
			GetNode("UI").AddChild(a);
			a.OpenMenu(ItemsOnShelves);
			a.EventReturnItem += shelf.SetItem;
		}
		
		private void AutoShelf(object idk)
		{
			var itemArray = SaveData.ItemBag.GetItemArray();
			var itemIdx = 0;
			var shelfIdx = 0;
			while (itemIdx < itemArray.Length && shelfIdx < _nodeShelves.GetChildCount())
			{
				if (ItemsOnShelves[itemIdx])
				{
					itemIdx++;
					continue;
				}
				if (_nodeShelves.GetChild<ItemShelf>(shelfIdx).HeldItem == null)
				{
					_nodeShelves.GetChild<ItemShelf>(shelfIdx).SetItem(itemArray[itemIdx], itemIdx);
					itemIdx++;
				}

				shelfIdx++;
			}
		}

		private void OpenStore(Node door)
		{
			var itemCount = 0;
			for (int i = 0; i < _nodeShelves.GetChildCount(); i++)
				if (_nodeShelves.GetChild<ItemShelf>(i).HeldItem != null)
					itemCount++;

			if (itemCount == 0)
				return;

			door.QueueFree();
			GetNode("Interactables/AutoShelf").QueueFree();

			HighlightSellableItems();
			NextCustomer();

			GetNode<AnimationPlayer>("Hero/Anim").Play("StartStoreDay");
		}

		public void HighlightSellableItems(CustomerInStore customer = null)
		{
			var itemCount = 0;
			for (int i = 0; i < _nodeShelves.GetChildCount(); i++)
			{
				if (customer != null && customer.WillBuyItem(_nodeShelves.GetChild<ItemShelf>(i).HeldItem))
				{
					itemCount++;
					_nodeShelves.GetChild<ItemShelf>(i).CanSell = true;
				}
				else
					_nodeShelves.GetChild<ItemShelf>(i).CanSell = false;
			}

			if (customer != null && itemCount == 0)
			{
				customer.GetNode("Anim").Connect("animation_finished",
					this, nameof(CustomerAnimationFinished), new Godot.Collections.Array() { customer }
					);
				customer.PlayAnimation("DenyPlayer");
			}
		}

		public void SellFromShelf(ItemShelf shelf)
		{
			_nodeNegotiatedShelf = shelf;
			
			var item = shelf.HeldItem;

			CurrentCustomer.StartNegotiation(item);
			_nodeMenuSell.OpenMenu(item);
			HighlightSellableItems();
			shelf.NodeAnim.Play("NoItem");

			_nodeHero.NodeAnim.Play("StoreSell");
			_nodeTween.InterpolateCallback(_nodeCustomerRequestBubble, 0.5f, "hide");
			_nodeTween.InterpolateProperty(_nodeCustomerRequestBubble, "margin_top",
				-21, -15, 
				0.5f, Tween.TransitionType.Quart, Tween.EaseType.In
				);
			_nodeTween.InterpolateProperty(_nodeCustomerRequestBubble, "margin_right",
				_nodeCustomerRequestBubble.MarginRight, 12,
				0.5f, Tween.TransitionType.Quart, Tween.EaseType.In
				);
			_nodeTween.Start();
		}

		public void RaisePrice()
		{
			if (CurrentCustomer.RaisePrice())
				Deny();
		}

		public void SuperRaisePrice()
		{
			if (CurrentCustomer.SuperRaisePrice())
				Deny();
		}

		public void Accept()
		{
			SaveData.Money += CurrentCustomer.CurrentPrice;
			SaveData.ItemBag.RemoveItem(CurrentCustomer.NegotiationItemUid);
			CurrentCustomer.PlayAnimation("ItemReceived");
			_nodeNegotiatedShelf.NodeAnim.Play("NoItem");
			_nodeNegotiatedShelf.HeldItem = null;
			_nodeHero.NodeAnim.PlaybackSpeed = 1;
			NextCustomer();
		}

		public void DenyPlayer()
		{
			_nodeNegotiatedShelf?.NodeAnim.Play("CantSell");
			CurrentCustomer.PlayAnimation("DenyPlayer");
			_nodeHero.NodeAnim.PlaybackSpeed = 1;
			NextCustomer();
		}

		public void Deny()
		{
			_nodeMenuSell.CloseMenu();
			_nodeNegotiatedShelf.NodeAnim.Play("CantSell");
			CurrentCustomer.PlayAnimation("Deny");
			_nodeHero.NodeAnim.PlaybackSpeed = 1;
			NextCustomer();
		}

		private void NextCustomer()
		{
			_customersLeft -= 1;
			var count = _nodeCustomers.GetChildCount();


			if (_customersLeft > 2)
				_nodeCustomers.GetChild<CustomerInStore>(_customersLeft - 2).PlayAnimation("Step2");
			if (_customersLeft > 1 && _customersLeft < count + 1)
				_nodeCustomers.GetChild<CustomerInStore>(_customersLeft - 1).PlayAnimation("Step1");
			if (_customersLeft > 0 && _customersLeft < count)
			{
				CurrentCustomer = _nodeCustomers.GetChild<CustomerInStore>(_customersLeft);
				CurrentCustomer.PlayAnimation("Step0");
			}


			if (_customersLeft == 0)
				return; // TODO: END THE DAY;

			if (CurrentCustomer == null)
			{
				var customer = _nodeCustomers.GetChild<CustomerInStore>(_customersLeft - 2);
				customer.GetNode("Anim").Connect("animation_finished", 
					this, nameof(CustomerAnimationFinished), new Godot.Collections.Array() { customer }
					);
				return;
			}

			CurrentCustomer.DisplayRequest(_nodeCustomerRequestBubble);

			_nodeTween.Stop(_nodeCustomerRequestBubble);
			_nodeTween.InterpolateCallback(_nodeCustomerRequestBubble, 0.5f, "show");
			_nodeTween.InterpolateProperty(_nodeCustomerRequestBubble, "margin_right",
				12, 9 + _nodeCustomerRequestBubble.GetNode<Control>("Clip/Box").RectSize.x,
				0.5f, Tween.TransitionType.Elastic, Tween.EaseType.Out, 0.5f
				);
			_nodeTween.InterpolateProperty(_nodeCustomerRequestBubble, "margin_top",
				-15, -21,
				0.5f, Tween.TransitionType.Elastic, Tween.EaseType.Out, 0.5f
				);
			_nodeTween.Start();

			HighlightSellableItems(CurrentCustomer);

		}

		public int GetNegotiationPrice()
		{
			return CurrentCustomer.CurrentPrice;
		}

		public int GetPriceOpinion()
		{
			return CurrentCustomer.GetPriceOpinion();
		}


		private void CustomerAnimationFinished(string name, CustomerInStore customer)
		{
			customer.GetNode("Anim").Disconnect("animation_finished", this, nameof(CustomerAnimationFinished));
			NextCustomer(); 
		}

	}
}




