using System.Collections.Generic;
using System.Linq;
using Systems.Game.Entities;
using UnityEditor;
using UnityEngine;
using static Extensions.RandomExtension;

namespace Systems
{
    public static class LevelGenerator
    {
        private const int MAX_WAVE_SIZE = 6;
        private const int MAX_BONUSES = 3;
        private const int MIN_BONUSES = 0;
        private const int MAX_BONUSES_VALUE = 10;
        private const int MAX_BONUS_VALUE = 6;
        private const int MIN_BONUS_VALUE = 1;
        
        private const int MAX_OBSTACLES = 6;
        private const int MIN_OBSTACLES = 2;
        private const int MAX_OBSTACLE_VALUE = 50;
        private const int MIN_OBSTACLE_VALUE = 1;
        
        private const int MAX_STARS_IN_WAVE = 2;
        
        public static LevelData GenerateLevelData(int length)
        {
            var data = new LevelData()
            {
                Waves = new List<LevelWave>() { }
            };

            for (int i = 0; i < length; i++)
            {
                var starWave1 = GenerateStarsWave(true);
                var bonusWave1 = GenerateBonusWave();
                var stars1Count = starWave1.Numbers.Count(n => n > 0);
                var starWave2 = GenerateStarsWave(stars1Count > 0);
                var bonusesValue = bonusWave1.Numbers.Sum();
                var bonusWave2 = GenerateBonusWave(bonusesValue);
                var obstaclesWave = GenerateObstacleWave(bonusWave1);
                var stars2Count = starWave2.Numbers.Count(n => n > 0);
                var starWave3 = GenerateStarsWave(stars2Count > 0, stars1Count > 0 && stars2Count > 0);
            
                data.Waves.Add(starWave1);
                data.Waves.Add(bonusWave1);
                data.Waves.Add(starWave2);
                data.Waves.Add(bonusWave2);
                data.Waves.Add(obstaclesWave);
                data.Waves.Add(starWave3);
            
                Debug.Log("Звёзды " + string.Join(", ", starWave3.Numbers));
                Debug.Log("Препятствия: " + string.Join(", ", obstaclesWave.Numbers));
                Debug.Log("Бонусы: " + string.Join(", ", bonusWave2.Numbers));
                Debug.Log("Звёзды: " + string.Join(", ", starWave2.Numbers));
                Debug.Log("Бонусы: " + string.Join(", ", bonusWave1.Numbers));
                Debug.Log("Звёзды: " + string.Join(", ", starWave1.Numbers));
            }

            return data;
        }

        public static LevelData Generate(LevelData data)
        {
            if (data == null)
            {
                data = new LevelData()
                {
                    Waves = new List<LevelWave>() { }
                };
            }

            var starWave1 = GenerateStarsWave(true);
            
            var bonusWave1 = GenerateBonusWave();
            
            var stars1Count = starWave1.Numbers.Count(n => n > 0);
            var starWave2 = GenerateStarsWave(stars1Count > 0);
            
            var bonusesValue = bonusWave1.Numbers.Sum();
            var bonusWave2 = GenerateBonusWave(bonusesValue);
            
            var obstaclesWave = GenerateObstacleWave(bonusWave1);
            
            var stars2Count = starWave2.Numbers.Count(n => n > 0);
            var starWave3 = GenerateStarsWave(stars2Count > 0, stars1Count > 0 && stars2Count > 0);
            
            data.Waves.Add(starWave1);
            data.Waves.Add(bonusWave1);
            data.Waves.Add(starWave2);
            data.Waves.Add(bonusWave2);
            data.Waves.Add(obstaclesWave);
            data.Waves.Add(starWave3);
            
            return data;
        }
        
        private static LevelWave GenerateStarsWave(bool chance = false, bool empty = false)
        {
            var wave = new LevelWave
            {
                Type = EEntities.Star,
                Delay = Random.Range(0.5f, 1f),
                Numbers = new int[MAX_WAVE_SIZE]
            };
            if (empty || (chance && !BoolRandom()))
            {
                wave.Delay = 0;
                return wave;
            }
            int random = Random.Range(0, MAX_WAVE_SIZE);
            wave.Numbers[random] = 1;
            return wave;
        }

        private static LevelWave GenerateObstacleWave(LevelWave bonusWave)
        {
            var wave = new LevelWave
            {
                Type = EEntities.Obstacle,
                Delay = 1,
                Numbers = new int[MAX_WAVE_SIZE]
            };
            
            int maxBonusValue = bonusWave.Numbers.Max();
            wave.Delay = Random.Range(1f, 1.5f) + 0.2f * maxBonusValue;
            int bonuses = bonusWave.Numbers.Count(n => n > 0);
            int obstacles = Random.Range(Mathf.Min(MIN_OBSTACLES + bonuses, MAX_WAVE_SIZE + 1), MAX_OBSTACLES + 1);
            bool hasPassage = false;

            for (int i = 0; i < obstacles; i++)
            {
                var value = Random.Range(MIN_OBSTACLE_VALUE, MAX_OBSTACLE_VALUE + 1);
                wave.Numbers[i] = value;
                if (value <= maxBonusValue)
                    hasPassage = true;
            }
            
            if (!hasPassage)
            {
                var random = Random.Range(0, MAX_WAVE_SIZE);
                wave.Numbers[random] = maxBonusValue;
            }
            return wave;
        }

        private static LevelWave GenerateBonusWave(int previousValue = 0)
        {
            var wave = new LevelWave
            {
                Type = EEntities.Bonus,
                Delay = Random.Range(0.75f, 1.5f),
                Numbers = new int[MAX_WAVE_SIZE]
            };
            int bonuses = Random.Range(1, 3);
            var excluding = new List<int>();
            int allValue = previousValue;
            for (int i = 0; i < bonuses; i++)
            {
                if(allValue >= MAX_BONUSES_VALUE)
                    break;
                var random = GetRandomExcluding(0, MAX_WAVE_SIZE - 1, excluding);
                excluding.Add(random);
                var availableValue = MAX_BONUSES_VALUE - allValue; 
                var value = Random.Range(MIN_BONUS_VALUE, Mathf.Min(availableValue + 1, MAX_BONUS_VALUE + 1));
                allValue += value;
                wave.Numbers[random] = value;
            }
            return wave;
        }
    }
}