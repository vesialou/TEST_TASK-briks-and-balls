using BricksAndBalls.Core.Interfaces;

namespace BricksAndBalls.Core.Models
{
    public class GameStateMachine
    {
        private readonly IAppLogger _appLogger;
        private GameState _currentState;

        public GameState CurrentState => _currentState;

        public GameStateMachine(IAppLogger appLogger)
        {
            _appLogger = appLogger;
            _currentState = GameState.WaitingForShoot;
        }

        public void ChangeState(GameState newState)
        {
            if (_currentState == newState)
            {
                return;
            }

            _appLogger.Log($"State changed: {_currentState} -> {newState}");
            _currentState = newState;
        }

        public bool CanShoot() => _currentState == GameState.WaitingForShoot;
    }


}
