using BricksAndBalls.Components.Physics;
using BricksAndBalls.Core.Models;
using BricksAndBalls.Views;

namespace BricksAndBalls.Factories
{
    public struct BallInstance
    {
        public BallModel Model;
        public BallView View;
        public BallPhysicsComponent Physics;
    }
}