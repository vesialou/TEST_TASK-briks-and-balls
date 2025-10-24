using System.Collections.Generic;
using UnityEngine;

namespace BricksAndBalls.Systems.Grid
{
    public class GridSystem
    {
        private readonly Dictionary<Vector2Int, int> _occupancy = new();
        private readonly Dictionary<int, Vector2Int> _positions = new();

        private readonly List<(int id, Vector2Int pos)> _snapshotBuffer = new(512);

        public void Register(int brickID, Vector2Int pos)
        {
            _occupancy[pos] = brickID;
            _positions[brickID] = pos;
        }

        public void Unregister(int id)
        {
            if (_positions.TryGetValue(id, out var pos))
            {
                _positions.Remove(id);
                _occupancy.Remove(pos);
            }
        }

        public bool TryGetPosition(int id, out Vector2Int pos)
        {
            return _positions.TryGetValue(id, out pos);
        }

        public void MoveBrick(int id, Vector2Int newPos)
        {
            if (!_positions.TryGetValue(id, out var oldPos))
            {
                return;
            }

            _occupancy.Remove(oldPos);
            _occupancy[newPos] = id;
            _positions[id] = newPos;
        }

        /// <summary>
        /// Возвращает snapshot в переиспользуемый буфер.
        /// После использования — не сохраняй ссылку!
        /// </summary>
        public IReadOnlyList<(int id, Vector2Int pos)> GetSnapshotBuffer()
        {
            _snapshotBuffer.Clear();
            foreach (var kvp in _positions)
                _snapshotBuffer.Add((kvp.Key, kvp.Value));
            return _snapshotBuffer;
        }
    }
}