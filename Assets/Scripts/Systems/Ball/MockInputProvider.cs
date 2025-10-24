using UnityEngine;

namespace BricksAndBalls.Systems.Ball
{
    public class MockInputProvider
    {
        public Vector2 GetLaunchDirection()
        {
            return new Vector2(0.5f, 1f).normalized;
        }
    }
}