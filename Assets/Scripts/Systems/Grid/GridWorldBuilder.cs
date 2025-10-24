using BricksAndBalls.Core.Interfaces;
using BricksAndBalls.Systems.Playfield;
using UnityEngine;
using Zenject;

namespace BricksAndBalls.Systems.Grid
{
    public class GridWorldBuilder
    {
        private PlayfieldManager _playfieldManager;
        private IAppLogger _logger;

        public Vector2 GridWorldSize => new Vector2(
            _playfieldManager.CellSize * _playfieldManager.GridWidth,
            _playfieldManager.CellSize * _playfieldManager.GridHeight
        );

        public Vector3 GridWorldCenter => new Vector3(
            _playfieldManager.ActiveGridRect.center.x,
            _playfieldManager.ActiveGridRect.center.y,
            0
        );

        public float CellSize => _playfieldManager.CellSize;
        public int GridSize => _playfieldManager.GridWidth;

        [Inject]
        public void Construct(PlayfieldManager playfieldManager, IAppLogger logger)
        {
            _playfieldManager = playfieldManager;
            _logger = logger;

            _logger.Log("GridWorldBuilder initialized with PlayfieldManager");
        }

        public Vector3 GridToWorld(Vector2Int gridPos)
        {
            return _playfieldManager.GridToWorld(gridPos);
        }

        public Vector2Int WorldToGrid(Vector3 worldPos)
        {
            return _playfieldManager.WorldToGrid(worldPos);
        }

        public bool IsValidGridPosition(Vector2Int gridPos)
        {
            return _playfieldManager.IsInActiveGrid(gridPos);
        }
    }
}