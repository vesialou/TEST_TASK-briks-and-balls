using BricksAndBalls.Core.Interfaces;
using UnityEngine.SceneManagement;

namespace BricksAndBalls.Services.Scene
{
    public class SceneLoader
    {
        private readonly IAppLogger _logger;

        public SceneLoader(IAppLogger logger)
        {
            _logger = logger;
        }

        public void LoadMainMenu()
        {
            _logger.Log("Loading MainMenu scene");
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }

        public void LoadGame()
        {
            _logger.Log("Loading Game scene");
            SceneManager.LoadScene("Game", LoadSceneMode.Single);
        }

        public void ReloadCurrentScene()
        {
            var currentScene = SceneManager.GetActiveScene().name;
            _logger.Log($"Reloading scene: {currentScene}");
            SceneManager.LoadScene(currentScene, LoadSceneMode.Single);
        }
    }
}