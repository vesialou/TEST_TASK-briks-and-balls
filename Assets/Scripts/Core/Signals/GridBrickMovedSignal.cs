using UnityEngine;

namespace BricksAndBalls.Core.Signals
{
    public struct GridBrickMovedSignal
    {
        public int BrickID { get; set; }
        public Vector2Int NewPos { get; set; }
    }
}