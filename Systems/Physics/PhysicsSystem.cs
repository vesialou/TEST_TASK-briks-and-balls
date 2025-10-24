using System.Collections.Generic;
using BricksAndBalls.Core.Commands;
using BricksAndBalls.Core.Interfaces;
using BricksAndBalls.Core.Models;
using BricksAndBalls.Systems.Ball;
using BricksAndBalls.Views;
using UnityEngine;
using Zenject;

namespace BricksAndBalls.Systems.Physics
{
    public class PhysicsSystem : ITickable
    {
        private readonly Dictionary<int, BrickPhysicsComponent> _physicsComponents = new Dictionary<int, BrickPhysicsComponent>();
        private readonly Dictionary<int, BrickModel> _models = new Dictionary<int, BrickModel>();
        private readonly BrickCommandBuffer _commandBuffer;
        private readonly BallPhysicsSystem _ballPhysicsSystem;
        private readonly BallSpeedNormalizer _speedNormalizer;
        private readonly List<IBrickCommandListener> _listeners = new List<IBrickCommandListener>(4);
        private readonly IAppLogger _logger;

        public PhysicsSystem(
            BallPhysicsSystem ballPhysicsSystem,
            BallSpeedNormalizer speedNormalizer,
            IAppLogger logger)
        {
            _ballPhysicsSystem = ballPhysicsSystem;
            _speedNormalizer = speedNormalizer;
            _logger = logger;
            _commandBuffer = new BrickCommandBuffer(logger);
        }

        public void AddListener(IBrickCommandListener listener)
        {
            _listeners.Add(listener);
            _logger.Log($"PhysicsSystem: Listener added, total={_listeners.Count}");
        }

        public void RegisterBrick(int brickID, BrickPhysicsComponent physics, BrickModel model)
        {
            _physicsComponents[brickID] = physics;
            _models[brickID] = model;
        }

        public void UnregisterBrick(int brickID)
        {
            _physicsComponents.Remove(brickID);
            _models.Remove(brickID);
        }

        public void OnCollisionDetected(int brickID, Collider2D ballCollider)
        {
            if (!_models.TryGetValue(brickID, out BrickModel model))
            {
                return;
            }
            
            if (!model.IsAlive)
            {
                return;
            }

            BallView ballView = ballCollider.GetComponentInParent<BallView>();
            if (ballView != null)
            {
                BallPhysicsComponent ballPhysics = _ballPhysicsSystem.GetBallPhysics(ballView.BallID);
                if (ballPhysics != null)
                {
                    _speedNormalizer.NormalizeSpeed(ballPhysics.Rigidbody);
                }

                _ballPhysicsSystem.OnBallHitBrick(ballView.BallID);
            }

            int oldHP = model.CurrentHP;
            model.CurrentHP--;

            if (model.CurrentHP <= 0)
            {
                model.IsAlive = false;
                _physicsComponents[brickID].Collider.enabled = false;

                _commandBuffer.AddCommand(new BrickCommand
                {
                    Type = BrickCommand.CommandType.Death,
                    BrickID = brickID
                });

                _logger.Log($"Brick {brickID} destroyed");
            }
            else
            {
                _commandBuffer.AddCommand(new BrickCommand
                {
                    Type = BrickCommand.CommandType.HPChanged,
                    BrickID = brickID,
                    NewHP = model.CurrentHP
                });
            }
        }

        public void Tick()
        {
            ProcessVisualCommands();
        }

        private void ProcessVisualCommands()
        {
            var commands = _commandBuffer.GetCommands();
            if (commands.Length == 0) return;

            foreach (var command in commands)
            {
                switch (command.Type)
                {
                    case BrickCommand.CommandType.HPChanged:
                        foreach (var listener in _listeners)
                        {
                            listener.OnBrickHPChanged(command.BrickID, command.NewHP);
                        }
                        break;

                    case BrickCommand.CommandType.Death:
                        foreach (var listener in _listeners)
                        {
                            listener.OnBrickDeath(command.BrickID);
                        }
                        break;
                }
            }

            _commandBuffer.Clear();
        }
    }
}