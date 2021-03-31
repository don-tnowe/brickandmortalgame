using BrickAndMortal.Scripts.ItemOperations;
using System.Collections.Generic;
using System;
using Godot;

namespace BrickAndMortal.Scripts.DungeonFeatures
{
    class DungeonAreaPool : Resource
    {
        [Export]
        private string _roomLoadFolder = "res://Scenes/Rooms";
        [Export]
        private string[] _roomNames;
        [Export]
        private StreamTexture _itemAtlas;
        [Export]
        private int[] _itemPSM = new int[3];
        [Export]
        private int[] _itemEnchantments;

        private Random _random;

        public string GetRandomRoomPath()
        {
            return _roomLoadFolder + _roomNames[_random.Next() % _roomNames.Length];
        }

        public Item GetRandomItem()
        {
            var item = new Item() 
            {
                Power = _itemPSM[0] / 2 + _random.Next(_itemPSM[0]),
                Shine = _itemPSM[1] / 2 + _random.Next(_itemPSM[1]),
                Magic = _itemPSM[2] / 2 + _random.Next(_itemPSM[2]),
            };


            return item;
        }
    }
}
