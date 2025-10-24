using BricksAndBalls.Systems.Ball;
using UnityEngine;
using Zenject;

namespace BricksAndBalls.Components.Physics
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public class BallPhysicsComponent : MonoBehaviour
    {
        [Inject] private BallPhysicsSystem _physicsSystem;
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private Collider2D _collider;

        public int BallID { get; set; }
        public Rigidbody2D Rigidbody => _rigidbody;
        public Collider2D Collider => _collider;
    }
}
