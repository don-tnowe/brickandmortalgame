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
		[Export]
		private PackedScene _sceneItemParachute;

		private bool[] _itemsOnShelves;

		private CustomerData _currentCustomer;
		private CustomerInStore _currentCustomerNode;
		private int _currentPrice = 1;
		private float _currentDeny = 0;

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
			_nodeHero = GetNode<HeroComponents.HeroStore>("Hero");
			_nodeShelves = GetNode("Room/Shelves");
			_nodeCustomers = GetNode("Customers");
			_nodeCustomerRequestBubble = GetNode<Control>("ZAbove/CustomerRequest");
			_nodeMenuSell = GetNode<MenuSell>("UI/MenuSell");
			_nodeTween = GetNode<Tween>("UI/Tween");

			_itemsOnShelves = new bool[SaveData.ItemBag.GetItemCount()];

			var dayCustomerNames = new List<string>();

			foreach (string i in SaveData.PossibleCustomers)
				if (_random.NextDouble() < 0.5)
					dayCustomerNames.Insert(0, i);
				else
					dayCustomerNames.Add(i);

			var customerCount = 4 + SaveData.CurCrawl / 3;

			if (customerCount > SaveData.PossibleCustomers.Count)
				customerCount = SaveData.PossibleCustomers.Count;

			for (int i = 0; i < customerCount; i++)
			{
				var newCustomer = (CustomerInStore)_sceneCustomer.Instance();
				_nodeCustomers.AddChild(newCustomer);
				newCustomer.Customer = (
					ResourceLoader.Load<CustomerData>(
						"res://Resources/StoreCustomers/"
						+ dayCustomerNames[i]
						+ ".tres"
						)
					);
			}

			_customersLeft = customerCount + 2;

			SaveData.LastDayCustomers = customerCount;
			SaveData.LastDayEarned = 0;
			SaveData.LastDaySold = 0;
		}
		
		public void ChooseShelfItem(ItemShelf shelf)
		{
			var a = (MenuItemChoose)_sceneMenuItemChoose.Instance();
			GetNode("UI").AddChild(a);
			a.OpenMenu(_itemsOnShelves);
			a.EventReturnItem += shelf.SetItem;
		}
		
		private void AutoShelf(object idk)
		{
			var itemArray = SaveData.ItemBag.GetItemArray();
			var itemIdx = 0;
			var shelfIdx = 0;
			while (itemIdx < itemArray.Length && shelfIdx < _nodeShelves.GetChildCount())
			{
				if (_itemsOnShelves[itemIdx])
				{
					itemIdx++;
					continue;
				}
				if (_nodeShelves.GetChild<ItemShelf>(shelfIdx).HeldItem == null)
				{
					_nodeShelves.GetChild<ItemShelf>(shelfIdx).SetItem(itemArray[itemIdx], itemIdx, _itemsOnShelves);
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

		public void HighlightSellableItems(CustomerData customer = null)
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
		}

		public void StartNegotiation(ItemShelf shelf)
		{
			_nodeNegotiatedShelf = shelf;
			
			var item = shelf.HeldItem;

			_currentPrice = _currentCustomer.GetItemStartingPrice(item);
			_currentDeny = _currentCustomer.GetStartingDeny();
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
			if (_currentDeny > _random.NextDouble() * 0.5)
			{ 
				DenyByCustomer();
				return;
			}
			_currentPrice = _currentCustomer.GetIncrementedPrice(_currentPrice);
			_currentDeny = _currentCustomer.GetIncrementedDeny(_currentPrice, _currentDeny);
		}

		public void SuperRaisePrice()
		{
			if (_currentDeny > _random.NextDouble())
			{
				DenyByCustomer();
				return;
			}
			_currentPrice = _currentCustomer.GetSuperIncrementedPrice(_currentPrice);
			_currentDeny = _currentCustomer.GetIncrementedDeny(_currentPrice, _currentDeny);
		}

		public void AcceptSale()
		{
			SaveData.LastDaySold++;
			SaveData.Money += _currentPrice;
			SaveData.LastDayEarned += _currentPrice;
			SaveData.ItemBag.RemoveItem(_currentCustomerNode.NegotiationItemUid);

			_currentCustomerNode.PlayAnimation("ItemReceived");
			_nodeNegotiatedShelf.NodeAnim.Play("NoItem");
			_nodeNegotiatedShelf.HeldItem = null;
			_nodeHero.NodeAnim.PlaybackSpeed = 1;

			NextCustomer();

			var newNode = (Path2D)_sceneItemParachute.Instance();
			newNode.GlobalPosition = _nodeNegotiatedShelf.GlobalPosition;
			newNode.Curve.AddPoint(_currentCustomerNode.GlobalPosition - _nodeNegotiatedShelf.GlobalPosition);
			AddChild(newNode);
		}

		public void DenyByPlayer(object idk = null)
		{
			_currentCustomerNode?.PlayAnimation("DenyPlayer");
			Deny();
		}

		public void DenyByCustomer()
		{
			_nodeMenuSell.CloseMenu();
			_currentCustomerNode.PlayAnimation("Deny");
			Deny();
		}

		private void Deny()
		{
			_nodeNegotiatedShelf?.ReturnToShelf();
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
				_currentCustomerNode = _nodeCustomers.GetChild<CustomerInStore>(_customersLeft);
				_currentCustomer = _currentCustomerNode.Customer;
				_currentCustomerNode.PlayAnimation("Step0");
			}

			if (_customersLeft == 0)
			{
				_nodeTween.InterpolateCallback(this, 3, "EndDay");
				_nodeCustomerRequestBubble.Visible = false;
				return;
			}

			if (_currentCustomer == null)
			{
				var customer = _nodeCustomers.GetChild<CustomerInStore>(_customersLeft - 2);
				customer.GetNode("Anim").Connect("animation_finished",
					this, nameof(CustomerAnimationFinished), new Godot.Collections.Array() { customer, true }
					);
				return;
			}

			GetNode<InteractableProps.InteractableArea>("Interactables/QuickDeny").Interactable = false;

			_currentCustomerNode.GetNode("Anim").Connect("animation_finished",
					this, nameof(CustomerAnimationFinished), new Godot.Collections.Array() { _currentCustomerNode, false });
		}

		public int GetNegotiationPrice()
		{
			return _currentPrice;
		}

		public int GetPriceOpinion()
		{
			return _currentCustomer.GetPriceOpinion(_currentPrice, _currentDeny);
		}

		private void CustomerAnimationFinished(string name, Node customer, bool skip)
		{
			customer.GetNode("Anim").Disconnect("animation_finished", this, nameof(CustomerAnimationFinished));

			if (skip)
			{
				NextCustomer();
				return;
			}

			_currentCustomer.DisplayRequest(_nodeCustomerRequestBubble);

			_nodeTween.Stop(_nodeCustomerRequestBubble);
			_nodeTween.InterpolateCallback(_nodeCustomerRequestBubble, 0.1f, "show");
			_nodeTween.InterpolateProperty(_nodeCustomerRequestBubble, "margin_right",
				12, 9 + _nodeCustomerRequestBubble.GetNode<Control>("Clip/Box").RectSize.x,
				0.5f, Tween.TransitionType.Elastic, Tween.EaseType.Out
				);
			_nodeTween.InterpolateProperty(_nodeCustomerRequestBubble, "margin_top",
				-15, -21,
				0.5f, Tween.TransitionType.Elastic, Tween.EaseType.Out
				);
			_nodeTween.Start();
			GetNode<InteractableProps.InteractableArea>("Interactables/QuickDeny").Interactable = true;

			HighlightSellableItems(_currentCustomer);
		}

		private void EndDay()
		{
			if (SaveData.BestDayEarned < SaveData.LastDayEarned) 
				SaveData.BestDayEarned = SaveData.LastDayEarned;

			SaveData.SaveGame();

			GetTree().ChangeScene("res://Scenes/Screens/Town.tscn");
		}	
	}
}








