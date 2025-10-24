using BricksAndBalls.Core.Interfaces;
using BricksAndBalls.Core.Models;
using BricksAndBalls.Core.Signals;
using BricksAndBalls.Services.Progress;
using BricksAndBalls.Services.Scene;
using BricksAndBalls.Systems.Grid;
using BricksAndBalls.Systems.Score;
using BricksAndBalls.UI.Popups;
using BricksAndBalls.UI.Presenters;
using Cysharp.Threading.Tasks;
using Zenject;

namespace BricksAndBalls.Gameplay
{
    public class GameController : IInitializable
    {
        private readonly IGameConfig _gameConfig;
        private readonly IPopupService _popupService;
        private readonly GameStateMachine _stateMachine;
        private readonly GameSession _session;
        private readonly GameplayData _gameplayData;
        private readonly GridMovementSystem _gridMovementSystem;
        private readonly ScoreManager _scoreManager;
        private readonly SceneLoader _sceneLoader;
        private readonly SignalBus _signalBus;
        private readonly IAppLogger _logger;
        private readonly IProgressService _progressService;

        public GameController(
            IGameConfig gameConfig,
            IPopupService  popupService,
            GameStateMachine stateMachine,
            GameSession session,
            GameplayData gameplayData,
            GridMovementSystem gridMovementSystem,
            ScoreManager scoreManager,
            SceneLoader sceneLoader,
            SignalBus signalBus,
            IAppLogger logger,
            IProgressService progressService)
        {
            _gameConfig = gameConfig;
            _popupService = popupService;
            _stateMachine = stateMachine;
            _session = session;
            _gameplayData = gameplayData;
            _gridMovementSystem = gridMovementSystem;
            _scoreManager = scoreManager;
            _sceneLoader = sceneLoader;
            _signalBus = signalBus;
            _logger = logger;
            _progressService = progressService;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<RoundEndedSignal>(OnRoundEnded);
            _signalBus.Subscribe<GameOverSignal>(OnGameOver);

            _logger.Log("GameController initialized");
        }

        private void OnRoundEnded(RoundEndedSignal signal)
        {
            MoveGridDownAsync().Forget();
        }
        private async UniTaskVoid MoveGridDownAsync()
        {
            _logger.Log("Round ended - Descending grid");
            _stateMachine.ChangeState(GameState.DescendingGrid);
            await _gridMovementSystem.MoveGridDownAsync();

            _stateMachine.ChangeState(GameState.CheckGameOver);
            CheckGameOver();
        }

        private void CheckGameOver()
        {
            var alive = _gameplayData.AliveBrickCount;
            var hasReachedBottom = _gameplayData.HasReachedBottom;
            _session.CheckEndConditions(alive, hasReachedBottom);

            if (!_session.IsGameActive)
            {
                return;
            }

            _session.NextRound();
            _stateMachine.ChangeState(GameState.WaitingForShoot);
        }

        private void OnGameOver(GameOverSignal signal)
        {
            OnGameOverAsync(signal).Forget();
        }
        
        private async UniTaskVoid OnGameOverAsync(GameOverSignal signal)
        {
            _logger.Log($"Game Over: {(signal.IsWin ? "WIN" : "LOSS")}");
            _stateMachine.ChangeState(GameState.GameEnded);
            var multiplayerData = new MultiplayerPopupData
            {
                CurrentScore = _scoreManager.CurrentScore,
                Multipliers = _gameConfig.Multipliers
            };
            var multiplier = await _popupService.ShowAsync<MultiplayerPopupPresenter,
                MultiplayerPopupData,
                int>(multiplayerData);
            
            _scoreManager.ApplyMultiplierAndSave(multiplier);
            var finalScore = _scoreManager.CurrentScore;
            _progressService.SaveGameResult(signal.IsWin, finalScore);
            
            await _popupService.ShowAsync<LeaderboardPopupPresenter>();
            
            var gameOverData = new GameOverPopupData
            {
                IsWin = signal.IsWin,
                Score = _scoreManager.CurrentScore,
                HasNextLevel = false
            };

            var result = await _popupService.ShowAsync<
                GameOverPopupPresenter,
                GameOverPopupData,
                GameOverPopupResult>(gameOverData);
            
            switch (result.State)
            {
                case GameOverPopupResult.ResultType.Menu:
                    _sceneLoader.LoadMainMenu();
                    break;
                
                case GameOverPopupResult.ResultType.Replay:
                    _sceneLoader.ReloadCurrentScene();
                    break;
                
                case GameOverPopupResult.ResultType.Next:
                    _sceneLoader.LoadGame();
                    break;
            }
            _sceneLoader.LoadGame();
        }
    }
}