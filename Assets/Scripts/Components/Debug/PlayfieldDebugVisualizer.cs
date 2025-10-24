using BricksAndBalls.Systems.Playfield;
using UnityEngine;
using Zenject;

namespace BricksAndBalls.Components.Debug
{
    public class PlayfieldDebugVisualizer : MonoBehaviour
    {
        [Inject] private PlayfieldManager _playfieldManager;

        [Header("Visualization Settings")]
        [SerializeField] private bool _showWorldRect = true;
        [SerializeField] private bool _showActiveGrid = true;
        [SerializeField] private bool _showGridLines = true;
        [SerializeField] private bool _showReserveRows = true;
        [SerializeField] private bool _showCameraView = true;

        private void OnDrawGizmos()
        {
            if (_playfieldManager == null)
            {
                return;
            }

            if (_showWorldRect)
            {
                DrawWorldRect();
            }

            if (_showActiveGrid)
            {
                DrawActiveGrid();
            }

            if (_showGridLines || _showReserveRows || _showCameraView)
            {
                _playfieldManager.DrawDebugGizmos();
            }
        }

        private void DrawWorldRect()
        {
            var worldRect = _playfieldManager.WorldRect;
            if (worldRect.width <= 0 || worldRect.height <= 0)
            {
                return;
            }

            Gizmos.color = Color.cyan;
            var center = new Vector3(worldRect.center.x, worldRect.center.y, 0);
            var size = new Vector3(worldRect.width, worldRect.height, 0);
            Gizmos.DrawWireCube(center, size);
        }

        private void DrawActiveGrid()
        {
            var activeRect = _playfieldManager.ActiveGridRect;
            if (activeRect.width <= 0 || activeRect.height <= 0)
            {
                return;
            }

            Gizmos.color = Color.green;
            var center = new Vector3(activeRect.center.x, activeRect.center.y, 0);
            var size = new Vector3(activeRect.width, activeRect.height, 0);
            Gizmos.DrawWireCube(center, size);
        }
    }
}