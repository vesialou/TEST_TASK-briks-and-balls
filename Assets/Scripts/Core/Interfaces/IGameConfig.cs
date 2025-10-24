namespace BricksAndBalls.Core.Interfaces
{
    public interface IGameConfig
    {
        // Ball
        int BallsCount { get; }
        float BallSpeed { get; }

        // Grid
        int GridSize { get; }
        float CellSize { get; }
        float TopMargin { get; }
        float SideMargin { get; }

        // Gameplay
        int RoundsCount { get; }
        float RowDescendSpeed { get; }
        
        // Play Rewards
        int HitBrickReward { get; }
        int DestroyBrickReward { get; }
        
        int BottomReserveRows { get; }
        float CameraPaddingPercent { get; }
        
        // Camera UI margins
        float CameraTopUIMargin { get; }
        float CameraBottomUIMargin { get; }
        
        // Grid positioning
        UnityEngine.Vector2 GridCenter { get; }
        int[] Multipliers { get; }
    }
}