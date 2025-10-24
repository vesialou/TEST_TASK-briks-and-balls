using BricksAndBalls.UI.Popups;
using Cysharp.Threading.Tasks;

namespace BricksAndBalls.UI.Presenters
{
    public interface IPopupPresenter<TResult> : IPopupPresenterMarker
    {
        UniTask<TResult> ShowAsync(BasePopup popup);
    }
}