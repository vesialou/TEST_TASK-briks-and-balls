namespace BricksAndBalls.Core.Interfaces
{
    public interface IGameConfig
    {
        int BallsCount { get; }
        float BallSpeed { get; }

        int GridSize { get; }
        float CellSize { get; }
        float TopMargin { get; }
        float SideMargin { get; }

        int RoundsCount { get; }
        float RowDescendSpeed { get; }
        
        int HitBrickReward { get; }
        int DestroyBrickReward { get; }
        
        int BottomReserveRows { get; }
        float CameraPaddingPercent { get; }
        
        float CameraTopUIMargin { get; }
        float CameraBottomUIMargin { get; }
        
        UnityEngine.Vector2 GridCenter { get; }
        int[] Multipliers { get; }
    }
}