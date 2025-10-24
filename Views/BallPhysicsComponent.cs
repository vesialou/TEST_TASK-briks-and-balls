using BricksAndBalls.Systems.Ball;
using UnityEngine;
using Zenject;

namespace BricksAndBalls.Views
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public class BallPhysicsComponent : MonoBehaviour
    {
        [Inject] private BallPhysicsSystem _physicsSystem;

        public int BallID { get; set; }
        public Rigidbody2D Rigidbody { get; private set; }
        public Collider2D Collider { get; private set; }

        private void Awake()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            Collider = GetComponent<Collider2D>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            _physicsSystem.OnBallTriggerEnter(BallID, other);
        }
    }
}