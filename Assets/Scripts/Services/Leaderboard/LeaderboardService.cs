using System;
using System.Collections.Generic;
using System.Linq;
using BricksAndBalls.Core.Interfaces;
using BricksAndBalls.Services.Settings;

namespace BricksAndBalls.Services.Leaderboard
{
    public class LeaderboardService
    {
        private const string LeaderboardKey = "Leaderboard";
        private const int TotalEntries = 100;

        private readonly IStorageService _storage;
        private readonly ISettingsService _settings;
        private readonly IAppLogger _appLogger;

        public LeaderboardService(
            IStorageService storage, 
            ISettingsService settings, 
            IAppLogger appLogger)
        {
            _storage = storage;
            _settings = settings;
            _appLogger = appLogger;
        }

        public List<LeaderboardEntry> GenerateRandomOpponents(int count)
        {
            var opponents = new List<LeaderboardEntry>();
            for (var i = 0; i < count; i++)
            {
                opponents.Add(new LeaderboardEntry
                {
                    PlayerName = $"Player_{UnityEngine.Random.Range(1000, 9999)}",
                    Score = UnityEngine.Random.Range(100, 5000),
                    IsPlayer = false
                });
            }

            return opponents;
        }

        public void SavePlayerScore(int score)
        {
            var gameScore = _storage.Load<GameScore>(LeaderboardKey);
            gameScore.Score = score;
            _appLogger.Log($"Player score saved: {gameScore.Score}");
            _storage.Save(LeaderboardKey, gameScore);
        }
        
        public int GetPlayerScore()
        {
            var gameScore = _storage.Load<GameScore>(LeaderboardKey);
            _appLogger.Log($"Player score is: {gameScore.Score}");
            return gameScore.Score;
        }

        public List<LeaderboardEntry> GetLeaderboard(int playerScore)
        {
            var entries = GenerateRandomOpponents(TotalEntries - 1);

            entries.Add(new LeaderboardEntry
            {
                PlayerName = _settings.GetPlayerName(),
                Score = playerScore,
                IsPlayer = true
            });

            return entries.OrderByDescending(e => e.Score).ToList();
        }
    }
    
    [Serializable]
    public class GameScore
    {
        public int Score;
    }
}
