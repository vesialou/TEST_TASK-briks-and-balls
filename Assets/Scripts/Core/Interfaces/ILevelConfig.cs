using System.Collections.Generic;
using BricksAndBalls.Core.Models;

namespace BricksAndBalls.Core.Interfaces
{
    public interface ILevelConfig
    {
        List<BrickData> GetLevelData();
    }
}