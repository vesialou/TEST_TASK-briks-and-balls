using BricksAndBalls.Core.Interfaces;
using BricksAndBalls.Factories;
using BricksAndBalls.Systems.Ball;
using BricksAndBalls.Systems.Playfield;
using UnityEngine;
using Zenject;

namespace BricksAndBalls.Gameplay
{
    public class LevelInitializer : IInitializable
    {
        private readonly BallLauncher _ballLauncher;
        private readonly PlayfieldManager _playfieldManager;
        private readonly BrickFactory _brickFactory;
        private readonly ILevelConfig _levelConfig;
        private readonly IAppLogger _logger;

        public LevelInitializer(
            BallLauncher ballLauncher,
            PlayfieldManager playfieldManager,
            BrickFactory brickFactory,
            ILevelConfig levelConfig,
            IAppLogger logger)
        {
            _ballLauncher = ballLauncher;
            _playfieldManager = playfieldManager;
            _brickFactory = brickFactory;
            _levelConfig = levelConfig;
            _logger = logger;
        }

        public void Initialize()
        {
            _logger.Log("LevelInitializer: Starting level initialization...");

            var bricks = _levelConfig.GetLevelData();

            foreach (var brickData in bricks)
            {
                _brickFactory.Create(brickData, null);
            }

            var launchPosition = new Vector3(
                _playfieldManager.WorldRect.center.x,
                _playfieldManager.WorldRect.yMin + 0.5f,
                0
            );

            _ballLauncher.SetLaunchPosition(launchPosition);
            
            _logger.Log($"Level initialized: {bricks.Count} bricks spawned");
        }
    }
}