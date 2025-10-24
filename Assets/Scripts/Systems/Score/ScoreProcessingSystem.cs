using BricksAndBalls.Core.Interfaces;
using BricksAndBalls.Core.Models;
using BricksAndBalls.Core.Signals;
using Zenject;

namespace BricksAndBalls.Systems.Score
{
    public class ScoreProcessingSystem : ILateTickable
    {
        private readonly GameplayData _gameplayData;
        private readonly SignalBus _signalBus;
        private readonly IAppLogger _logger;

        public ScoreProcessingSystem(
            GameplayData gameplayData,
            SignalBus signalBus,
            IAppLogger logger)
        {
            _gameplayData = gameplayData;
            _signalBus = signalBus;
            _logger = logger;
        }

        public void LateTick()
        {
            var accumulatedScore = _gameplayData.AccumulatedHitScore + 
                                   _gameplayData.AccumulatedDestroyScore;

            if (accumulatedScore > 0)
            {
                _gameplayData.CurrentScore += accumulatedScore;
                _gameplayData.HasScoreChanged = true;
                
                _gameplayData.AccumulatedHitScore = 0;
                _gameplayData.AccumulatedDestroyScore = 0;
                
                _signalBus.Fire(new ScoreChangedSignal 
                { 
                    Score = _gameplayData.CurrentScore 
                });
            }
        }
    }
}