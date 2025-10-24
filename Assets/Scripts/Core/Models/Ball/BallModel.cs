using UnityEngine;

namespace BricksAndBalls.Core.Models
{
    public class BallModel
    {
        public int ID { get; set; }
        public Vector2 Velocity { get; set; }
        public bool IsActive { get; set; }
        public bool HasReturned { get; set; }
    }
}