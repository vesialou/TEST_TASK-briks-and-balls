using BricksAndBalls.Core.Signals;
using Zenject;

namespace BricksAndBalls.Installers
{
    public class SignalBusInstaller  : MonoInstaller
    {
        public override void InstallBindings()
        {
            Zenject.SignalBusInstaller.Install(Container);

            Container.DeclareSignal<BrickDestroyedSignal>().OptionalSubscriber();
            Container.DeclareSignal<RoundEndedSignal>();
            Container.DeclareSignal<GameOverSignal>();
            Container.DeclareSignal<ScoreChangedSignal>();
            Container.DeclareSignal<GridDescendedSignal>().OptionalSubscriber();
            Container.DeclareSignal<GridBrickMovedSignal>();
            Container.DeclareSignal<LastBallLaunchedSignal>();
            Container.DeclareSignal<FirstBallHitBottomSignal>();
            Container.DeclareSignal<BallReturnedToPoolSignal>();
            Container.DeclareSignal<BrickDestroyedForPoolSignal>();
            Container.DeclareSignal<GridReachedBottomSignal>();
        }
    }
}