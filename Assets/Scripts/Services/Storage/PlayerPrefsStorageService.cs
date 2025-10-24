using BricksAndBalls.Core.Interfaces;
using UnityEngine;

namespace BricksAndBalls.Services.Storage
{
    public class PlayerPrefsStorageService : IStorageService
    {
        private readonly IAppLogger _logger;

        public PlayerPrefsStorageService(IAppLogger logger)
        {
            _logger = logger;
        }

        public void Save<T>(string key, T data)
        {
            var json = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(key, json);
            PlayerPrefs.Save();
            _logger.Log($"Saved data to key: {key}");
        }

        public T Load<T>(string key, T defaultValue = default)
        {
            if (!HasKey(key))
            {
                _logger.LogWarning($"Key not found: {key}, returning default value");
                return defaultValue;
            }

            var json = PlayerPrefs.GetString(key);
            var data = JsonUtility.FromJson<T>(json);
            _logger.Log($"Loaded data from key: {key}");
            return data;
        }

        public bool HasKey(string key)
        {
            return PlayerPrefs.HasKey(key);
        }

        public void DeleteKey(string key)
        {
            PlayerPrefs.DeleteKey(key);
            _logger.Log($"Deleted key: {key}");
        }

        public void DeleteAll()
        {
            PlayerPrefs.DeleteAll();
            _logger.Log("Deleted all PlayerPrefs data");
        }
    }
}
