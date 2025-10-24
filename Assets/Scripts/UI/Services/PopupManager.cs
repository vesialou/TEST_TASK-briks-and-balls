using System.Collections.Generic;
using BricksAndBalls.Core.Interfaces;
using BricksAndBalls.UI.Popups;
using UnityEngine;
using Zenject;

namespace BricksAndBalls.UI.Services
{
    public class PopupManager
    {
        private readonly IAppLogger _logger;
        private readonly DiContainer _container;
        private readonly Stack<BasePopup> _popupStack = new();
        private Transform _popupContainer;

        public PopupManager(IAppLogger logger, DiContainer container)
        {
            _logger = logger;
            _container = container;
        }

        public void SetContainer(Transform container)
        {
            _popupContainer = container;
        }

        public TPopup Show<TPopup>(TPopup prefab) where TPopup : BasePopup
        {
            if (_popupContainer == null)
            {
                _logger.LogWarning("PopupManager: Container not set! Using scene root.");
                _popupContainer = Object.FindObjectOfType<Canvas>()?.transform;
            }

            var instance = _container.InstantiatePrefabForComponent<TPopup>(prefab, _popupContainer);

            _popupStack.Push(instance);
            instance.OnClosed += () => OnPopupClosed(instance);

            instance.Show();
            _logger.Log($"PopupManager: Show<{typeof(TPopup).Name}>");

            return instance;
        }

        private void OnPopupClosed(BasePopup popup)
        {
            if (_popupStack.Count <= 0 || _popupStack.Peek() != popup)
            {
                _popupStack.TryPop(popup);
            }
            else
            {
                _popupStack.Pop();
            }

            Object.Destroy(popup.gameObject);
            _logger.Log($"PopupManager: Closed {popup.GetType().Name}");
        }

        public void CloseTop()
        {
            if (_popupStack.Count > 0)
            {
                _popupStack.Peek().Close();
            }
        }

        public void CloseAll()
        {
            while (_popupStack.Count > 0)
                _popupStack.Pop().Close();
        }
    }

    
    internal static class StackExtensions
    {
        public static void TryPop<T>(this Stack<T> stack, T value)
        {
            if (stack.Count == 0)
            {
                return;
            }

            var tmp = new Stack<T>(stack.Count);
            while (stack.Count > 0)
            {
                var item = stack.Pop();
                if (EqualityComparer<T>.Default.Equals(item, value))
                {
                    break;
                }

                tmp.Push(item);
            }
            while (tmp.Count > 0)
                stack.Push(tmp.Pop());
        }
    }
}
