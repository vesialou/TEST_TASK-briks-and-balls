using BricksAndBalls.Core.Interfaces;
using UnityEngine;

namespace BricksAndBalls.Systems.Ball
{
    public class TimeScaleController
    {
        private readonly IAppLogger _logger;
        private float _currentTimeScale = 1f;

        public float CurrentTimeScale => _currentTimeScale;

        public TimeScaleController(IAppLogger logger)
        {
            _logger = logger;
        }

        public void SetTimeScale(float scale)
        {
            _currentTimeScale = Mathf.Max(0.1f, scale);
            Time.timeScale = _currentTimeScale;
            _logger.Log($"TimeScale set to {_currentTimeScale}x");
        }

        public void ResetTimeScale()
        {
            SetTimeScale(1f);
        }

        public void SpeedUp(float multiplier = 100f)
        {
            SetTimeScale(multiplier);
        }
    }
}