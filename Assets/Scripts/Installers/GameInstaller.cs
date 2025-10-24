using BricksAndBalls.Configs;
using BricksAndBalls.Core.Models;
using BricksAndBalls.Factories;
using BricksAndBalls.Gameplay;
using BricksAndBalls.Systems.Ball;
using BricksAndBalls.Systems.Grid;
using BricksAndBalls.Systems.Physics;
using BricksAndBalls.Systems.Playfield;
using BricksAndBalls.Systems.Score;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace BricksAndBalls.Installers
{
    public class GameInstaller : MonoInstaller
    {
        [Header("Configs")] [SerializeField] private GameplayObjectsConfig _gameplayObjectsConfig;

        [Header("Scene References")] [SerializeField]
        private PlayfieldBoundsReader _boundsReader;

        [Header("Pool Settings")] [SerializeField]
        private Transform _ballPoolParent;

        [SerializeField] private Transform _brickPoolParent;

        public override void InstallBindings()
        {
            Container.BindInstance(_gameplayObjectsConfig).AsSingle();

            Container.Bind<Camera>().FromInstance(Camera.main).AsSingle();
            Container.BindInstance(_boundsReader).AsSingle();
            Container.Bind<WallAutoAligner>().AsSingle().WithArguments(_boundsReader.Walls);

            Container.BindInterfacesAndSelfTo<PlayfieldManager>().AsSingle().NonLazy();
            Container.Bind<GridWorldBuilder>().AsSingle().NonLazy();

            Container.Bind<GameplayData>().AsSingle();

            Container.Bind<ScoreManager>().AsSingle().NonLazy();

            Container.Bind<GridSystem>().AsSingle();
            Container.Bind<BrickViewRegistry>().AsSingle();
            Container.Bind<BrickPhysicsRegistry>().AsSingle();

            Container.Bind<BallPool>().AsSingle().WithArguments(_gameplayObjectsConfig.BallPrefab, _ballPoolParent).NonLazy();
            Container.Bind<BrickPool>().AsSingle().WithArguments(_gameplayObjectsConfig.BrickPrefab, _brickPoolParent).NonLazy();

            Container.Bind<BrickFactory>().AsSingle();
            Container.Bind<BallFactory>().AsSingle();

            Container.Bind<BallLauncher>().AsSingle();
            Container.Bind<GridMovementSystem>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<GridViewSystem>().AsSingle().NonLazy();
            Container.Bind<LaunchPointMarker>().FromComponentInHierarchy().AsSingle();

            Container.Bind<BallPhysicsSystem>().AsSingle().NonLazy();
            Container.Bind<BallVisualSystem>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<BrickPhysicsSystem>().AsSingle().NonLazy();

            Container.BindInterfacesTo<ScoreProcessingSystem>().AsSingle().NonLazy();

            Container.BindInterfacesAndSelfTo<AimingSystem>().AsSingle().NonLazy();
            Container.Bind<TimeScaleController>().AsSingle();

            Container.Bind<GameStateMachine>().AsSingle();
            Container.Bind<GameSession>().AsSingle();

            Container.BindInterfacesTo<ShootingController>().AsSingle().NonLazy();
            Container.BindInterfacesTo<GameController>().AsSingle().NonLazy();
            Container.BindInterfacesTo<LevelInitializer>().AsSingle().NonLazy();
            Container.BindInterfacesTo<BallLifecycleManager>().AsSingle().NonLazy();
            Container.BindInterfacesTo<BrickLifecycleManager>().AsSingle().NonLazy();
        }

        [ContextMenu("++LoadScene MainMenu")]
        public void Menu()
        {
            Container.Resolve<SceneManager>();
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }
    }
}