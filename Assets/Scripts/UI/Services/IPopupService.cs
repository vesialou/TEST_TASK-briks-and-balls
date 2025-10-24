using BricksAndBalls.UI.Presenters;
using Cysharp.Threading.Tasks;

namespace BricksAndBalls.UI.Popups
{
    public interface IPopupService
    {
        UniTask<TPresenter> ShowAsync<TPresenter>()
            where TPresenter : IPopupPresenter;

        UniTask<TResult> ShowAsync<TPresenter, TData, TResult>(TData data) 
            where TPresenter : IPopupPresenter<TData, TResult>;
    }
}