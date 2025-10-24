using BricksAndBalls.UI.Popups;
using Cysharp.Threading.Tasks;

namespace BricksAndBalls.UI.Presenters
{
    public interface IPopupPresenter : IPopupPresenterMarker
    {
        UniTask InitializeAsync(BasePopup popup);
    }
}