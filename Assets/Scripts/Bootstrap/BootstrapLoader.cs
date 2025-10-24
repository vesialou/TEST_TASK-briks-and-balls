using BricksAndBalls.Core.Interfaces;
using BricksAndBalls.Services.Scene;
using Zenject;

namespace BricksAndBalls.Bootstrap
{
    public class BootstrapLoader : IInitializable
    {
        private readonly IAppLogger _logger;
        private readonly SceneLoader _sceneLoader;

        public BootstrapLoader(IAppLogger logger, SceneLoader sceneLoader)
        {
            _logger = logger;
            _sceneLoader = sceneLoader;
        }

        public void Initialize()
        {
            _logger.Log("Bootstrap complete, loading MainMenu");
            _sceneLoader.LoadMainMenu();
        }
    }
}