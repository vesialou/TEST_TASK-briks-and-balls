using BricksAndBalls.Bootstrap;
using BricksAndBalls.Configs.Sources;
using BricksAndBalls.Core.Interfaces;
using BricksAndBalls.Services;
using BricksAndBalls.Services.Audio;
using BricksAndBalls.Services.Config;
using BricksAndBalls.Services.Leaderboard;
using BricksAndBalls.Services.Logging;
using BricksAndBalls.Services.Progress;
using BricksAndBalls.Services.Scene;
using BricksAndBalls.Services.Settings;
using BricksAndBalls.Services.Storage;
using BricksAndBalls.UI.Popups;
using BricksAndBalls.UI.Services;
using UnityEngine;
using Zenject;

namespace BricksAndBalls.Installers
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private GameOverPopup _gameOverPopupPrefab;
        [SerializeField] private SettingsPopup _settingsPopupPrefab;

        public override void InstallBindings()
        {            
            // === Core bindings ===
            Container.Bind<IAppLogger>().To<UnityLogger>().AsSingle();
            Container.Bind<IStorageService>().To<PlayerPrefsStorageService>().AsSingle();
            
            // === Config providers ===
            Container.Bind<IGameConfigProvider>().To<ScriptableGameConfigProvider>().AsSingle();
            Container.Bind<IGameConfig>().FromMethod(GetGameConfig).AsSingle();
            Container.Bind<ILevelConfigSource>().To<DynamicLevelConfigSourceSelector>().AsSingle();
            Container.Bind<ILevelConfigProvider>().To<LevelConfigProvider>().AsSingle();
            Container.Bind<ILevelConfig>().FromMethod(GetLevelConfig).AsSingle();

            // === Services ===
            Container.Bind<IAddressablesService>().To<AddressablesService>().AsSingle();
            Container.Bind<IProgressService>().To<ProgressService>().AsSingle();
            Container.Bind<ISettingsService>().To<SettingsService>().AsSingle();
            Container.Bind<LeaderboardService>().AsSingle();
            Container.Bind<AudioManager>().AsSingle();
            Container.Bind<SceneLoader>().AsSingle();
            
            // === Bootstrap ===
            Container.BindInterfacesTo<BootstrapLoader>().AsSingle();

            // === Popup System ===
            Container.Bind<PopupManager>().AsSingle();
            Container.Bind<AddressablePopupProvider>().AsSingle();
            Container.Bind<PopupPrefabProvider>().AsSingle();
            Container.Bind<IPopupPrefabProvider>().To<CompositePopupProvider>().AsSingle();
            Container.Bind<IPopupService>().To<PopupService>().AsSingle();
            Container.BindInterfacesTo<PopupContainerInitializer>().AsSingle();
        }


        private IGameConfig GetGameConfig(InjectContext context)
        {
            var provider = context.Container.Resolve<IGameConfigProvider>();
            return provider.GetConfig();
        }
        
        private ILevelConfig GetLevelConfig()
        {
            return Container.Resolve<ILevelConfigProvider>().GetCurrentLevel();
        }
    }
}
