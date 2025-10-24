using BricksAndBalls.Systems.Ball;
using BricksAndBalls.Systems.Grid;
using UnityEngine;
using Zenject;

namespace BricksAndBalls.Systems.Playfield
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class WallBounds : MonoBehaviour
    {
        public enum WallType
        {
            Top,
            Bottom,
            Left,
            Right,
            Background
        }

        [SerializeField] private BoxCollider2D _collider;
        [SerializeField] private WallType _wallType;
        
        private BallPhysicsSystem _ballPhysicsSystem;
        private BrickPhysicsRegistry _brickPhysicsRegistry;
        
        [Inject] 
        public void Construct(BallPhysicsSystem  ballPhysicsSystem, BrickPhysicsRegistry brickPhysicsRegistry)
        {
            _ballPhysicsSystem = ballPhysicsSystem;
            _brickPhysicsRegistry = brickPhysicsRegistry;
        }

        public WallType Type => _wallType;
        public BoxCollider2D Collider => _collider;

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (_wallType != WallType.Bottom)
            {
                return;
            }

            if (_ballPhysicsSystem.TryGetBallIDFromCollider(other.collider, out var ballID))
            {
                _ballPhysicsSystem.RegisterBottomHit(ballID, other.transform.position);
            }

            if (_brickPhysicsRegistry.TryGetBrickFromCollider(other.collider, out var brickID))
            {
                _brickPhysicsRegistry.RegisterBottomHit(brickID);
            }
        }
    }
}