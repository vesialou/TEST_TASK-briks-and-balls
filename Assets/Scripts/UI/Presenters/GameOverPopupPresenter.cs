using System;
using BricksAndBalls.Core.Interfaces;
using BricksAndBalls.UI.Popups;
using Cysharp.Threading.Tasks;
using Zenject;

namespace BricksAndBalls.UI.Presenters
{
    public class GameOverPopupPresenter : IPopupPresenter<GameOverPopupData, GameOverPopupResult>
    {
        private readonly DiContainer _container;
        private readonly IAppLogger _logger;

        private GameOverPopupResult _result;
        
        public GameOverPopupPresenter(
            DiContainer  container,
            IAppLogger logger)
        {
            _container = container;
            _logger = logger;
            _result = new();
        }

        public async UniTask<GameOverPopupResult> ShowAsync(BasePopup popup, GameOverPopupData data)
        {
            var view = popup as GameOverPopup;
            if (view == null)
            {
                throw new InvalidCastException("Popup is not a GameOverPopup");
            }
            
            view.OnMenuClicked += ViewOnOnMenuClicked;
            view.OnPlayAgainClicked += ViewOnOnPlayAgainClicked;
            view.OnPlayNextClicked += ViewOnOnPlayNextClicked;
            _container.Inject(view);
            
            var isWin = data.IsWin;
            var score = data.Score;

            view.Setup(isWin, score, hasNextLevel: false);
            await view.WaitForCloseAsync();
            _logger.Log("GameOver popup closed.");
            return _result;
        }

        private void ViewOnOnPlayNextClicked()
        {
            _result.State = GameOverPopupResult.ResultType.Next;
        }

        private void ViewOnOnPlayAgainClicked()
        {
            _result.State = GameOverPopupResult.ResultType.Replay;
        }

        private void ViewOnOnMenuClicked()
        {
            _result.State = GameOverPopupResult.ResultType.Menu;
        }
    }

}