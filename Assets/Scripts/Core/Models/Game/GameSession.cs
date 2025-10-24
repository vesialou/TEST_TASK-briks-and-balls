using BricksAndBalls.Core.Interfaces;
using BricksAndBalls.Core.Signals;
using Zenject;

namespace BricksAndBalls.Core.Models
{
    public class GameSession
    {
        private readonly SignalBus _signalBus;
        private readonly IGameConfig _config;
        private readonly IAppLogger _appLogger;

        public int CurrentRound { get; private set; }
        public int MaxRounds => _config.RoundsCount;
        public bool IsGameActive { get; private set; }
        public GameResult Result { get; private set; }

        public GameSession(
            SignalBus  signalBus,
            IGameConfig config, 
            IAppLogger appLogger)
        {
            _signalBus = signalBus;
            _config = config;
            _appLogger = appLogger;

            Reset();
        }

        public void Reset()
        {
            CurrentRound = 0;
            IsGameActive = true;
            Result = GameResult.None;
            
            _appLogger.Log("GameSession reset");
        }

        public void NextRound()
        {
            if (!IsGameActive)
            {
                return;
            }

            CurrentRound++;
            _appLogger.Log($"Round advanced: {CurrentRound}/{MaxRounds}");

        }        
        
        public void CheckEndConditions(int aliveBricks, bool hasReachedBottom)
        {
            if (!IsGameActive)
            {
                return;
            }

            // 🟢 Победа — все блоки уничтожены
            if (aliveBricks <= 0)
            {
                EndGame(GameResult.Win);
                _signalBus.Fire(new GameOverSignal { IsWin = true });
                return;
            }

            // 🔴 Поражение — блок достиг нижней линии
            if (hasReachedBottom)
            {
                EndGame(GameResult.Loss);
                _signalBus.Fire(new GameOverSignal { IsWin = false });
                return;
            }

            // 🔴 Поражение — закончились раунды
            if (CurrentRound >= MaxRounds - 1 && aliveBricks > 0)
            {
                EndGame(GameResult.Loss);
                _signalBus.Fire(new GameOverSignal { IsWin = false });
                return;
            }
            
            if (CurrentRound >= MaxRounds - 1)
            {
                EndGame(GameResult.Win);
                _signalBus.Fire(new GameOverSignal { IsWin = true });
            }
        }


        public void EndGame(GameResult result)
        {
            IsGameActive = false;
            Result = result;
            
            _appLogger.Log($"Game ended: {result}");
        }
    }
}
