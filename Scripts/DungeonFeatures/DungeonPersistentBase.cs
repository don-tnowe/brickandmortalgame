using Godot;

namespace BrickAndMortal.Scripts.DungeonFeatures
{
	abstract class DungeonPersistentBase : Sprite
	{
		public static string[] Prefixes = 
		{ 
			"Null ||" ,
			"ItemPedestal ||"
		};

        public abstract string SerializedPrefix {get;}

        public abstract void DeserializeFrom(string from);
		public abstract string GetSerialized();
		public abstract void Initialize();
	}
}
