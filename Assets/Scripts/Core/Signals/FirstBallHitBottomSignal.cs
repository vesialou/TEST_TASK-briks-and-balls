using UnityEngine;

namespace BricksAndBalls.Core.Signals
{
    public struct FirstBallHitBottomSignal
    {
        public Vector3 Position;
        public FirstBallHitBottomSignal(Vector3 position)
        {
            Position = position;
        }
    }
}