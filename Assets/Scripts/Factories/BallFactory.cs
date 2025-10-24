using BricksAndBalls.Components.Physics;
using BricksAndBalls.Configs;
using BricksAndBalls.Core.Interfaces;
using BricksAndBalls.Core.Models;
using BricksAndBalls.Systems.Ball;
using Zenject;

namespace BricksAndBalls.Factories
{
    public class BallFactory : IFactory<BallData, BallInstance>
    {
        private readonly BallPool _pool;
        private readonly DiContainer _container;
        private readonly BallVisualSystem _visualSystem;
        private readonly BallPhysicsSystem _physicsSystem;
        private readonly IAppLogger _logger;

        [Inject] private GameplayObjectsConfig _gameplayObjectsConfig;

        private int _nextID = 0;

        public BallFactory(
            BallPool pool,
            DiContainer container,
            BallVisualSystem visualSystem,
            BallPhysicsSystem physicsSystem,
            IAppLogger logger)
        {
            _pool = pool;
            _container = container;
            _visualSystem = visualSystem;
            _physicsSystem = physicsSystem;
            _logger = logger;
        }

        public BallInstance Create(BallData data)
        {;
            var view = _pool.Get();
            var viewGameObject = view.gameObject;
            var physics = viewGameObject.GetComponent<BallPhysicsComponent>();

            if (view == null || physics == null)
            {
                _logger.LogError("BallFactory: Prefab missing required components!");
                return default(BallInstance);;
            }

            var model = new BallModel
            {
                ID = _nextID++,
                IsActive = false,
                HasReturned = false
            };

            physics.BallID = model.ID;
            view.BallID = model.ID;

            _visualSystem.RegisterBall(model.ID, view);
            _physicsSystem.RegisterBall(model.ID, physics, model); 

            viewGameObject.transform.position = data.Position;

            _logger.Log($"Ball created: ID={model.ID}");

            return new BallInstance
            {
                Model = model,
                View = view,
                Physics = physics
            };
        }
        
        public void Release(int ballID)
        {
            if (!_visualSystem.TryGetBallView(ballID, out var view))
            {
                _logger.LogWarning($"BallFactory: Cannot release ball {ballID}, view not found in visual system!");
                return;
            }
            
            _physicsSystem.UnregisterBall(ballID);
            _visualSystem.UnregisterBall(ballID);
            _pool.Release(view);
    
            _logger.Log($"Ball returned to pool: ID={ballID}");
        }
    }


}