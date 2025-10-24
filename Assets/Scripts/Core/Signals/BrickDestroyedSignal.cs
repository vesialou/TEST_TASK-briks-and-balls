
using UnityEngine;

namespace BricksAndBalls.Core.Signals
{
    public struct BrickDestroyedSignal
    {
        public Vector2Int GridPosition { get; set; }
    }
}