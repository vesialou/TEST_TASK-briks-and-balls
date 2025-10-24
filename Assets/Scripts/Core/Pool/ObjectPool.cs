using System;
using System.Collections.Generic;
using UnityEngine;

namespace BricksAndBalls.Core.Pool
{
    public class ObjectPool<T> where T : Component
    {
        private readonly Stack<T> _pool = new();
        private readonly T _prefab;
        private readonly Transform _parent;
        private readonly int _initialSize;
        private readonly int _maxSize;
        private readonly Action<T> _onGet;
        private readonly Action<T> _onRelease;

        public int CountActive => _totalCreated - _pool.Count;
        public int CountInactive => _pool.Count;
        
        private int _totalCreated = 0;

        public ObjectPool(
            T prefab, 
            Transform parent = null,
            int initialSize = 10,
            int maxSize = 100,
            Action<T> onGet = null,
            Action<T> onRelease = null)
        {
            _prefab = prefab;
            _parent = parent;
            _initialSize = initialSize;
            _maxSize = maxSize;
            _onGet = onGet;
            _onRelease = onRelease;

            Prewarm();
        }

        private void Prewarm()
        {
            for (var i = 0; i < _initialSize; i++)
            {
                var obj = CreateNew();
                Release(obj);
            }
        }

        public T Get()
        {
            T obj;

            if (_pool.Count > 0)
            {
                obj = _pool.Pop();
            }
            else
            {
                obj = CreateNew();
            }

            obj.gameObject.SetActive(true);
            _onGet?.Invoke(obj);
            
            return obj;
        }

        public void Release(T obj)
        {
            if (_pool.Count >= _maxSize)
            {
                UnityEngine.Object.Destroy(obj.gameObject);
                _totalCreated--;
                return;
            }

            _onRelease?.Invoke(obj);
            obj.gameObject.SetActive(false);
            _pool.Push(obj);
        }

        private T CreateNew()
        {
            var instance = UnityEngine.Object.Instantiate(_prefab, _parent);
            instance.gameObject.SetActive(false);
            _totalCreated++;
            return instance;
        }

        public void Clear()
        {
            while (_pool.Count > 0)
            {
                var obj = _pool.Pop();
                UnityEngine.Object.Destroy(obj.gameObject);
            }
            _totalCreated = 0;
        }
    }
}