using BricksAndBalls.Components.Physics;
using BricksAndBalls.Core.Pool;
using BricksAndBalls.Views;
using UnityEngine;
using Zenject;

namespace BricksAndBalls.Systems.Grid
{
    public class BrickPool
    {
        private ObjectPool<BrickView> _pool;
        private readonly DiContainer _container;
        private readonly Transform _poolParent;

        public int CountActive => _pool.CountActive;
        public int CountInactive => _pool.CountInactive;

        public BrickPool(DiContainer container, GameObject brickPrefab, Transform poolParent = null)
        {
            _container = container;
            _poolParent = poolParent;

            var prefabView = brickPrefab.GetComponentInChildren<BrickView>();
            
            _pool = new ObjectPool<BrickView>(
                prefab: prefabView,
                parent: _poolParent,
                initialSize: 50,
                maxSize: 200,
                onGet: OnBrickGet,
                onRelease: OnBrickRelease
            );
        }

        public BrickView Get()
        {
            return _pool.Get();
        }

        public void Release(BrickView brickView)
        {
            _pool.Release(brickView);
        }

        private void OnBrickGet(BrickView brick)
        {
            brick.transform.SetParent(null);
            var physics = brick.GetComponent<BrickPhysicsComponent>();
            if (physics != null)
            {
                _container.Inject(physics);
            }

            brick.ResetVisuals();
        }

        private void OnBrickRelease(BrickView brick)
        {
            brick.transform.SetParent(_poolParent);
            brick.transform.position = Vector3.zero;
            brick.ResetVisuals();
        }

        public void Clear()
        {
            _pool.Clear();
        }
    }
}