using System.Threading;
using BricksAndBalls.Core.Interfaces;
using BricksAndBalls.Core.Signals;
using BricksAndBalls.Systems.Playfield;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace BricksAndBalls.Systems.Grid
{
    public class GridMovementSystem
    {
        private readonly PlayfieldManager _playfieldManager;
        private readonly GridSystem _gridSystem;
        private readonly BrickViewRegistry _brickView;
        private readonly SignalBus _signalBus;
        private readonly IAppLogger _logger;

        private readonly int[] _brickBuffer = new int[512];
        private float _moveDuration = 0.5f;

        public GridMovementSystem(
            PlayfieldManager playfieldManager,
            GridSystem gridSystem,
            BrickViewRegistry brickView,
            SignalBus signalBus,
            IAppLogger logger)
        {
            _playfieldManager = playfieldManager;
            _gridSystem = gridSystem;
            _brickView = brickView;
            _signalBus = signalBus;
            _logger = logger;
        }

        public async UniTask MoveGridDownAsync(CancellationToken ct = default)
        {
            _logger.Log("GridMovementSystem: Moving grid down...");
            var seq = DOTween.Sequence().SetEase(Ease.OutQuad);
            var snapshot = _gridSystem.GetSnapshotBuffer();

            foreach (var (brickID, oldPos) in snapshot)
            {
                var newPos = oldPos + Vector2Int.up;
                _gridSystem.MoveBrick(brickID, newPos);
                if (_brickView.TryGet(brickID, out var view))
                {
                    var worldPos = _playfieldManager.GridToWorld(newPos);
                    seq.Join(view.transform.DOMove(worldPos, _moveDuration));
                }

                _signalBus.Fire(new GridBrickMovedSignal { BrickID = brickID, NewPos = newPos });
            }

            await using (ct.Register(() => { if (seq.IsActive())
                                         {
                                             seq.Kill();
                                         }
                                     }))
            {
                await seq.AsyncWaitForCompletion();
            }

            _logger.Log(" Grid moved down!");
            _signalBus.Fire<GridDescendedSignal>();
        }
    }
}