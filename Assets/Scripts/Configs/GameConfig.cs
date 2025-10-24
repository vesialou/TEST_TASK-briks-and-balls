using System.Collections.Generic;
using BricksAndBalls.Core.Interfaces;
using UnityEngine;

namespace BricksAndBalls.Configs
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Game/Game Config")]
    public class GameConfig : ScriptableObject, IGameConfig
    {
        [Header("Ball Settings")] [SerializeField]
        private int _ballsCount = 10;

        [SerializeField] private float _ballSpeed = 10f;

        [Header("Grid Settings")] [SerializeField]
        private int _gridSize = 10;

        [SerializeField] private float _cellSize = 1f;
        [Range(0f, 0.3f)] [SerializeField] private float _topMargin = 0.1f;
        [Range(0f, 0.2f)] [SerializeField] private float _sideMargin = 0.05f;
        [SerializeField] private int _bottomReserveRows = 10;

        [Header("Grid Positioning")] [SerializeField]
        private Vector2 _gridCenter = Vector2.zero;

        [Header("Camera Settings")] [SerializeField] [Range(0f, 0.3f)]
        private float _cameraPaddingPercent = 0.1f;

        [SerializeField] [Range(0f, 5f)] private float _cameraTopUIMargin = 2f;
        [SerializeField] [Range(0f, 5f)] private float _cameraBottomUIMargin = 2f;

        [Header("Gameplay")] [SerializeField] private int _roundsCount = 3;
        [SerializeField] private float _rowDescendSpeed = 0.3f;

        [Header("Play Rewards")] [SerializeField]
        private int _hitBrickReward = 1;

        [SerializeField] private int _destroyBrickReward = 5;
        [SerializeField] private List<int> _multipliers = new() { 1, 3, 5 };

        public int BallsCount => _ballsCount;
        public float BallSpeed => _ballSpeed;
        public int GridSize => _gridSize;
        public float CellSize => _cellSize;
        public float TopMargin => _topMargin;
        public float SideMargin => _sideMargin;
        public int RoundsCount => _roundsCount;
        public float RowDescendSpeed => _rowDescendSpeed;
        public int HitBrickReward => _hitBrickReward;
        public int DestroyBrickReward => _destroyBrickReward;
        public int BottomReserveRows => _bottomReserveRows;
        public float CameraPaddingPercent => _cameraPaddingPercent;
        public float CameraTopUIMargin => _cameraTopUIMargin;
        public float CameraBottomUIMargin => _cameraBottomUIMargin;
        public Vector2 GridCenter => _gridCenter;
        public int[] Multipliers => _multipliers.ToArray();
    }
}