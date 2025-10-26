using BricksAndBalls.Core.Interfaces;
using UnityEngine;
using Zenject;

namespace BricksAndBalls.Systems.Playfield
{
    public class PlayfieldManager : IInitializable
    {
        private readonly WallAutoAligner _wallAutoAligner;
        private readonly IGameConfig _gameConfig;
        private readonly Camera _camera;
        private readonly IAppLogger _logger;

        private GridGeometryBuilder _gridBuilder;
        private CameraPlayfieldFitter _cameraFitter;

        public Rect WorldRect { get; private set; }
        public Rect ActiveGridRect { get; private set; }
        public float CellSize { get; private set; }
        public Vector3 GridOrigin { get; private set; }
        public int GridWidth { get; private set; }
        public int GridHeight { get; private set; }
        public int BottomReserveRows { get; private set; }
        public int TotalWorldRows { get; private set; }

        public PlayfieldManager(
            WallAutoAligner wallAutoAligner,
            IGameConfig gameConfig,
            Camera camera,
            IAppLogger logger)
        {
            _wallAutoAligner = wallAutoAligner;
            _gameConfig = gameConfig;
            _camera = camera;
            _logger = logger;
        }

        public void Initialize()
        {
            _logger.Log("PlayfieldManager: Initializing...");

            GridWidth = _gameConfig.GridSize;
            GridHeight = _gameConfig.GridSize;
            BottomReserveRows = _gameConfig.BottomReserveRows;

            var cellSize = _gameConfig.CellSize;
            var gridWorldWidth = GridWidth * cellSize;
            var gridWorldHeight = (GridHeight + BottomReserveRows) * cellSize;

            var gridCenter = _gameConfig.GridCenter;

            var playableRect = new Rect(
                gridCenter.x - gridWorldWidth / 2f,
                gridCenter.y - gridWorldHeight / 2f,
                gridWorldWidth,
                gridWorldHeight);

            var topUIMargin = _gameConfig.CameraTopUIMargin;
            var bottomUIMargin = _gameConfig.CameraBottomUIMargin;
            var sideMargin = gridWorldWidth * _gameConfig.SideMargin;

            WorldRect = new Rect(
                playableRect.x - sideMargin,
                playableRect.y - bottomUIMargin,
                playableRect.width + sideMargin * 2f,
                playableRect.height + topUIMargin + bottomUIMargin);

            _gridBuilder = new GridGeometryBuilder(
                playableRect,
                GridWidth,
                GridHeight,
                BottomReserveRows);

            CellSize = _gridBuilder.CellSize;
            GridOrigin = _gridBuilder.GridOrigin;
            ActiveGridRect = _gridBuilder.ActiveGridRect;
            TotalWorldRows = _gridBuilder.TotalWorldRows;

            _cameraFitter = new CameraPlayfieldFitter(
                _camera,
                _logger,
                _gameConfig.CameraPaddingPercent,
                _gameConfig.CameraTopUIMargin,
                _gameConfig.CameraBottomUIMargin);
            _cameraFitter.FitToPlayfield(WorldRect);

            if (!_cameraFitter.IsRectFullyVisible(playableRect))
            {
                _logger.LogWarning("PlayfieldManager: Playable area is not fully visible in camera!");
            }

            _wallAutoAligner.AlignToWorldRect(WorldRect);
        }

        public Vector3 GridToWorld(Vector2Int gridPos)
        {
            return _gridBuilder.GridToWorld(gridPos);
        }

        public Vector2Int WorldToGrid(Vector3 worldPos)
        {
            return _gridBuilder.WorldToGrid(worldPos);
        }

        public bool IsInActiveGrid(Vector2Int gridPos)
        {
            return _gridBuilder.IsInActiveGrid(gridPos);
        }

        public bool IsInWorldBounds(Vector2Int gridPos)
        {
            return _gridBuilder.IsInWorldBounds(gridPos);
        }

        public void RecalculateForAspectRatio()
        {
            _logger.Log("PlayfieldManager: Recalculating for aspect ratio change...");
            _cameraFitter.FitToPlayfield(WorldRect);
        }

        public void DrawDebugGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(
                new Vector3(
                    ActiveGridRect.center.x,
                    ActiveGridRect.center.y,
                    0),
                new Vector3(
                    ActiveGridRect.width,
                    ActiveGridRect.height,
                    0));

            Gizmos.color = new Color(
                0.5f,
                1f,
                0.5f,
                0.3f);

            for (var x = 0; x <= GridWidth; x++)
            {
                var top = GridToWorld(new Vector2Int(x, 0));
                var bottom = GridToWorld(new Vector2Int(x, TotalWorldRows));

                top.y += CellSize * 0.5f;
                bottom.y -= CellSize * 0.5f;

                Gizmos.DrawLine(top, bottom);
            }

            for (var y = 0; y <= TotalWorldRows; y++)
            {
                var left = GridToWorld(new Vector2Int(0, y));
                var right = GridToWorld(new Vector2Int(GridWidth, y));

                left.x -= CellSize * 0.5f;
                right.x += CellSize * 0.5f;

                if (y >= GridHeight)
                {
                    Gizmos.color = new Color(
                        1f,
                        0.5f,
                        0.5f,
                        0.3f);
                }
                else
                {
                    Gizmos.color = new Color(
                        0.5f,
                        1f,
                        0.5f,
                        0.3f);
                }

                Gizmos.DrawLine(left, right);
            }

            if (BottomReserveRows > 0)
            {
                Gizmos.color = Color.red;
                var reserveLineLeft = GridToWorld(new Vector2Int(0, GridHeight));
                var reserveLineRight = GridToWorld(new Vector2Int(GridWidth, GridHeight));

                reserveLineLeft.x -= CellSize * 0.5f;
                reserveLineRight.x += CellSize * 0.5f;

                Gizmos.DrawLine(reserveLineLeft, reserveLineRight);
            }

            Gizmos.color = Color.blue;
            var visibleRect = _cameraFitter.GetVisibleRect();
            Gizmos.DrawWireCube(
                new Vector3(
                    visibleRect.center.x,
                    visibleRect.center.y,
                    0),
                new Vector3(
                    visibleRect.width,
                    visibleRect.height,
                    0));
        }
    }
}