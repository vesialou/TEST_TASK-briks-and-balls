using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BricksAndBalls.UI.Popups
{
    public abstract class BasePopup : MonoBehaviour
    {
        [SerializeField] protected GameObject _panel;
        [SerializeField] protected CanvasGroup _canvasGroup;

        public event Action OnClosed;
        private UniTaskCompletionSource _closeTcs;

        public virtual void Show()
        {
            _closeTcs?.TrySetResult(); 
            _closeTcs =  new UniTaskCompletionSource();
            _panel.SetActive(true);
            if (_canvasGroup != null)
            {
                _canvasGroup.alpha = 1f;
                _canvasGroup.interactable = true;
                _canvasGroup.blocksRaycasts = true;
            }
        }

        public virtual void Close()
        {
            if (_canvasGroup != null)
            {
                _canvasGroup.interactable = false;
                _canvasGroup.blocksRaycasts = false;
            }

            OnClosed?.Invoke();
            _closeTcs?.TrySetResult();
        }

        public UniTask WaitForCloseAsync()
        {
            if (_closeTcs?.Task != null)
            {
                return _closeTcs.Task;
            }
            
            return UniTask.CompletedTask;
        }


        protected virtual void OnDestroy()
        {
            _closeTcs?.TrySetResult();
        }
    }
}