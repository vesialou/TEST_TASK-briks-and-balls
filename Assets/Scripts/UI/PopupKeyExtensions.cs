using System;
using BricksAndBalls.UI.Presenters;

namespace BricksAndBalls.UI
{
    public static class PopupKeyExtensions
    {
        /// <summary>
        /// Генерирует стандартный ключ загрузки popup-префаба из типа презентера.
        /// Пример: MultiplayerPopupPresenter → "popup_multiplayerpopup"
        /// </summary>
        public static string GetPopupKey<TPresenter>() where TPresenter : IPopupPresenterMarker
        {
            return GetPopupKey(typeof(TPresenter));
        }

        /// <summary>
        /// Универсальный вариант для произвольного типа презентера.
        /// </summary>
        public static string GetPopupKey(Type presenterType)
        {
            if (presenterType == null)
            {
                throw new ArgumentNullException(nameof(presenterType));
            }

            var name = presenterType.Name;
            if (name.EndsWith("Presenter", StringComparison.OrdinalIgnoreCase))
            {
                name = name.Substring(0, name.Length - "PopupPresenter".Length);
            }

            return $"popup_{name.ToLower()}";
        }
    }
}