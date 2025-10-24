using BricksAndBalls.Core.Interfaces;
using BricksAndBalls.Core.Models;
using BricksAndBalls.Core.Signals;
using BricksAndBalls.Factories;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace BricksAndBalls.Systems.Ball
{
    public class BallLauncher
    {
        private readonly BallFactory _ballFactory;
        private readonly BallPhysicsSystem _ballPhysicsSystem;
        private readonly IGameConfig _gameConfig;
        private readonly LaunchPointMarker _marker;
        private readonly SignalBus _signalBus;
        private readonly IAppLogger _logger;

        private Vector3 _launchPosition;
        private bool _isLaunching;

        public BallLauncher(
            BallFactory ballFactory,
            BallPhysicsSystem ballPhysicsSystem,
            IGameConfig gameConfig,
            LaunchPointMarker marker,
            SignalBus  signalBus,
            IAppLogger logger)
        {
            _ballFactory = ballFactory;
            _ballPhysicsSystem = ballPhysicsSystem;
            _gameConfig = gameConfig;
            _marker = marker;
            _signalBus = signalBus;
            _logger = logger;
        }

        public void UpdateLaunchPosition(Vector3 position)
        {
            _launchPosition.x = position.x;
            _marker.UpdatePosition(_launchPosition);
        }
        
        public void SetLaunchPosition(Vector3 position)
        {
            _launchPosition = position;
            _marker.UpdatePosition(_launchPosition);
        }

        public async UniTaskVoid LaunchBalls(Vector2 direction)
        {
            if (_isLaunching)
            {
                _logger.LogWarning("BallLauncher: Launch already in progress");
                return;
            }

            _isLaunching = true;

            var ballsToLaunch = _gameConfig.BallsCount;
            var launchIntervalMs = 200;

            _logger.Log($"Launching {ballsToLaunch} balls");
            var launchPosition = _launchPosition;
            for (var i = 0; i < ballsToLaunch; i++)
            {
                LaunchSingleBall(direction, launchPosition);
                
                if (i < ballsToLaunch - 1)
                {
                    await UniTask.Delay(launchIntervalMs, ignoreTimeScale: false);
                }
                else
                {
                    _signalBus.Fire<LastBallLaunchedSignal>();
                    if (!_ballPhysicsSystem.HasFirstLandPos())
                    {
                        _marker.Hide();
                    }
                }
            }

            _isLaunching = false;
            _logger.Log("All balls launched");
        }
        
        public Vector3 GetLaunchPosition()
        {
            return _launchPosition; 
        }

        private void LaunchSingleBall(Vector2 direction, Vector3 position)
        {
            var date = new BallData() { Position = position, Direction = direction };
            var ball = _ballFactory.Create(date);
            
            if (Equals(ball, default(BallInstance)))
            {
                _logger.LogError("Failed to create ball");
                return;
            }

            var velocity = direction.normalized * _gameConfig.BallSpeed;
            ball.Physics.Rigidbody.velocity = velocity;
            ball.Model.IsActive = true;

            _ballPhysicsSystem.IncrementActiveBallCount();
        }

    }
}