using BricksAndBalls.Core.Interfaces;
using BricksAndBalls.Core.Models;
using BricksAndBalls.Systems.Ball;
using BricksAndBalls.Views;
using UnityEngine;
using Zenject;

namespace BricksAndBalls.Factories
{
    public class BallFactory : IFactory<Vector3, BallInstance>
    {
        private readonly DiContainer _container;
        private readonly BallVisualSystem _visualSystem;
        private readonly BallPhysicsSystem _physicsSystem;
        private readonly IAppLogger _logger;

        [Inject]
        private GameObject _ballPrefab;

        private int _nextID = 0;

        public BallFactory(
            DiContainer container,
            BallVisualSystem visualSystem,
            BallPhysicsSystem physicsSystem,
            IAppLogger logger)
        {
            _container = container;
            _visualSystem = visualSystem;
            _physicsSystem = physicsSystem;
            _logger = logger;
        }

        public BallInstance Create(Vector3 position)
        {
            GameObject prefab = CreatePrefabInstance(position);

            BallView view = prefab.GetComponentInChildren<BallView>();
            BallPhysicsComponent physics = prefab.GetComponentInChildren<BallPhysicsComponent>();

            if (!ValidateComponents(view, physics))
                return null;

            _container.Inject(physics);

            BallModel model = InitializeModel();

            AssignIDs(view, physics, model.ID);
            RegisterComponents(model, view, physics);

            _logger.Log($"Ball created: ID={model.ID}");

            return new BallInstance
            {
                Model = model,
                View = view,
                Physics = physics
            };
        }

        private GameObject CreatePrefabInstance(Vector3 position)
        {
            GameObject instance = Object.Instantiate(_ballPrefab);
            instance.transform.position = position;
            return instance;
        }

        private BallModel InitializeModel()
        {
            return new BallModel
            {
                ID = _nextID++,
                IsActive = false,
                HasReturned = false
            };
        }

        private void AssignIDs(BallView view, BallPhysicsComponent physics, int id)
        {
            physics.BallID = id;
            view.BallID = id;
        }

        private void RegisterComponents(BallModel model, BallView view, BallPhysicsComponent physics)
        {
            _visualSystem.RegisterBall(model.ID, view);
            _physicsSystem.RegisterBall(model.ID, physics, model);
        }

        private bool ValidateComponents(BallView view, BallPhysicsComponent physics)
        {
            if (view == null || physics == null)
            {
                _logger.LogError("BallFactory: Prefab missing required components");
                return false;
            }

            return true;
        }
    }
}