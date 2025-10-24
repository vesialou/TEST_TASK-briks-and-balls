using System;
using BricksAndBalls.Core.Interfaces;
using BricksAndBalls.Core.Signals;
using BricksAndBalls.Factories;
using Zenject;

namespace BricksAndBalls.Systems.Ball
{
    public class BallLifecycleManager : IInitializable, IDisposable
    {
        private readonly SignalBus _signalBus;
        private readonly BallFactory _ballFactory;
        private readonly IAppLogger _logger;

        public BallLifecycleManager(
            SignalBus signalBus,
            BallFactory ballFactory,
            IAppLogger logger)
        {
            _signalBus = signalBus;
            _ballFactory = ballFactory;
            _logger = logger;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<BallReturnedToPoolSignal>(OnBallReturnedToPool);
            
            _logger.Log("BallLifecycleManager initialized");
        }

        public void Dispose()
        {
            _signalBus.TryUnsubscribe<BallReturnedToPoolSignal>(OnBallReturnedToPool);
        }

        private void OnBallReturnedToPool(BallReturnedToPoolSignal signal)
        {
            _logger.Log($"BallLifecycleManager: Returning ball {signal.BallID} to pool");
            
            _ballFactory.Release(signal.BallID);
        }
    }
}