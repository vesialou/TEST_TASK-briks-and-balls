using BricksAndBalls.Configs;
using BricksAndBalls.Core.Interfaces;
using BricksAndBalls.Core.Models;
using BricksAndBalls.Core.Signals;
using BricksAndBalls.Systems.Ball;
using BricksAndBalls.Systems.Grid;
using BricksAndBalls.Views;
using UnityEngine;
using Zenject;

namespace BricksAndBalls.Systems.Physics
{
    public class BrickPhysicsSystem : ILateTickable
    {
        private readonly BallPhysicsSystem _ballPhysicsSystem;
        private readonly GridSystem _gridSystem;
        private readonly GameConfig _gameConfig;
        private readonly SignalBus _signalBus;
        private readonly IAppLogger _logger;

        private BrickPhysicsComponent[] _physicsComponents;
        private BrickModel[] _models;
        private BrickView[] _views;
        private int _registeredCount;

        private BrickHPUpdate[] _hpUpdates;
        private int _hpUpdateCount;

        public BrickPhysicsSystem(
            BallPhysicsSystem ballPhysicsSystem,
            GridSystem gridSystem,
            GameConfig gameConfig,
            SignalBus signalBus,
            IAppLogger logger)
        {
            _ballPhysicsSystem = ballPhysicsSystem;
            _gridSystem = gridSystem;
            _gameConfig = gameConfig;
            _signalBus = signalBus;
            _logger = logger;

            _physicsComponents = new BrickPhysicsComponent[300];
            _models = new BrickModel[300];
            _views = new BrickView[300];
            _registeredCount = 0;

            _hpUpdates = new BrickHPUpdate[300];
            _hpUpdateCount = 0;
        }

        public void RegisterBrick(int brickID, BrickPhysicsComponent physics, BrickModel model, BrickView view)
        {
            if (brickID >= _physicsComponents.Length)
            {
                int newSize = Mathf.NextPowerOfTwo(brickID + 1);
                System.Array.Resize(ref _physicsComponents, newSize);
                System.Array.Resize(ref _models, newSize);
                System.Array.Resize(ref _views, newSize);
                System.Array.Resize(ref _hpUpdates, newSize);
            }

            _physicsComponents[brickID] = physics;
            _models[brickID] = model;
            _views[brickID] = view;
            _registeredCount = Mathf.Max(_registeredCount, brickID + 1);
        }

        public void UnregisterBrick(int brickID)
        {
            if (brickID >= _registeredCount) return;

            _physicsComponents[brickID] = null;
            _models[brickID] = null;
            _views[brickID] = null;
        }

        public void OnCollisionDetected(int brickID, Collider2D ballCollider)
        {
            if (brickID >= _registeredCount)
            {
                return;
            }

            BrickModel model = _models[brickID];
            if (model == null || !model.IsAlive)
            {
                return;
            }

            if (_ballPhysicsSystem.TryGetBallIDFromCollider(ballCollider, out int ballID))
            {
                Rigidbody2D ballRb = _ballPhysicsSystem.GetBallRigidbody(ballID);
                if (ballRb != null)
                {
                    NormalizeBallSpeed(ballRb);
                }
            }

            model.CurrentHP--;

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
                BrickPhysicsComponent physics = _physicsComponents[brickID];
                if (physics != null)
                {
                    physics.Collider.enabled = false;
                }
            }
        }

        public void LateTick()
        {
            if (_hpUpdateCount == 0) return;

            for (int i = 0; i < _hpUpdateCount; i++)
            {
                BrickHPUpdate update = _hpUpdates[i];
                BrickView view = _views[update.BrickID];
                
                if (view == null) continue;

                if (update.IsDead)
                {
                    view.PlayDeathAnimation();
                    
                    BrickModel model = _models[update.BrickID];
                    if (model != null)
                    {
                        _gridSystem.OnBrickDestroyed(update.BrickID, model.GridPosition);
                        _signalBus.Fire(new BrickDestroyedSignal { GridPosition = model.GridPosition });
                    }
                }
                else
                {
                    view.UpdateHP(update.NewHP);
                }
            }

            _hpUpdateCount = 0;
        }

        private void NormalizeBallSpeed(Rigidbody2D rigidbody)
        {
            if (rigidbody.velocity.sqrMagnitude < 0.01f) return;

            Vector2 normalizedVelocity = rigidbody.velocity.normalized * _gameConfig.BallSpeed;
            rigidbody.velocity = normalizedVelocity;
        }
    }
}