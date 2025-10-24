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
            if (!_registry.TryGet(signal.BrickID, out var view))
            {
                return;
            }

            var worldPos = _worldBuilder.GridToWorld(signal.NewPos);
            view.transform.DOMove(worldPos, _config.RowDescendSpeed).SetEase(Ease.OutQuad);
        }

        public void Dispose()
        {
            _bus.TryUnsubscribe<GridBrickMovedSignal>(OnBrickMoved);
        }
    }
}