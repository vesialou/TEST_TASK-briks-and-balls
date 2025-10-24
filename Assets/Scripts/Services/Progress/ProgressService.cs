using System;
using BricksAndBalls.Core.Interfaces;

namespace BricksAndBalls.Services.Progress
{
    public class ProgressService : IProgressService
    {
        private const string ProgressKey = "GameProgress";
        
        private readonly IStorageService _storage;
        private readonly IAppLogger _appLogger;
        private GameProgress _progress;

        public int CurrentLevelIndex => _progress.Wins;

        public ProgressService(IStorageService storage, IAppLogger appLogger)
        {
            _storage = storage;
            _appLogger = appLogger;

            LoadProgress();
        }

        private void LoadProgress()
        {
            _progress = _storage.Load<GameProgress>(ProgressKey, new GameProgress());
            _appLogger.Log($"Progress loaded: Wins={_progress.Wins}, Losses={_progress.Losses}, HighScore={_progress.HighScore}");
        }

        public void SaveGameResult(bool isWin, int finalScore)
        {
            _progress.TotalGamesPlayed++;

            if (isWin)
            {
                _progress.Wins++;
            }
            else
            {
                _progress.Losses++;
            }

            if (finalScore > _progress.HighScore)
            {
                _progress.HighScore = finalScore;
                _appLogger.Log($"New high score: {finalScore}");
            }

            _storage.Save(ProgressKey, _progress);
            _appLogger.Log($"Game result saved: Win={isWin}, Score={finalScore}");
        }

        public GameProgress GetProgress() => _progress;
    }

    [Serializable]
    public class GameProgress
    {
        public int TotalGamesPlayed;
        public int Wins;
        public int Losses;
        public int HighScore;
    }
}
