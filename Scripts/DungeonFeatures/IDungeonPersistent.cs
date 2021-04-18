using Godot;

namespace BrickAndMortal.Scripts.DungeonFeatures
{
	interface IDungeonPersistent
	{
		string GetSerializedPrefix();
        void DeserializeFrom(string from);
		string GetSerialized();
		void Initialize();
	}
}
