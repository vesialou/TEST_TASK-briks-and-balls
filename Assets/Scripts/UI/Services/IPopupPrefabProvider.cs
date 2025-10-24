using BricksAndBalls.UI.Popups;
using BricksAndBalls.UI.Presenters;
using Cysharp.Threading.Tasks;

namespace BricksAndBalls.UI.Services
{
    public interface IPopupPrefabProvider
    {
        UniTask<BasePopup> LoadPrefabForPresenterAsync<TPresenter>() where TPresenter : IPopupPresenterMarker;
    }
}