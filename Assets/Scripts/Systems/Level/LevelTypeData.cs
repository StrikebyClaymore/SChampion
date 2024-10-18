using System;
using UnityEngine;

namespace Systems
{
    [Serializable]
    public class LevelTypeData
    {
        public EGameTypes Type;
        public Sprite BaseSprite;
        public Sprite SelectedSprite;
        public LevelData[] Levels;
    }
}