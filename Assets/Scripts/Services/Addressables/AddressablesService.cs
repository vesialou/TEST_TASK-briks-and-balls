using System;
using BricksAndBalls.Core.Interfaces;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace BricksAndBalls.Services
{
    public class AddressablesService : IAddressablesService
    {
        private readonly IAppLogger _logger;

        public AddressablesService(IAppLogger logger)
        {
            _logger = logger;
        }

        public async UniTask<GameObject> LoadAsset(string key)
        {
            _logger.Log($"[Addressables] Loading asset: {key}");

            var handle = Addressables.LoadAsset<GameObject>(key);
            await handle.Task;

            if (handle.Status != AsyncOperationStatus.Succeeded)
            {
                _logger.LogError($"[Addressables] Failed to load asset with key: {key}");
                throw new Exception($"Failed to load addressable asset: {key}");
            }

            _logger.Log($"[Addressables] Successfully loaded: {key}");
            return handle.Result;
        }

        public void ReleaseAsset(string key)
        {
            _logger.Log($"[Addressables] Releasing asset: {key}");
            Addressables.Release(key);
        }
    }
}