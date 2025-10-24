using System;
using BricksAndBalls.Core.Interfaces;
using BricksAndBalls.Services.Leaderboard;
using BricksAndBalls.UI.Popups;
using Cysharp.Threading.Tasks;
using Zenject;

namespace BricksAndBalls.UI.Presenters
{
    public class LeaderboardPopupPresenter : IPopupPresenter
    {
        private readonly DiContainer _container;
        private readonly LeaderboardService _leaderboardService;
        private readonly IAppLogger _appLogger;

        public LeaderboardPopupPresenter(
            DiContainer  container,
            LeaderboardService leaderboardService,
            IAppLogger appLogger)
        {
            _container = container;
            _leaderboardService = leaderboardService;
            _appLogger = appLogger;
        }
        public async UniTask InitializeAsync(BasePopup popup)
        {
            var view = popup as LeaderboardPopup;
            if (view == null)
            {
                throw new InvalidCastException("Popup is not a LeaderboardPopup");
            }
            _container.Inject(view);
            
            var myScore = _leaderboardService.GetPlayerScore();
            var all = _leaderboardService.GetLeaderboard(myScore);
            view.ShowLeaderboard(all);
            _appLogger.Log($"Loaded {all.Count} leaderboards");
            await view.WaitForCloseAsync();
        }
    }
}