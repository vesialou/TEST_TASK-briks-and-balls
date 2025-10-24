using System;
using BricksAndBalls.Core.Interfaces;
using BricksAndBalls.Core.Models;
using BricksAndBalls.Core.Signals;
using BricksAndBalls.Systems.Ball;
using UnityEngine;
using Zenject;

namespace BricksAndBalls.Gameplay
{
    public class ShootingController : IInitializable, ITickable, IDisposable
    {
        private readonly AimingSystem _aimingSystem;
        private readonly SignalBus _signalBus;
        private readonly BallPhysicsSystem _ballPhysicsSystem;
        private readonly GameStateMachine _stateMachine;
        private readonly BallLauncher _ballLauncher;
        private readonly LaunchPointMarker _marker;
        private readonly IAppLogger _logger;

        public ShootingController(
            AimingSystem aimingSystem, 
            SignalBus signalBus,
            BallPhysicsSystem ballPhysicsSystem,
            GameStateMachine stateMachine,
            BallLauncher ballLauncher,
            LaunchPointMarker marker,
            IAppLogger logger)
        {
            _aimingSystem = aimingSystem;
            _signalBus = signalBus;
            _ballPhysicsSystem = ballPhysicsSystem;
            _stateMachine = stateMachine;
            _ballLauncher = ballLauncher;
            _marker = marker;
            _logger = logger;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<FirstBallHitBottomSignal>(OnFirstBallHitBottom);
            _signalBus.Subscribe<RoundEndedSignal>(OnRoundEnded);
        }
        
        public void Tick()
        {
            if (!_stateMachine.CanShoot())
            {
                return;
            }

            if (Input.GetMouseButtonUp(0) || 
                (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended))
            {
                if (_aimingSystem.HasValidDirection)
                {
                    var direction = _aimingSystem.GetLaunchDirection();
                    _logger.Log($"Shooting: {direction}");
                    
                    _stateMachine.ChangeState(GameState.BallsFlying);
                    _ballLauncher.LaunchBalls(direction).Forget();
                }
            }
        }
        
        public void Dispose()
        {
            _signalBus.TryUnsubscribe<FirstBallHitBottomSignal>(OnFirstBallHitBottom);
            _signalBus.TryUnsubscribe<RoundEndedSignal>(OnRoundEnded);
            _aimingSystem.Dispose();
        }    
        
        private void OnRoundEnded()
        {
            _logger.Log("Round ended — preparing for next round");

            _marker.Show();
            _stateMachine.ChangeState(GameState.WaitingForShoot);
        }

        private void OnFirstBallHitBottom(FirstBallHitBottomSignal signal)
        {
            if (_ballPhysicsSystem.HasFirstLandPos())
            {
                _logger.Log("Double first land pos");
            }
            
            _ballLauncher.UpdateLaunchPosition(signal.Position);
            _marker.Show();
        }
    }
}