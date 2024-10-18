using System;
using UnityEngine;

namespace Systems
{
    [Serializable]
    public struct GameTypeData
    {
        public EGameTypes Type;
        public string Name;
        public int Cost;
        public Sprite Sprite;
        public Sprite LockSprite;
        public bool CanUnlock;
        [HideInInspector] public bool IsUnlocked;
    }
}