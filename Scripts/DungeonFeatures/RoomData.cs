
namespace BrickAndMortal.Scripts.DungeonFeatures
{
	class RoomData
	{
		public string ScenePath;
		public bool[] SlainEnemies;
		public string[] PersistentObjects;
		public bool[] BlockedExits;
		public int[] MapMods;
		
		public string ToJSON() 
		{
			return "\"Value\":\"nay i'll do this later\""; // TODO
		}
	} 
}
