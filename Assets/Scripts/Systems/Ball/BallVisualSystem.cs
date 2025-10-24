using System.Collections.Generic;
using BricksAndBalls.Core.Interfaces;
using BricksAndBalls.Views;

namespace BricksAndBalls.Systems.Ball
{
    public class BallVisualSystem
    {
        private readonly Dictionary<int, BallView> _views = new Dictionary<int, BallView>();
        private readonly IAppLogger _logger;

        public BallVisualSystem(IAppLogger logger)
        {
            _logger = logger;

            _logger.Log("BallVisualSystem initialized");
        }

        public void RegisterBall(int ballID, BallView view)
        {
            _views[ballID] = view;
        }

        public void UnregisterBall(int ballID)
        {
            _views.Remove(ballID);
        }

        public bool TryGetBallView(int ballID, out BallView ballView)
        {
            return _views.TryGetValue(ballID, out ballView);
        }
    }
}
