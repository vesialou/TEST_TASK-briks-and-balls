using BricksAndBalls.Core.Interfaces;
using UnityEngine;

namespace BricksAndBalls.Services.Audio
{
    public class AudioManager
    {
        private readonly IAppLogger _logger;
        private AudioSource _musicSource;

        public AudioManager(IAppLogger logger)
        {
            _logger = logger;
            _logger.Log("AudioManager initialized");
        }

        public void PlaySound(AudioClip clip, float volume = 1f)
        {
            if (clip == null)
            {
                _logger.LogWarning("AudioManager.PlaySound: clip is null");
                return;
            }

            _logger.LogError("AudioManager.PlaySound not implemented");
        }

        public void PlayMusic(AudioClip clip, bool loop = true)
        {
            if (clip == null)
            {
                _logger.LogWarning("AudioManager.PlayMusic: clip is null");
                return;
            }

            _logger.LogError("AudioManager.PlayMusic not implemented");
        }

        public void StopMusic()
        {
            if (_musicSource != null)
            {
                _musicSource.Stop();
            }
        }

        public void SetVolume(float volume)
        {
            _logger.Log($"Volume set to: {volume}");
        }
    }
}