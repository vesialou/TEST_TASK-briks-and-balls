using System;
using BricksAndBalls.Core.Interfaces;
using BricksAndBalls.UI.Popups;
using Cysharp.Threading.Tasks;
using Zenject;

namespace BricksAndBalls.UI.Presenters
{
    public class SettingsPopupPresenter: IPopupPresenter< SettingsPopupData,  SettingsPopupResult>
    {
        private readonly DiContainer _container;
        private readonly IAppLogger _appLogger;

        public SettingsPopupPresenter(
            DiContainer  container,
            IAppLogger appLogger)
        {
            _container = container;
            _appLogger = appLogger;
        }
        public async UniTask<SettingsPopupResult> ShowAsync(BasePopup popup, SettingsPopupData data)
        {
            var view = popup as SettingsPopup;
            if (view == null)
            {
                throw new InvalidCastException("Popup is not a SettingsPopup");
            }
            _container.Inject(view);
            view.Setup(data.UserName);
            
            await view.WaitForCloseAsync();
            var result = new SettingsPopupResult();
            return result;
        }
    }
}