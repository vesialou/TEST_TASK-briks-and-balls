using BricksAndBalls.Core.Interfaces;
using BricksAndBalls.Systems.Grid;
using UnityEngine;
using Zenject;

namespace BricksAndBalls.Components.Debug
{
    public class GridDebugVisualizer : MonoBehaviour
    {
        private GridWorldBuilder _worldBuilder;
        private IGameConfig _config;

        [Inject]
        public void Construct(IGameConfig config, GridWorldBuilder worldBuilder)
        {
            _worldBuilder = worldBuilder;
            _config = config;
        }

        private void OnDrawGizmos()
        {
            if (_worldBuilder == null)
            {
                return;
            }

            Gizmos.color = Color.yellow;
            var center = _worldBuilder.GridWorldCenter;
            var size = new Vector3(
                _worldBuilder.GridWorldSize.x,
                _worldBuilder.GridWorldSize.y,
                0);
            Gizmos.DrawWireCube(center, size);

            Gizmos.color = new Color(
                0.5f,
                0.5f,
                0.5f,
                0.3f);
            for (var x = 0; x <= _config.GridSize; x++)
            {
                var start = _worldBuilder.GridToWorld(new Vector2Int(x, 0));
                var end = _worldBuilder.GridToWorld(new Vector2Int(x, _config.GridSize));

                start.y -= _config.CellSize / 2f;
                end.y += _config.CellSize / 2f;

                Gizmos.DrawLine(start, end);
            }

            for (var y = 0; y <= _config.GridSize; y++)
            {
                var start = _worldBuilder.GridToWorld(new Vector2Int(0, y));
                var end = _worldBuilder.GridToWorld(new Vector2Int(_config.GridSize, y));

                start.x -= _config.CellSize / 2f;
                end.x += _config.CellSize / 2f;

                Gizmos.DrawLine(start, end);
            }

            var cam = Camera.main;
            if (cam != null)
            {
                Gizmos.color = Color.green;
                var height = cam.orthographicSize * 2f;
                var width = height * cam.aspect;
                var camCenter = cam.transform.position;
                Gizmos.DrawWireCube(
                    camCenter,
                    new Vector3(
                        width,
                        height,
                        0));
            }
        }
    }
}