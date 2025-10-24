using BricksAndBalls.Core.Interfaces;
using BricksAndBalls.Core.Models;
using BricksAndBalls.Services.Leaderboard;
using BricksAndBalls.Services.Progress;

namespace BricksAndBalls.Systems.Score
{
    public class ScoreManager
    {
        private readonly IProgressService _progressService;
        private readonly LeaderboardService _leaderboardService;
        private readonly GameplayData _gameplayData;
        private readonly IAppLogger _appLogger;

        public int CurrentScore => _gameplayData.CurrentScore;

        public ScoreManager(
            LeaderboardService leaderboardService,
            GameplayData gameplayData, 
            IAppLogger appLogger)
        {
            _leaderboardService = leaderboardService;
            _gameplayData = gameplayData;
            _appLogger = appLogger;

            _appLogger.Log("ScoreManager initialized");
        }

        public void ResetScore()
        {
            _gameplayData.CurrentScore = 0;
            _appLogger.Log("Score reset");
        }

        public int GetFinalScore(int multiplier)
        {
            return _gameplayData.CurrentScore * multiplier;
        }
        
        public void ApplyMultiplierAndSave(int multiplier)
        {
            var finalScore = GetFinalScore(multiplier);
            _gameplayData.CurrentScore = finalScore;

            _leaderboardService.SavePlayerScore(finalScore);
            _appLogger.Log($"Final score {finalScore} saved with multiplier x{multiplier}");
        }
    }
}
