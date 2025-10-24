using BricksAndBalls.Core.Interfaces;
using BricksAndBalls.Core.Models;
using BricksAndBalls.Core.Signals;
using BricksAndBalls.Views;
using UnityEngine;
using Zenject;

namespace BricksAndBalls.Systems.Ball
{//asssssssssssssssssssssssssssssssssssssssssss
    public class BallPhysicsSystem
    {
        private readonly SignalBus _signalBus;
        private readonly IAppLogger _logger;

        private BallPhysicsComponent[] _physicsComponents;
        private BallModel[] _models;
        private int _registeredCount;
        private int _activeBallCount;

        private System.Collections.Generic.Dictionary<int, int> _colliderInstanceIDToBallID;

        public BallPhysicsSystem(SignalBus signalBus, IAppLogger logger)
        {
            _signalBus = signalBus;
            _logger = logger;

            _physicsComponents = new BallPhysicsComponent[100];
            _models = new BallModel[100];
            _registeredCount = 0;

            _colliderInstanceIDToBallID = new System.Collections.Generic.Dictionary<int, int>(100);
        }

        public void RegisterBall(int ballID, BallPhysicsComponent physics, BallModel model)
        {
            if (ballID >= _physicsComponents.Length)
            {
                int newSize = Mathf.NextPowerOfTwo(ballID + 1);
                System.Array.Resize(ref _physicsComponents, newSize);
                System.Array.Resize(ref _models, newSize);
            }

            _physicsComponents[ballID] = physics;
            _models[ballID] = model;
            _registeredCount = Mathf.Max(_registeredCount, ballID + 1);

            int colliderID = physics.Collider.GetInstanceID();
            _colliderInstanceIDToBallID[colliderID] = ballID;
        }

        public bool TryGetBallIDFromCollider(Collider2D collider, out int ballID)
        {
            int colliderID = collider.GetInstanceID();
            return _colliderInstanceIDToBallID.TryGetValue(colliderID, out ballID);
        }

        public void StopBall(int ballID)
        {
            if (ballID >= _registeredCount) return;

            BallModel model = _models[ballID];
            BallPhysicsComponent physics = _physicsComponents[ballID];

            if (model == null || physics == null) return;

            Rigidbody2D rb = physics.Rigidbody;
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;

            model.IsActive = false;
            model.HasReturned = true;

            _activeBallCount--;

            _logger.Log($"Ball {ballID} stopped. Active: {_activeBallCount}");

            if (_activeBallCount == 0)
            {
                _logger.Log("All balls returned");
                _signalBus.Fire<RoundEndedSignal>();
            }
        }

        public void IncrementActiveBallCount()
        {
            _activeBallCount++;
        }

        public Rigidbody2D GetBallRigidbody(int ballID)
        {
            if (ballID >= _registeredCount) return null;
            BallPhysicsComponent physics = _physicsComponents[ballID];
            return physics != null ? physics.Rigidbody : null;
        }
    }
}