using BricksAndBalls.Configs;
using BricksAndBalls.Core.Interfaces;
using UnityEngine;
using Zenject;

namespace BricksAndBalls.Services.Config
{
    public class ScriptableGameConfigProvider : IGameConfigProvider
    {
        private const string Path = "Configs/GameConfig"; // in Resources
        private readonly IAppLogger _logger;
        private IGameConfig _cached;

        [Inject]
        public ScriptableGameConfigProvider(IAppLogger logger)
        {
            _logger = logger;
        }
        

        public IGameConfig GetConfig()
        {
            if (_cached != null)
            {
                return _cached;
            }

            var so = Resources.Load<GameConfig>(Path);
            if (so == null)
            {
                _logger.LogError($"[GameConfigProvider] Can't find GameConfig at Resources/{Path}");
                return null;
            }

            _cached = so;
            return _cached;
        }
    }
}