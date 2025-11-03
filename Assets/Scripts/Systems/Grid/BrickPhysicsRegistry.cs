using System.Collections.Generic;
using BricksAndBalls.Components.Physics;
using BricksAndBalls.Core.Models;
using UnityEngine;

namespace BricksAndBalls.Systems.Grid
{
    public class BrickPhysicsRegistry
    {
        private readonly GameplayData _gameplayData;
        private readonly Dictionary<int, BrickPhysicsComponent> _components = new();
        private Dictionary<int, int> _colliderInstanceIDToBrickID = new();

        public BrickPhysicsRegistry(
            GameplayData gameplayData)
        {
            _gameplayData = gameplayData;
        }
        public void Register(int id, BrickPhysicsComponent component)
        {
            _components[id] = component;
            var colliderID = component.Collider.GetInstanceID();
            _colliderInstanceIDToBrickID[colliderID] = id;
        }

        public void Unregister(int id)
        {
            var physics = _components[id];
            if (physics != null)
            {
                var colliderID = physics.Collider.GetInstanceID();
                _colliderInstanceIDToBrickID.Remove(colliderID);
            }
            _components.Remove(id);
        }

        public bool TryGetBrickFromCollider(Collider2D collider, out int id)
        {
            var colliderID = collider.GetInstanceID();
            return _colliderInstanceIDToBrickID.TryGetValue(colliderID, out id);
        }
        
        public bool TryGet(int id, out BrickPhysicsComponent component)
        {
            return _components.TryGetValue(id, out component);
        }

        public void RegisterBottomHit(int brickID)
        {
            _gameplayData.HasReachedBottom = true;
        }
    }
}