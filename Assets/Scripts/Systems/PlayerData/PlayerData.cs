namespace Systems
{
    /// <summary>
    /// Хранит данные об игроке. Текущее оружие, золото, алмазы и т.д. 
    /// </summary>
    public static class PlayerData
    {
        public static PlayerPrefsData Data { get; } = new PlayerPrefsData();

        public static bool GetTutorialCompleted() => Data.GetBool(GameConstants.TUTORAIL_COMPLETED_SAVE_KEY);
        
        public static void SetTutorialCompleted() => Data.SetBool(GameConstants.TUTORAIL_COMPLETED_SAVE_KEY, true);

        public static bool GetGameTypeUnlocked(EGameTypes type) => Data.GetBool(type.ToString());

        public static void SetGameTypeUnlocked(EGameTypes type) => Data.SetBool(type.ToString(), true);

        public static int GetLevelCompleted(EGameTypes type) => Data.GetInt(type + GameConstants.LEVEL_COMPLETED_SAVE_KEY, 0);

        public static void SetLevelCompleted(EGameTypes type, int value) => Data.SetInt(type + GameConstants.LEVEL_COMPLETED_SAVE_KEY, value);

        public static int GetBestScore(EGameTypes game, ERecordPlace type) => Data.GetInt($"{game}{type}{GameConstants.RECORD_SCORE_SAVE_KEY}", 0);
        
        public static void SetBestScore(EGameTypes game, ERecordPlace type, int value) => Data.SetInt($"{game}{type}{GameConstants.RECORD_SCORE_SAVE_KEY}", value);

        public static int GetInt(string name)
        {
            if (!Data.HasKey(name))
                return -1;
            return Data.GetInt(name, 0);
        }
        
        public static void SetIntValue(string name, int value)
        {
            if (value < 0 || !Data.HasKey(name))
                return;
            Data.SetInt(name, value);
        }
        
        public static void AddIntValue(string name, int value)
        {
            if (value <= 0 || !Data.HasKey(name))
                return;
            int currentValue = Data.GetInt(name);
            long nextValue = currentValue + value;
            if (nextValue > int.MaxValue)
                currentValue = int.MaxValue;
            else
                currentValue = (int)nextValue;
            Data.SetInt(name, currentValue);
        }

        public static void RemoveIntValue(string name, int value)
        {
            if (value <= 0 || !Data.HasKey(name))
                return;
            int currentValue = Data.GetInt(name);
            int nextValue = currentValue - value;
            Data.SetInt(name, nextValue);
        }
        
        public static bool HasEnoughIntValue(string name, int value)
        {
            if (!Data.HasKey(name))
                return false;
            int currentValue = Data.GetInt(name);
            return currentValue >= value;
        }
    }
}