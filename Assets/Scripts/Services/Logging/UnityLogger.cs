using BricksAndBalls.Core.Interfaces;
using UnityEngine;

namespace BricksAndBalls.Services.Logging
{
    public class UnityLogger : IAppLogger
    {
        public void Log(string message)
        {
            Debug.Log(message);
        }

        public void LogWarning(string message)
        {
            Debug.LogWarning(message);
        }

        public void LogError(string message)
        {
            Debug.LogError(message);
        }
    }
}
