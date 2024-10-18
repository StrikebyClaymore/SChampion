using System;
using System.Collections.Generic;

namespace Extensions
{
    public static class RandomExtension
    {
        public static bool BoolRandom()
        {
            return UnityEngine.Random.Range(0, 2) != 0;
        }
        
        public static int GetRandomExcluding(int min, int max, List<int> excluding)
        {
            HashSet<int> excludeSet = new HashSet<int>(excluding);
            List<int> validNumbers = new List<int>();
            for (int i = min; i <= max; i++)
            {
                if (!excludeSet.Contains(i))
                {
                    validNumbers.Add(i);
                }
            }
            if (validNumbers.Count == 0)
            {
                throw new InvalidOperationException("No valid numbers available to choose from.");
            }
            return validNumbers[UnityEngine.Random.Range(0, validNumbers.Count)];
        }
    }
}