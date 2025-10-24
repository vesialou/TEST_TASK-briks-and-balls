namespace BricksAndBalls.Core.Interfaces
{
    public interface ILevelConfigSource
    {
        ILevelConfig LoadLevel(int index);
    }

}