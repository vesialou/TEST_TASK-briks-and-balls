using BricksAndBalls.Core.Interfaces;
using BricksAndBalls.Core.Models;
using BricksAndBalls.Core.Signals;
using BricksAndBalls.Systems.Ball;
using BricksAndBalls.Systems.Grid;
using UnityEngine;
using Zenject;

namespace BricksAndBalls.Systems.Physics
{
    public class BrickPhysicsSystem : ILateTickable
    {
        private readonly BallPhysicsSystem _ballPhysicsSystem;
        private readonly BrickViewRegistry _viewRegistry;
        private readonly BrickPhysicsRegistry _physicsRegistry;
        private readonly GameplayData _gameplayData;
        private readonly IGameConfig _gameConfig;
        private readonly SignalBus _signalBus;
        private readonly IAppLogger _logger;

        private BrickHPUpdate[] _hpUpdates;
        private int _hpUpdateCount;
        
        private int _frameHits;
        private int _frameDestroys;

        public BrickPhysicsSystem(
            BallPhysicsSystem ballPhysicsSystem,
            BrickViewRegistry viewRegistry,
            BrickPhysicsRegistry physicsRegistry,
            GameplayData gameplayData,
            IGameConfig gameConfig,
            SignalBus signalBus,
            IAppLogger logger)
        {
            _ballPhysicsSystem = ballPhysicsSystem;
            _viewRegistry = viewRegistry;
            _physicsRegistry = physicsRegistry;
            _gameplayData = gameplayData;
            _gameConfig = gameConfig;
            _signalBus = signalBus;
            _logger = logger;

            _hpUpdates = new BrickHPUpdate[300];
            _hpUpdateCount = 0;
            _frameHits = 0;
            _frameDestroys = 0;
        }

        public void OnCollisionDetected(int brickID, Collider2D ballCollider)
        {
            if (brickID >= _gameplayData.BrickCount)
            {
                return;
            }

            var model = _gameplayData.Bricks[brickID];
            if (!model.IsAlive)
            {
                return;
            }

            if (_ballPhysicsSystem.TryGetBallIDFromCollider(ballCollider, out var ballID))
            {
                var ballRb = _ballPhysicsSystem.GetBallRigidbody(ballID);
                if (ballRb != null)
                {
                    NormalizeBallSpeed(ballRb);
                }
            }
            else
            {
                return;
            }

            model.CurrentHP--;
            _frameHits++;

            if (_hpUpdateCount >= _hpUpdates.Length)
            {
                _logger.LogError("HP updates buffer overflow!");
                return;
            }

            _hpUpdates[_hpUpdateCount++] = new BrickHPUpdate
            {
                BrickID = brickID,
                NewHP = model.CurrentHP,
                IsDead = model.CurrentHP <= 0
            };

            if (model.CurrentHP <= 0)
            {
                model.IsAlive = false;
                _frameDestroys++;
                
                if (_physicsRegistry.TryGet(brickID, out var physics))
                {
                    physics.Collider.enabled = false;
                }
            }
        }

        public void LateTick()
        {
            if (_hpUpdateCount == 0)
            {
                return;
            }

            for (var i = 0; i < _hpUpdateCount; i++)
            {
                var update = _hpUpdates[i];

                if (!_viewRegistry.TryGet(update.BrickID, out var view))
                {
                    continue;
                }

                if (update.IsDead)
                {
                    var model = _gameplayData.Bricks[update.BrickID];
            
                    _signalBus.Fire(new BrickDestroyedSignal 
                    { 
                        GridPosition = model.GridPosition 
                    });

                    _signalBus.Fire(new BrickDestroyedForPoolSignal 
                    { 
                        BrickID = update.BrickID 
                    });
                }
                else
                {
                    view.UpdateHP(update.NewHP);
                }
            }

            if (_frameHits > 0)
            {
                _gameplayData.AccumulatedHitScore += _frameHits * _gameConfig.HitBrickReward;
                _frameHits = 0;
            }
            
            if (_frameDestroys > 0)
            {
                _gameplayData.AccumulatedDestroyScore += _frameDestroys * _gameConfig.DestroyBrickReward;
                _gameplayData.AliveBrickCount -= _frameDestroys;
                _frameDestroys = 0;
            }

            _hpUpdateCount = 0;
        }

        private void NormalizeBallSpeed(Rigidbody2D rigidbody)
        {
            if (rigidbody.velocity.sqrMagnitude < 0.01f)
            {
                return;
            }

            var normalizedVelocity = rigidbody.velocity.normalized * _gameConfig.BallSpeed;
            rigidbody.velocity = normalizedVelocity;
        }
    }
}