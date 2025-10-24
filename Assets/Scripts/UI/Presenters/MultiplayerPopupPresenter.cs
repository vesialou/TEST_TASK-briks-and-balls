using System;
using BricksAndBalls.Core.Interfaces;
using BricksAndBalls.UI.Popups;
using Cysharp.Threading.Tasks;
using Zenject;

namespace BricksAndBalls.UI.Presenters
{
    public class MultiplayerPopupPresenter : IPopupPresenter<MultiplayerPopupData, int>
    {
        private readonly DiContainer _container;
        private readonly IAppLogger _logger;

        public MultiplayerPopupPresenter(
            DiContainer  container,
            IAppLogger logger)
        {
            _container = container;
            _logger = logger;
        }

        public async UniTask<int> ShowAsync(BasePopup popup, MultiplayerPopupData data)
        {
            var view = popup as MultiplayerPopup;
            if (view == null)
            {
                throw new InvalidCastException("Popup is not a MultiplayerPopup");
            }
            _container.Inject(view);

            view.Initialize( data.Multipliers, data.CurrentScore);
            
            await view.WaitForCloseAsync();
            var multiplier = 1;
            if (view.SelectedMult >= 0 && view.SelectedMult < data.Multipliers.Length)
            {
                multiplier = data.Multipliers[view.SelectedMult];
            }

            return multiplier;
        }
    }
}