namespace BricksAndBalls.Core.Models
{
    public class GameplayData
    {
        public BrickModel[] Bricks;
        public BallModel[] Balls;
        
        public int BrickCount;
        public int BallCount;
        public int AliveBrickCount;
        public int ActiveBallCount;
        
        public int AccumulatedHitScore;
        public int AccumulatedDestroyScore;
        public int CurrentScore;
        
        public int CurrentRound;
        public bool HasScoreChanged;
        public bool HasRoundEnded;
        public bool IsGameOver;
        public bool HasReachedBottom;
        public GameResult GameResult;

        public GameplayData(int maxBricks = 300, int maxBalls = 100)
        {
            Bricks = new BrickModel[maxBricks];
            Balls = new BallModel[maxBalls];
            Reset();
        }

        public void Reset()
        {
            BrickCount = 0;
            BallCount = 0;
            AliveBrickCount = 0;
            ActiveBallCount = 0;
            AccumulatedHitScore = 0;
            AccumulatedDestroyScore = 0;
            CurrentScore = 0;
            CurrentRound = 0;
            HasScoreChanged = false;
            HasRoundEnded = false;
            IsGameOver = false;
            HasReachedBottom = false;
            GameResult = GameResult.None;
        }
        
        public void ResetFrameFlags()
        {
            HasScoreChanged = false;
            HasRoundEnded = false;
        }
    }
}