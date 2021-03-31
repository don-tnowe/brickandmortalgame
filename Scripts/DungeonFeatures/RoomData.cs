using System;
using Godot;

namespace BrickAndMortal.Scripts.DungeonFeatures
{
    class RoomData
    {
        public string ScenePath;
        public bool[] SlainEnemies;
        public string[] PersistentObjects;
        public bool[] BlockedExits;

        public RoomData() { }

        public RoomData(string scenePath, bool[] slainEnemies, bool[] blockedExits, string[] persistentObjects)
        {
            ScenePath = scenePath;
            SlainEnemies = slainEnemies;
            BlockedExits = blockedExits;
            PersistentObjects = persistentObjects;
        }

        public override string ToString()
        {
            return ScenePath + " " + SlainEnemies.Length;
        }
    }
}
