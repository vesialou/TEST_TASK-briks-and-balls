using System;
using BricksAndBalls.UI.Configs;
using BricksAndBalls.UI.Popups;
using BricksAndBalls.UI.Presenters;
using Cysharp.Threading.Tasks;

namespace BricksAndBalls.UI.Services
{
    public class PopupPrefabProvider : IPopupPrefabProvider
    {
        private readonly PopupRegistry _registry;

        public PopupPrefabProvider(PopupRegistry registry)
        {
            _registry = registry;
        }

        public BasePopup GetPrefabForPresenter<TPresenter>() where TPresenter : IPopupPresenterMarker
        {
            return _registry.GetPrefab(typeof(TPresenter));
        }

        public UniTask<BasePopup> LoadPrefabForPresenterAsync<TPresenter>()
            where TPresenter : IPopupPresenterMarker
        {
            var prefab = GetPrefabForPresenter<TPresenter>();
            if (prefab == null)
            {
                throw new InvalidOperationException(
                    $"No prefab registered in PopupRegistry for {typeof(TPresenter).Name}");
            }

            return UniTask.FromResult(prefab);
        }
    }
}