using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BricksAndBalls.Services
{
    public interface IAddressablesService
    {
        UniTask<GameObject> LoadAsset(string key);
        void ReleaseAsset(string key);
    }
}
