using System;
using System.Collections.Generic;
using BricksAndBalls.UI.Popups;
using BricksAndBalls.UI.Presenters;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace BricksAndBalls.UI.Services
{
    public class AddressablePopupProvider : IPopupPrefabProvider
    {
        private readonly Dictionary<Type, BasePopup> _cache = new();
        public async UniTask<BasePopup> LoadPrefabForPresenterAsync<TPresenter>()
            where TPresenter : IPopupPresenterMarker
        {
            var type = typeof(TPresenter);
            if (_cache.TryGetValue(type, out var cached))
            {
                return cached;
            }
            
            var key = PopupKeyExtensions.GetPopupKey<TPresenter>();
            var handle = Addressables.LoadAssetAsync<GameObject>(key);
            await handle.Task;

            if (handle.Status != AsyncOperationStatus.Succeeded)
            {
                throw new Exception($"Failed to load popup prefab for key '{key}'");
            }

            var prefab = handle.Result.GetComponent<BasePopup>();
            _cache[type] = prefab;
            return prefab;
        }
    }
}