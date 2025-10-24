using System;

namespace BricksAndBalls.Services.Leaderboard
{

    [Serializable]
    public class LeaderboardEntry
    {
        public string PlayerName;
        public int Score;
        public bool IsPlayer;
    }
}