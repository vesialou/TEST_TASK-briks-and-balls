using UnityEngine;

namespace BricksAndBalls.Systems.Playfield
{
    public class GridGeometryBuilder
    {
        private readonly Rect _worldRect;
        private readonly int _gridWidth;
        private readonly int _gridHeight;
        private readonly int _bottomReserveRows;

        public float CellSize { get; private set; }
        public Vector3 GridOrigin { get; private set; }
        public Rect ActiveGridRect { get; private set; }
        public int TotalWorldRows { get; private set; }

        public GridGeometryBuilder(Rect worldRect, int gridWidth, int gridHeight, int bottomReserveRows)
        {
            _worldRect = worldRect;
            _gridWidth = gridWidth;
            _gridHeight = gridHeight;
            _bottomReserveRows = bottomReserveRows;

            Calculate();
        }

        private void Calculate()
        {
            var cellSizeByWidth = _worldRect.width / _gridWidth;
            var cellSizeByHeight = _worldRect.height / (_gridHeight + _bottomReserveRows);

            CellSize = Mathf.Min(cellSizeByWidth, cellSizeByHeight);

            TotalWorldRows = _gridHeight + _bottomReserveRows;

            var totalGridWidth = CellSize * _gridWidth;
            var totalGridHeight = CellSize * TotalWorldRows;

            var gridLeftX = _worldRect.center.x - (totalGridWidth / 2f);
            var gridTopY = _worldRect.yMax;

            GridOrigin = new Vector3(gridLeftX, gridTopY, 0);

            ActiveGridRect = new Rect(
                gridLeftX,
                gridTopY - (_gridHeight * CellSize),
                totalGridWidth,
                _gridHeight * CellSize
            );
        }

        public Vector3 GridToWorld(Vector2Int gridPos)
        {
            var worldX = GridOrigin.x + (gridPos.x + 0.5f) * CellSize;
            // NOTE: Y=0 top. Y move down.
            var worldY = GridOrigin.y - (gridPos.y + 0.5f) * CellSize;


            return new Vector3(worldX, worldY, 0);
        }

        public Vector2Int WorldToGrid(Vector3 worldPos)
        {
            var localX = worldPos.x - GridOrigin.x;
            var localY = GridOrigin.y - worldPos.y;

            var gridX = Mathf.FloorToInt(localX / CellSize);
            var gridY = Mathf.FloorToInt(localY / CellSize);

            gridX = Mathf.Clamp(gridX, 0, _gridWidth - 1);
            gridY = Mathf.Clamp(gridY, 0, TotalWorldRows - 1);

            return new Vector2Int(gridX, gridY);
        }

        public bool IsInActiveGrid(Vector2Int gridPos)
        {
            return gridPos.x >= 0 && gridPos.x < _gridWidth &&
                   gridPos.y >= 0 && gridPos.y < _gridHeight;
        }

        public bool IsInWorldBounds(Vector2Int gridPos)
        {
            return gridPos.x >= 0 && gridPos.x < _gridWidth &&
                   gridPos.y >= 0 && gridPos.y < TotalWorldRows;
        }
    }
}