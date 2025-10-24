using BricksAndBalls.Bootstrap;
using BricksAndBalls.UI;
using UnityEngine;
using Zenject;

namespace BricksAndBalls.Installers
{
    public class UIInstaller : MonoInstaller
    {
        [SerializeField] private GameHUD _gameHUD;

        public override void InstallBindings()
        {
            Container.Bind<GameHUD>().FromInstance(_gameHUD).AsSingle().NonLazy();

            Container.BindInterfacesTo<UIInjectionInitializer>().AsSingle().NonLazy();
        }
    }
}