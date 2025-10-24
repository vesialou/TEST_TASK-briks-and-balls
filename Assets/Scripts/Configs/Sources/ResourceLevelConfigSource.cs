using BricksAndBalls.Core.Interfaces;
using Configs;
using UnityEngine;

namespace BricksAndBalls.Configs.Sources
{
    public class ResourceLevelConfigSource : ILevelConfigSource
    {
        public ILevelConfig LoadLevel(int index)
        {
            ILevelConfig asset = Resources.Load<ScriptableLevelConfig>($"Levels/Level_{index}");
            if (asset == null)
            {
                Debug.LogWarning($"Level {index} not found in Resources, using default");
                asset = new MockedLevelConfig();
            }
            return asset;
        }
    }

}