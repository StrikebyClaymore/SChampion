using System;
using Systems.Game.Entities;

namespace Systems
{
    [Serializable]
    public class LevelWave
    {
        public EEntities Type;
        public float Delay;
        public int[] Numbers;
    }
}