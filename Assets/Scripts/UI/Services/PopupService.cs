using System;
using BricksAndBalls.UI.Popups;
using BricksAndBalls.UI.Presenters;
using Cysharp.Threading.Tasks;
using Zenject;

namespace BricksAndBalls.UI.Services
{
    public class PopupService : IPopupService
    {
        private readonly DiContainer _container;
        private readonly PopupManager _popupManager;
        private readonly IPopupPrefabProvider _prefabProvider;

        public PopupService(DiContainer container, PopupManager popupManager, IPopupPrefabProvider prefabProvider)
        {
            _container = container;
            _popupManager = popupManager;
            _prefabProvider = prefabProvider;
        }

        public async UniTask<TPresenter> ShowAsync<TPresenter>() where TPresenter : IPopupPresenter
        {
            var prefab = await _prefabProvider.LoadPrefabForPresenterAsync<TPresenter>();
            if (prefab == null)
            {
                throw new InvalidOperationException($"No prefab found for {typeof(TPresenter).Name}");
            }

            var popup = _popupManager.Show(prefab);
            var presenter = _container.Instantiate<TPresenter>();
            await presenter.InitializeAsync(popup);
            return presenter;
        }

        public async UniTask<TResult> ShowAsync<TPresenter, TData, TResult>(TData data) where TPresenter : IPopupPresenter<TData, TResult>
        {
            var prefab = await _prefabProvider.LoadPrefabForPresenterAsync<TPresenter>();
            if (prefab == null)
            {
                throw new InvalidOperationException($"No popup prefab for {typeof(TPresenter).Name}");
            }

            var popup = _popupManager.Show(prefab);
            var presenter = _container.Instantiate<TPresenter>();

            return await presenter.ShowAsync(popup, data);
        }
    }
}