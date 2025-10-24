using BricksAndBalls.Systems.Physics;
using UnityEngine;
using Zenject;

namespace BricksAndBalls.Components.Physics
{
    [RequireComponent(typeof(Collider2D))]
    public class BrickPhysicsComponent : MonoBehaviour
    {
        [SerializeField] private Collider2D _collider2D;
        [Inject] private BrickPhysicsSystem _physicsSystem;

        public int BrickID { get; set; }
        public Collider2D Collider => _collider2D;

        private void OnCollisionEnter2D(Collision2D other)
        {
            _physicsSystem.OnCollisionDetected(BrickID, other.collider);
        }
    }
}