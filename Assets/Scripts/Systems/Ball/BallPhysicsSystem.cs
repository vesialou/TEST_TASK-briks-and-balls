using System;
using System.Collections.Generic;
using BricksAndBalls.Components.Physics;
using BricksAndBalls.Core.Interfaces;
using BricksAndBalls.Core.Models;
using BricksAndBalls.Core.Signals;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace BricksAndBalls.Systems.Ball
{
    public class BallPhysicsSystem
    {
        private readonly SignalBus _signalBus;
        private readonly IAppLogger _logger;

        private BallPhysicsComponent[] _physicsComponents;
        private BallModel[] _models;
        private int _registeredCount;
        private int _activeBallCount;
        private Vector3? _firstHitPosition;

        private Dictionary<int, int> _colliderInstanceIDToBallID;

        public BallPhysicsSystem(
            SignalBus signalBus, 
            IAppLogger logger)
        {
            _signalBus = signalBus;
            _logger = logger;

            _physicsComponents = new BallPhysicsComponent[100];
            _models = new BallModel[100];
            _registeredCount = 0;

            _colliderInstanceIDToBallID = new Dictionary<int, int>(100);
        }

        public void RegisterBall(int ballID, BallPhysicsComponent physics, BallModel model)
        {
            if (ballID >= _physicsComponents.Length)
            {
                var newSize = Mathf.NextPowerOfTwo(ballID + 1);
                Array.Resize(ref _physicsComponents, newSize);
                Array.Resize(ref _models, newSize);
            }

            _physicsComponents[ballID] = physics;
            _models[ballID] = model;
            _registeredCount = Mathf.Max(_registeredCount, ballID + 1);

            var colliderID = physics.Collider.GetInstanceID();
            _colliderInstanceIDToBallID[colliderID] = ballID;
        }

        public void UnregisterBall(int ballID)
        {
            if (ballID >= _registeredCount)
            {
                return;
            }

            var physics = _physicsComponents[ballID];
            if (physics != null)
            {
                var colliderID = physics.Collider.GetInstanceID();
                _colliderInstanceIDToBallID.Remove(colliderID);
            }

            _physicsComponents[ballID] = null;
            _models[ballID] = null;
        }

        public bool TryGetBallIDFromCollider(Collider2D collider, out int ballID)
        {
            var colliderID = collider.GetInstanceID();
            return _colliderInstanceIDToBallID.TryGetValue(colliderID, out ballID);
        }

        public void RegisterBottomHit(int ballID, Vector3 position)
        {
            if (!_firstHitPosition.HasValue)
            {
                _firstHitPosition = position;
                _logger.Log($"Ball {ballID} — first hit at bottom ({position}), IMMEDIATELY returning to pool");

                _signalBus.Fire(new FirstBallHitBottomSignal(position));

                ReturnBallToPool(ballID);
                return;
            }

            var rb = GetBallRigidbody(ballID);
            if (rb == null)
            {
                return;
            }

            var target = _firstHitPosition.Value;
            MoveBallToTargetAndReturn(
                rb,
                target,
                ballID);
        }

        public void ClearReturnPosition()
        {
            _firstHitPosition = null;
        }

        private void MoveBallToTargetAndReturn(Rigidbody2D rb, Vector3 target, int ballID)
        {
            var dist = Vector3.Distance(rb.position, target);
            var duration = Mathf.Clamp(
                dist / 10f,
                0.25f,
                0.6f);

            DisabledState(ballID);
            rb.DOMove(target, duration).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                _logger.Log($"Ball {ballID} reached target, returning to pool");
                ReturnBallToPool(ballID);
            });
        }

        private void ReturnBallToPool(int ballID)
        {
            if (ballID >= _registeredCount)
            {
                return;
            }

            var model = _models[ballID];
            var physics = _physicsComponents[ballID];

            if (model == null || physics == null)
            {
                return;
            }

            ActiveState(physics);

            model.IsActive = false;
            model.HasReturned = true;

            _activeBallCount--;
            _logger.Log($"Ball {ballID} stopped. Active left: {_activeBallCount}");

            _signalBus.Fire(new BallReturnedToPoolSignal { BallID = ballID });

            if (_activeBallCount == 0)
            {
                _logger.Log("All balls stopped — round ended");
                _signalBus.Fire<RoundEndedSignal>();
                ClearReturnPosition();
            }
        }

        public void IncrementActiveBallCount()
        {
            _activeBallCount++;
        }

        public Rigidbody2D GetBallRigidbody(int ballID)
        {
            if (ballID >= _registeredCount)
            {
                return null;
            }

            var physics = _physicsComponents[ballID];
            if (physics != null)
            {
                return physics.Rigidbody;
            }

            return null;
        }

        private void DisabledState(int ballID)
        {
            var physics = _physicsComponents[ballID];
            var rb = physics.Rigidbody;
            physics.Collider.enabled = false;
            rb.angularVelocity = 0f;
            rb.isKinematic = true;
            rb.DOKill();
            rb.velocity = Vector2.zero;
        }

        private void ActiveState(BallPhysicsComponent physics)
        {
            var rb = physics.Rigidbody;
            physics.Collider.enabled = true;
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.isKinematic = false;
            rb.DOKill();
        }

        public bool HasFirstLandPos()
        {
            return _firstHitPosition.HasValue;
        }
    }
}