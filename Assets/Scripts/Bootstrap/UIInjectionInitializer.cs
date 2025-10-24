using BricksAndBalls.Core.Interfaces;
using BricksAndBalls.UI;
using Zenject;

namespace BricksAndBalls.Bootstrap
{
    public class UIInjectionInitializer : IInitializable
    {
        private readonly DiContainer _container;
        private readonly GameHUD _gameHUD;
        private readonly IAppLogger _logger;

        public UIInjectionInitializer(
            DiContainer container,
            GameHUD gameHUD,
            IAppLogger logger)
        {
            _container = container;
            _gameHUD = gameHUD;
            _logger = logger;
        }

        public void Initialize()
        {
            _container.Inject(_gameHUD);
            _logger.Log("UIInjectionInitializer: UI components injection completed");
        }
    }
}