using System.Collections.Generic;
using BricksAndBalls.Core.Interfaces;
using UnityEngine;
using Zenject;

namespace BricksAndBalls.Systems.Playfield
{
    public class PlayfieldBoundsReader : MonoBehaviour
    {
        [SerializeField] private List<WallBounds> _walls = new List<WallBounds>();
        public List<WallBounds> Walls => _walls;
        [Inject] private IAppLogger _logger;

        public float MinX { get; private set; }
        public float MaxX { get; private set; }
        public float MinY { get; private set; }
        public float MaxY { get; private set; }
        public Vector2 Center { get; private set; }
        public Rect WorldRect { get; private set; }

#if UNITY_EDITOR
        private void Awake()
        {
            CalculateBounds();
        }
        
        [ContextMenu("Recalculate Bounds (Debug)")]
        public void CalculateBounds()
        {
            var minX = float.MaxValue;
            var maxX = float.MinValue;
            var minY = float.MaxValue;
            var maxY = float.MinValue;

            foreach (var wall in _walls)
            {
                var bounds = wall.Collider.bounds;

                switch (wall.Type)
                {
                    case WallBounds.WallType.Top:
                        maxY = Mathf.Max(maxY, bounds.min.y);
                        break;

                    case WallBounds.WallType.Bottom:
                        minY = Mathf.Min(minY, bounds.max.y);
                        break;

                    case WallBounds.WallType.Left:
                        minX = Mathf.Min(minX, bounds.max.x);
                        break;

                    case WallBounds.WallType.Right:
                        maxX = Mathf.Max(maxX, bounds.min.x);
                        break;
                }
            }

            MinX = minX;
            MaxX = maxX;
            MinY = minY;
            MaxY = maxY;

            Center = new Vector2((MinX + MaxX) / 2f, (MinY + MaxY) / 2f);
            WorldRect = new Rect(
                MinX,
                MinY,
                MaxX - MinX,
                MaxY - MinY);

            _logger.Log($"PlayfieldBoundsReader: Bounds calculated - X:[{MinX:F2}, {MaxX:F2}], Y:[{MinY:F2}, {MaxY:F2}], Size:{WorldRect.width:F2}x{WorldRect.height:F2}");
        }
#endif
        private void OnDrawGizmos()
        {
            if (WorldRect.width <= 0 || WorldRect.height <= 0)
            {
                return;
            }

            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(
                Center,
                new Vector3(
                    WorldRect.width,
                    WorldRect.height,
                    0));

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(
                new Vector3(
                    MinX,
                    MaxY,
                    0),
                new Vector3(
                    MaxX,
                    MaxY,
                    0));
            Gizmos.DrawLine(
                new Vector3(
                    MinX,
                    MinY,
                    0),
                new Vector3(
                    MaxX,
                    MinY,
                    0));
            Gizmos.DrawLine(
                new Vector3(
                    MinX,
                    MinY,
                    0),
                new Vector3(
                    MinX,
                    MaxY,
                    0));
            Gizmos.DrawLine(
                new Vector3(
                    MaxX,
                    MinY,
                    0),
                new Vector3(
                    MaxX,
                    MaxY,
                    0));
        }
    }
}