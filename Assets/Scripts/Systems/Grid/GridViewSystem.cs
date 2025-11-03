using System;
using BricksAndBalls.Core.Interfaces;
using BricksAndBalls.Core.Signals;
using DG.Tweening;
using Zenject;

namespace BricksAndBalls.Systems.Grid
{
    public class GridViewSystem : IInitializable, IDisposable
    {
        private readonly SignalBus _bus;
        private readonly GridWorldBuilder _worldBuilder;
        private readonly IGameConfig _config;
        private readonly BrickViewRegistry _registry;

        public GridViewSystem(
            SignalBus bus,
            GridWorldBuilder worldBuilder,
            IGameConfig config,
            BrickViewRegistry registry)
        {
            _bus = bus;
            _worldBuilder = worldBuilder;
            _config = config;
            _registry = registry;
        }

        public void Initialize()
        {
            _bus.Subscribe<GridBrickMovedSignal>(OnBrickMoved);
        }

        private void OnBrickMoved(GridBrickMovedSignal signal)
        {
        }

        public void Dispose()
        {
            _bus.TryUnsubscribe<GridBrickMovedSignal>(OnBrickMoved);
        }
    }
}