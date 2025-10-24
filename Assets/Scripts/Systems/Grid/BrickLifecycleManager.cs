using System;
using BricksAndBalls.Core.Interfaces;
using BricksAndBalls.Core.Signals;
using BricksAndBalls.Factories;
using Zenject;

namespace BricksAndBalls.Systems.Grid
{
    public class BrickLifecycleManager : IInitializable, IDisposable
    {
        private readonly SignalBus _signalBus;
        private readonly BrickFactory _brickFactory;
        private readonly IAppLogger _logger;

        public BrickLifecycleManager(
            SignalBus signalBus,
            BrickFactory brickFactory,
            IAppLogger logger)
        {
            _signalBus = signalBus;
            _brickFactory = brickFactory;
            _logger = logger;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<BrickDestroyedForPoolSignal>(OnBrickDestroyedForPool);
            
            _logger.Log("BrickLifecycleManager initialized");
        }

        public void Dispose()
        {
            _signalBus.TryUnsubscribe<BrickDestroyedForPoolSignal>(OnBrickDestroyedForPool);
        }

        private void OnBrickDestroyedForPool(BrickDestroyedForPoolSignal signal)
        {
            _logger.Log($"BrickLifecycleManager: Returning brick {signal.BrickID} to pool");
            
            _brickFactory.Release(signal.BrickID);
        }
    }
}