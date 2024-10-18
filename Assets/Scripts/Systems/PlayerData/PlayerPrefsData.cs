using UnityEngine;

namespace Systems
{
    public class PlayerPrefsData
    {
        public T GetData<T>(string key) where T : class
        {
            if (string.IsNullOrEmpty(key) || !PlayerPrefs.HasKey(key))
                return null;
            var value = PlayerPrefs.GetString(key, string.Empty);
            if (!string.IsNullOrEmpty(value))
                return JsonUtility.FromJson<T>(value);
            return null;
        }

        public void SetData<T>(string key, T value) where T : class
        {
            if (value == null || string.IsNullOrEmpty(key))
                return;
            var json = JsonUtility.ToJson(value);
            PlayerPrefs.SetString(key, json);
        }

        public int GetInt(string key, int defaultValue = 0)
        {
            return PlayerPrefs.GetInt(key, defaultValue);
        }

        public void SetInt(string key, int value)
        {
            PlayerPrefs.SetInt(key, value);
        }

        public float GetFloat(string key, float defaultValue = 0)
        {
            return PlayerPrefs.GetFloat(key, defaultValue);
        }

        public void SetFloat(string key, float value)
        { 
            PlayerPrefs.SetFloat(key, value);
        }

        public string GetString(string key, string defaultValue = "")
        {
            return PlayerPrefs.GetString(key, defaultValue);
        }

        public void SetString(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
        }

        public bool GetBool(string key, bool defaultValue = false)
        {
            return PlayerPrefs.GetInt(key, defaultValue ? 1 : 0) == 1;
        }

        public void SetBool(string key, bool value)
        {
            PlayerPrefs.SetInt(key, value ? 1 : 0);
        }

        public bool HasKey(string key)
        {
            return PlayerPrefs.HasKey(key);
        }
    }
}