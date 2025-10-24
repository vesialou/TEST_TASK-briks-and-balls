using BricksAndBalls.UI.Popups;
using Cysharp.Threading.Tasks;

namespace BricksAndBalls.UI.Presenters
{
    public interface IPopupPresenter<TData, TResult> : IPopupPresenterMarker
    {
        UniTask<TResult> ShowAsync(BasePopup popup, TData data);
    }
}