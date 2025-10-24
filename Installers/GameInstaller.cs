using BricksAndBalls.Configs;
using BricksAndBalls.Controllers;
using BricksAndBalls.Core;
using BricksAndBalls.Factories;
using BricksAndBalls.Systems.Ball;
using BricksAndBalls.Systems.Grid;
using BricksAndBalls.Systems.Physics;
using BricksAndBalls.Systems.Playfield;
using BricksAndBalls.Systems.Score;
using Game.Configs;
using UnityEngine;
using Zenject;

namespace BricksAndBalls.Installers
{
    public class GameInstaller : MonoInstaller
    {
        [Header("Configs")]
        [SerializeField] private MockedLevelConfig _levelConfig;
        [SerializeField] private BrickFactoryConfig _factoryConfig;

        [Header("Scene References")]
        [SerializeField] private PlayfieldBoundsReader _boundsReader;

        [Header("Playfield Settings")]
        [SerializeField] private int _bottomReserveRows = 3;
        [SerializeField][Range(0f, 0.3f)] private float _cameraPaddingPercent = 0.1f;

        public override void InstallBindings()
        {
            Container.QueueForInject(_levelConfig);
            Container.BindInstance(_levelConfig).AsSingle();
            Container.BindInstance(_factoryConfig).AsSingle();

            Container.Bind<Camera>().FromInstance(Camera.main).AsSingle();
            Container.BindInstance(_boundsReader).AsSingle();

            var playfieldSettings = new PlayfieldManager.Settings
            {
                BottomReserveRows = _bottomReserveRows,
                CameraPaddingPercent = _cameraPaddingPercent
            };
            Container.BindInstance(playfieldSettings).AsSingle();

            Container.BindInterfacesAndSelfTo<PlayfieldManager>().AsSingle().NonLazy();
            Container.Bind<GridWorldBuilder>().AsSingle().NonLazy();
            Container.Bind<GridSystem>().AsSingle();

            Container.Bind<BallPhysicsSystem>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<BrickPhysicsSystem>().AsSingle().NonLazy();

            Container.Bind<MockInputProvider>().AsSingle();
            Container.Bind<TimeScaleController>().AsSingle();
            Container.Bind<BallLauncher>().AsSingle();

            Container.Bind<BrickFactory>().AsSingle();
            Container.Bind<BallFactory>().AsSingle();
            Container.Bind<GameStateMachine>().AsSingle();
            Container.Bind<GameSession>().AsSingle();
            Container.Bind<ScoreManager>().AsSingle();
            Container.Bind<ShootingController>().AsSingle().NonLazy();
            Container.Bind<GameController>().AsSingle().NonLazy();
            Container.BindInterfacesTo<LevelInitializer>().AsSingle().NonLazy();
        }
    }
}