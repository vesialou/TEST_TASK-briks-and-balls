using BricksAndBalls.Core.Interfaces;
using BricksAndBalls.Services.Progress;

namespace BricksAndBalls.Services.Config
{
    public class LevelConfigProvider : ILevelConfigProvider
    {
        private readonly ILevelConfigSource _source;
        private readonly IProgressService _progress;

        public LevelConfigProvider(ILevelConfigSource source, IProgressService progress)
        {
            _source = source;
            _progress = progress;
        }

        public ILevelConfig GetCurrentLevel()
        {
            return _source.LoadLevel(_progress.CurrentLevelIndex);
        }
    }
}