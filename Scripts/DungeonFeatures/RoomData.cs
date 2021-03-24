using System;
using Godot;

namespace BrickAndMortal.Scripts.DungeonFeatures
{
    class RoomData
    {
        public string ScenePath;
        public bool[] SlainEnemies;
        public bool[] BlockedExits;

        public RoomData() { }

        public RoomData(string scenePath, bool[] slainEnemies, bool[] blockedExits)
        {
            ScenePath = scenePath;
            SlainEnemies = slainEnemies;
            BlockedExits = blockedExits;
        }

        public override string ToString()
        {
            return ScenePath + " " + SlainEnemies.Length;
        }
    }
}
