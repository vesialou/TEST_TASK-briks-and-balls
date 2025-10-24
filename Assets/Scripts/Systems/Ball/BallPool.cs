using BricksAndBalls.Components.Physics;
using BricksAndBalls.Core.Pool;
using BricksAndBalls.Views;
using UnityEngine;
using Zenject;

namespace BricksAndBalls.Systems.Ball
{
    public class BallPool
    {
        private ObjectPool<BallView> _pool;
        private readonly DiContainer _container;
        private readonly Transform _poolParent;

        public int CountActive => _pool.CountActive;
        public int CountInactive => _pool.CountInactive;

        public BallPool(DiContainer container, GameObject ballPrefab, Transform poolParent = null)
        {
            _container = container;
            _poolParent = poolParent;

            var prefabView = ballPrefab.GetComponentInChildren<BallView>();
            
            _pool = new ObjectPool<BallView>(
                prefab: prefabView,
                parent: _poolParent,
                initialSize: 15,
                maxSize: 50,
                onGet: OnBallGet,
                onRelease: OnBallRelease
            );
        }

        public BallView Get()
        {
            return _pool.Get();
        }

        public void Release(BallView ballView)
        {
            _pool.Release(ballView);
        }

        private void OnBallGet(BallView ball)
        {
            ball.transform.parent = null;
            var physics = ball.GetComponent<BallPhysicsComponent>();
            if (physics != null)
            {
                _container.Inject(physics);
            }
        }

        private void OnBallRelease(BallView ball)
        {
            ball.transform.parent = _poolParent;
            ball.transform.position = Vector3.zero;
            ball.transform.localScale = Vector3.one;
            ball.transform.rotation = Quaternion.identity;
            
            var rb = ball.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }
        }

        public void Clear()
        {
            _pool.Clear();
        }
    }
}