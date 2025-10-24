using System;
using UnityEngine;

namespace BricksAndBalls.Core.Models
{
    [Serializable]
    public struct BrickData
    {
        public Vector2Int GridPosition;
        public int HP;
        public BrickType Type;
    }
}
