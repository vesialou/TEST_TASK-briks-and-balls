using BricksAndBalls.Configs;
using BricksAndBalls.Core.Interfaces;
using BricksAndBalls.Factories;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace BricksAndBalls.Systems.Ball
{
    public class BallLauncher
    {
        private readonly BallFactory _ballFactory;
        private readonly BallPhysicsSystem _ballPhysicsSystem;
        private readonly GameConfig _gameConfig;
        private readonly MockInputProvider _inputProvider;
        private readonly IAppLogger _logger;

        private Vector3 _launchPosition;
        private bool _isLaunching;

        public BallLauncher(
            BallFactory ballFactory,
            BallPhysicsSystem ballPhysicsSystem,
            GameConfig gameConfig,
            MockInputProvider inputProvider,
            IAppLogger logger)
        {
            _ballFactory = ballFactory;
            _ballPhysicsSystem = ballPhysicsSystem;
            _gameConfig = gameConfig;
            _inputProvider = inputProvider;
            _logger = logger;
        }

        public void SetLaunchPosition(Vector3 position)
        {
            _launchPosition = position;
        }

        public async UniTaskVoid LaunchBalls(Vector2 direction)
        {
            if (_isLaunching)
            {
                _logger.LogWarning("BallLauncher: Launch already in progress");
                return;
            }

            _isLaunching = true;

            int ballsToLaunch = _gameConfig.BallsCount;
            float launchInterval = 0.2f;

            _logger.Log($"Launching {ballsToLaunch} balls with interval {launchInterval}s");

            for (int i = 0; i < ballsToLaunch; i++)
            {
                LaunchSingleBall(direction);
                
                if (i < ballsToLaunch - 1)
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(launchInterval));
                }
            }

            _isLaunching = false;
            _logger.Log("All balls launched");
        }

        public void LaunchBallsWithMockInput()
        {
            Vector2 direction = _inputProvider.GetLaunchDirection();
            LaunchBalls(direction).Forget();
        }

        private void LaunchSingleBall(Vector2 direction)
        {
            BallInstance ball = _ballFactory.Create(_launchPosition);
            
            if (ball == null)
            {
                _logger.LogError("Failed to create ball");
                return;
            }

            Rigidbody2D rb = ball.Physics.Rigidbody;
            rb.velocity = direction.normalized * _gameConfig.BallSpeed;

            ball.Model.IsActive = true;
            ball.View.SetTrailActive(true);

            _ballPhysicsSystem.IncrementActiveBallCount();

            _logger.Log($"Ball {ball.Model.ID} launched with velocity {rb.velocity}");
        }
    }
}