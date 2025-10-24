using BricksAndBalls.Systems.Physics;
using UnityEngine;
using Zenject;

namespace BricksAndBalls.Views
{
    [RequireComponent(typeof(Collider2D))]
    public class BrickPhysicsComponent : MonoBehaviour
    {
        [Inject] private BrickPhysicsSystem _physicsSystem;

        public int BrickID { get; set; }
        public Collider2D Collider { get; private set; }

        private void Awake()
        {
            Collider = GetComponent<Collider2D>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Ball"))
            {
                _physicsSystem.OnCollisionDetected(BrickID, other);
            }
        }
    }
}