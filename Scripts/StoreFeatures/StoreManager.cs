using BrickAndMortal.Scripts.Menus;
using System.Collections.Generic;
using Godot;

namespace BrickAndMortal.Scripts.StoreFeatures
{
	class StoreManager : Node
	{
		[Export]
		private PackedScene _sceneMenuItemChoose;
		
		public bool[] ItemsOnShelves;

		private HeroComponents.HeroStore _nodeHero;
		private Node _nodeShelves;
		private Node _nodeCustomers;

		private CustomerData[] _dayCustomers;

		private System.Random _random = new System.Random();

		public override void _Ready()
		{
			SaveData.LoadGame();

			_nodeHero = GetNode<HeroComponents.HeroStore>("Hero");

			_nodeShelves = GetNode("Room/Shelves");
			_nodeCustomers = GetNode("Customers");

			ItemsOnShelves = new bool[SaveData.ItemBag.GetItemCount()];

			var dayCustomerNames = new List<string>();
			foreach (string i in SaveData.PossibleCustomers)
				if (_random.NextDouble() < 0.5)
					dayCustomerNames.Insert(0, i);
				else
					dayCustomerNames.Add(i);
			_dayCustomers = new CustomerData[dayCustomerNames.Count];
			for (int i = 0; i < _dayCustomers.Length; i++)
				_dayCustomers[i] = ResourceLoader.Load<CustomerData>(
					"res://Resources/StoreCustomers/" 
					+ dayCustomerNames[i]
					+ ".tres"
					);
			//TODO: create customer instances and Initialize with loaded data.
		}
		
		public void ChooseShelfItem(ItemShelf shelf)
		{
			var a = (MenuItemChoose)_sceneMenuItemChoose.Instance();
			GetNode("UI").AddChild(a);
			a.OpenMenu(ItemsOnShelves);
			a.EventReturnItem += shelf.SetItem;
		}
		
		private void OpenStore(object idk)
		{
			GetNode<AnimationPlayer>("Hero/Anim").Play("StartStoreDay");
			foreach (Node i in _nodeShelves.GetChildren())
				((ItemShelf)i).CanSell = true;
		}
		
		public void SellFromShelf(ItemShelf shelf)
		{
			shelf.NodeAnim.Play("NoItem");
			
			var item = shelf.HeldItem;
			// TODO
		}
		
		private void SoldItem()
		{

		}
		
		private void NextCustomer()
		{
			
		}

		private void AutoShelf(object idk)
		{

		}
	}
}




