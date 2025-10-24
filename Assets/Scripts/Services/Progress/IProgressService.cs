namespace BricksAndBalls.Services.Progress
{
    public interface IProgressService
    {
        int CurrentLevelIndex { get; }
        void SaveGameResult(bool isWin, int finalScore);
    }
}