using BricksAndBalls.Core;
using BricksAndBalls.Core.Interfaces;
using BricksAndBalls.Systems.Ball;
using UnityEngine;
using Zenject;

namespace BricksAndBalls.Controllers
{
    public class ShootingController : ITickable
    {
        private readonly GameStateMachine _stateMachine;
        private readonly BallLauncher _ballLauncher;
        private readonly MockInputProvider _inputProvider;
        private readonly IAppLogger _logger;

        public ShootingController(
            GameStateMachine stateMachine,
            BallLauncher ballLauncher,
            MockInputProvider inputProvider,
            IAppLogger logger)
        {
            _stateMachine = stateMachine;
            _ballLauncher = ballLauncher;
            _inputProvider = inputProvider;
            _logger = logger;

            _logger.Log("ShootingController initialized");
        }

        public void Tick()
        {
            if (!_stateMachine.CanShoot()) return;

            HandleInput();
        }

        private void HandleInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Shoot();
            }
        }

        private void Shoot()
        {
            Vector2 direction = _inputProvider.GetLaunchDirection();
            
            _logger.Log($"Shooting in direction: {direction}");

            _stateMachine.ChangeState(GameState.BallsFlying);

            _ballLauncher.LaunchBalls(direction).Forget();
        }
    }
}