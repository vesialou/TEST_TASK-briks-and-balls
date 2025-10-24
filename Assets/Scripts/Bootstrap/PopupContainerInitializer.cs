using BricksAndBalls.Core.Interfaces;
using BricksAndBalls.UI.Services;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace BricksAndBalls.Bootstrap
{
    public class PopupContainerInitializer : IInitializable
    {
        private readonly IAppLogger _logger;
        private readonly PopupManager _popupManager;

        public PopupContainerInitializer(IAppLogger logger, PopupManager popupManager)
        {
            _logger = logger;
            _popupManager = popupManager;
        }

        public void Initialize()
        {
            var container = CreateGlobalPopupContainer();
            _popupManager.SetContainer(container);
            _logger.Log("Global popup container created (DontDestroyOnLoad)");
        }

        private Transform CreateGlobalPopupContainer()
        {
            var popupRoot = new GameObject("GlobalPopupsCanvas");
            Object.DontDestroyOnLoad(popupRoot);

            var canvas = popupRoot.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 999;

            var scaler = popupRoot.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.matchWidthOrHeight = 0.5f;

            popupRoot.AddComponent<GraphicRaycaster>();
            return popupRoot.transform;
        }
    }
}