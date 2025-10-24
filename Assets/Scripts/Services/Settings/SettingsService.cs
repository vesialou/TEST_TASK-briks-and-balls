using System;
using BricksAndBalls.Core.Interfaces;

namespace BricksAndBalls.Services.Settings
{
    public class SettingsService : ISettingsService
    {
        private const string SettingsKey = "PlayerSettings";
        private const string DefaultPlayerName = "Player";

        private readonly IStorageService _storage;
        private readonly IAppLogger _appLogger;
        private PlayerSettings _settings;

        public SettingsService(IStorageService storage, IAppLogger appLogger)
        {
            _storage = storage;
            _appLogger = appLogger;

            LoadSettings();
        }

        private void LoadSettings()
        {
            _settings = _storage.Load<PlayerSettings>(SettingsKey, new PlayerSettings
            {
                PlayerName = DefaultPlayerName,
                Volume = 1f,
                Vibration = true
            });

            _appLogger.Log($"Settings loaded: PlayerName={_settings.PlayerName}");
        }

        public void SetPlayerName(string name)
        {
            _settings.PlayerName = string.IsNullOrWhiteSpace(name) ? DefaultPlayerName : name;
            SaveSettings();
        }

        public string GetPlayerName() => _settings.PlayerName;

        public void SetVolume(float volume)
        {
            _settings.Volume = UnityEngine.Mathf.Clamp01(volume);
            SaveSettings();
        }

        public float GetVolume() => _settings.Volume;

        private void SaveSettings()
        {
            _storage.Save(SettingsKey, _settings);
            _appLogger.Log("Settings saved");
        }
    }

    [Serializable]
    public class PlayerSettings
    {
        public string PlayerName;
        public float Volume;
        public bool Vibration;
    }
}
