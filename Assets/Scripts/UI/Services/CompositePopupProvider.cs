using BricksAndBalls.UI.Popups;
using BricksAndBalls.UI.Presenters;
using Cysharp.Threading.Tasks;

namespace BricksAndBalls.UI.Services
{
    public class CompositePopupProvider : IPopupPrefabProvider
    {
        private readonly PopupPrefabProvider  _registryProvider;
        private readonly AddressablePopupProvider _addressableProvider;

        public CompositePopupProvider(PopupPrefabProvider  registryProvider, AddressablePopupProvider addressableProvider)
        {
            _registryProvider = registryProvider;
            _addressableProvider = addressableProvider;
        }

        public async UniTask<BasePopup> LoadPrefabForPresenterAsync<TPresenter>()
            where TPresenter : IPopupPresenterMarker
        {
#if UNITY_EDITOR
            var prefab = _registryProvider.GetPrefabForPresenter<TPresenter>();
            if (prefab != null)
            {
                return prefab;
            }
#endif
            return await _addressableProvider.LoadPrefabForPresenterAsync<TPresenter>();
        }
    }
}