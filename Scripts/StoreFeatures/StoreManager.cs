using System.Collections.Generic;
using Godot;

namespace BrickAndMortal.Scripts.StoreFeatures
{
	class StoreManager : Node
	{
		private CustomerData[] _dayCustomers;

		private System.Random _random = new System.Random();

		public override void _Ready()
		{
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
	}
}
