using Godot;

namespace BrickAndMortal.Scripts.DungeonFeatures
{
    abstract class DungeonPersistentBase : Sprite
    {
        public abstract void DeserializeFrom(string from);
        public abstract string GetSerialized();
        public abstract void Initialize();
    }
}
