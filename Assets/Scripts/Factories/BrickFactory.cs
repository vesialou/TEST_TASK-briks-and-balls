using BricksAndBalls.Components.Physics;
using BricksAndBalls.Configs;
using BricksAndBalls.Core.Interfaces;
using BricksAndBalls.Core.Models;
using BricksAndBalls.Systems.Grid;
using BricksAndBalls.Systems.Playfield;
using BricksAndBalls.Views;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace BricksAndBalls.Factories
{
    public class BrickFactory : IFactory<BrickData, Transform, GameObject>
    {
        private readonly BrickPool _pool;
        private readonly DiContainer _container;
        private readonly GridSystem _gridSystem;
        private readonly BrickViewRegistry _viewRegistry;
        private readonly BrickPhysicsRegistry _physicsRegistry;
        private readonly PlayfieldManager _playfieldManager;
        private readonly GameplayData _gameplayData;
        private readonly IAppLogger _logger;

        [Inject] private GameplayObjectsConfig _gameplayObjectsConfig;

        public BrickFactory(
            BrickPool pool,
            DiContainer container,
            GridSystem gridSystem,
            BrickViewRegistry viewRegistry,
            BrickPhysicsRegistry physicsRegistry,
            PlayfieldManager playfieldManager,
            GameplayData gameplayData,
            IAppLogger logger)
        {
            _pool = pool;
            _container = container;
            _gridSystem = gridSystem;
            _viewRegistry = viewRegistry;
            _physicsRegistry = physicsRegistry;
            _playfieldManager = playfieldManager;
            _gameplayData = gameplayData;
            _logger = logger;
        }

        public GameObject Create(BrickData data, Transform parent)
        {
            var view = _pool.Get();
            var gameObject = view.gameObject;
            
            var physics = gameObject.GetComponentInChildren<BrickPhysicsComponent>();

            if (view == null || physics == null)
            {
                _logger.LogError("BrickFactory: Missing components");
                return null;
            }

            if (data.HP <= 0)
            {
                _logger.LogError("BrickFactory: Missing HP");
                return null;
            }

            _container.Inject(physics);

            var brickID = _gameplayData.BrickCount;
            
            _gameplayData.Bricks[brickID] = new BrickModel
            {
                ID = brickID,
                GridPosition = data.GridPosition,
                MaxHP = data.HP,
                CurrentHP = data.HP,
                IsAlive = true,
                Type = data.Type
            };

            _gameplayData.BrickCount++;
            _gameplayData.AliveBrickCount++;

            physics.BrickID = brickID;
            view.BrickID = brickID;
            view.Initialize(data.HP);

            _gridSystem.Register(brickID, data.GridPosition);
            _viewRegistry.Register(brickID, view);
            _physicsRegistry.Register(brickID, physics);

            var worldPosition = _playfieldManager.GridToWorld(data.GridPosition);
            gameObject.transform.position = worldPosition;

            var scale = _playfieldManager.CellSize * 0.95f;
            physics.transform.localScale = new Vector3(scale, scale, 1f);
            
            return gameObject;
        }
        
        public void Release(int brickID)
        {
            if (!_viewRegistry.TryGet(brickID, out var view))
            {
                _logger.LogWarning($"BrickFactory: Cannot release brick {brickID}, not found in registry");
                return;
            }

            view.PlayDeathAnimation();
            DelayedReleaseAsync(brickID, view).Forget();
        }

        private async UniTaskVoid DelayedReleaseAsync(int brickID, BrickView view)
        {
            await UniTask.Delay(200);

            _gridSystem.Unregister(brickID);
            _viewRegistry.Unregister(brickID);
            _physicsRegistry.Unregister(brickID);

            _pool.Release(view);
            _logger.Log($"Brick returned to pool: ID={brickID}");
        }
    }
}