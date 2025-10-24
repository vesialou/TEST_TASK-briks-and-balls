using System;
using System.Collections.Generic;
using BricksAndBalls.UI.Popups;
using UnityEngine;

namespace BricksAndBalls.UI.Configs
{
    [CreateAssetMenu(menuName = "Game/Popup Registry", fileName = "PopupRegistry")]
    public class PopupRegistry : ScriptableObject
    {
        [Serializable]
        public struct Entry
        {
            public string PresenterTypeName;
            public BasePopup PopupPrefab;
        }

        [SerializeField] private List<Entry> _entries = new();

        private Dictionary<string, BasePopup> _map;

        private void OnEnable()
        {
            _map = new Dictionary<string, BasePopup>(_entries.Count);
            foreach (var e in _entries)
            {
                var key = e.PresenterTypeName;
                if (key == null)
                {
                    Debug.LogWarning($"PopupRegistry: type not found '{e.PresenterTypeName}'");
                }
                else
                {
                    _map[key] = e.PopupPrefab;
                }
            }
        }

        public BasePopup GetPrefab(Type presenterType)
        {
            var key = PopupKeyExtensions.GetPopupKey(presenterType);
            if (_map.TryGetValue(key, out var prefab))
            {
                return prefab;
            }

            Debug.LogError($"PopupRegistry: prefab not found for {presenterType.Name}");
            return null;
        }
    }
}