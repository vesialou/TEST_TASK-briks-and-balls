using BricksAndBalls.Core.Interfaces;
using BricksAndBalls.Core.Models;
using BricksAndBalls.Systems.Grid;
using BricksAndBalls.Systems.Physics;
using BricksAndBalls.Systems.Playfield;
using BricksAndBalls.Views;
using Game.Configs;
using UnityEngine;
using Zenject;

namespace BricksAndBalls.Factories
{
    public class BrickFactory : IFactory<BrickData, Transform, BrickInstance>
    {
        private readonly DiContainer _container;
        private readonly GridSystem _gridSystem;
        private readonly BrickPhysicsSystem _brickPhysicsSystem;
        private readonly PlayfieldManager _playfieldManager;
        private readonly IAppLogger _logger;

        [Inject] private BrickFactoryConfig _factoryConfig;

        private int _nextID = 0;

        public BrickFactory(
            DiContainer container,
            GridSystem gridSystem,
            BrickPhysicsSystem brickPhysicsSystem,
            PlayfieldManager playfieldManager,
            IAppLogger logger)
        {
            _container = container;
            _gridSystem = gridSystem;
            _brickPhysicsSystem = brickPhysicsSystem;
            _playfieldManager = playfieldManager;
            _logger = logger;
        }

        public BrickInstance Create(BrickData data, Transform parent)
        {
            GameObject prefab = Object.Instantiate(_factoryConfig.BrickPrefab, parent);
            
            BrickView view = prefab.GetComponentInChildren<BrickView>();
            BrickPhysicsComponent physics = prefab.GetComponentInChildren<BrickPhysicsComponent>();

            if (view == null || physics == null)
            {
                _logger.LogError("BrickFactory: Missing components");
                return null;
            }

            _container.Inject(physics);

            BrickModel model = new BrickModel
            {
                ID = _nextID++,
                GridPosition = data.GridPosition,
                MaxHP = data.HP,
                CurrentHP = data.HP,
                IsAlive = true,
                Type = data.Type
            };

            physics.BrickID = model.ID;
            view.BrickID = model.ID;

            _gridSystem.RegisterBrick(model.ID, view, data.GridPosition);
            _brickPhysicsSystem.RegisterBrick(model.ID, physics, model, view);

            Vector3 worldPosition = _playfieldManager.GridToWorld(data.GridPosition);
            prefab.transform.position = worldPosition;

            float scale = _playfieldManager.CellSize * 0.95f;
            physics.transform.localScale = new Vector3(scale, scale, 1f);

            return new BrickInstance
            {
                Model = model,
                View = view,
                Physics = physics
            };
        }
    }
}