using BricksAndBalls.Systems.Ball;
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
            Right
        }

        [SerializeField] private WallType _wallType;
        [Inject] private BallPhysicsSystem _ballPhysicsSystem;

        public WallType Type => _wallType;
        public BoxCollider2D Collider { get; private set; }

        private void Awake()
        {
            Collider = GetComponent<BoxCollider2D>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_wallType != WallType.Bottom) return;
            if (!other.CompareTag("Ball")) return;

            if (_ballPhysicsSystem.TryGetBallIDFromCollider(other, out int ballID))
            {
                _ballPhysicsSystem.StopBall(ballID);
            }
        }
    }
}