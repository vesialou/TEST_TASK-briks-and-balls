using BricksAndBalls.Configs;
using BricksAndBalls.Core.Interfaces;
using UnityEngine;

namespace BricksAndBalls.Systems.Ball
{
    public class BallSpeedNormalizer
    {
        private readonly GameConfig _gameConfig;
        private readonly IAppLogger _logger;

        public BallSpeedNormalizer(GameConfig gameConfig, IAppLogger logger)
        {
            _gameConfig = gameConfig;
            _logger = logger;
        }

        public void NormalizeSpeed(Rigidbody2D rigidbody)
        {
            if (rigidbody.velocity.sqrMagnitude < 0.01f)
                return;

            Vector2 normalizedVelocity = rigidbody.velocity.normalized * _gameConfig.BallSpeed;
            rigidbody.velocity = normalizedVelocity;
        }
    }
}