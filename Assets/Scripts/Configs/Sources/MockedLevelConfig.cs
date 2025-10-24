using System.Collections.Generic;
using BricksAndBalls.Core.Interfaces;
using BricksAndBalls.Core.Models;
using UnityEngine;

namespace Configs
{
    public class MockedLevelConfig : ILevelConfig
    {
        public List<BrickData> GetLevelData()
        {
            // Массив наличия блоков (1 = есть блок, 0 = пусто)
            var presence = new int[,]
            {
                // X: 0  1  2  3  4  5  6  7  8  9
                {    1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, // Y=0 (TOP)
                {    1, 0, 1, 0, 1, 0, 1, 0, 1, 0 }, // Y=1
                {    1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, // Y=2
                {    0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, // Y=3
                {    1, 0, 0, 1, 1, 1, 1, 0, 0, 1 }, // Y=4
                {    1, 0, 0, 1, 0, 0, 1, 0, 0, 1 }, // Y=5
                {    1, 0, 0, 1, 1, 1, 1, 0, 0, 1 }, // Y=6
                {    0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, // Y=7
                {    0, 1, 1, 1, 1, 1, 1, 1, 1, 0 }, // Y=8
                {    1, 1, 1, 0, 1, 1, 1, 1, 1, 1 }, // Y=9 (BOTTOM)
            };

            // Массив HP блоков
            var hp = new int[,]
            {
                // X: 0  1  2  3  4  5  6  7  8  9
                {     1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, // Y=0
                {     1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, // Y=1
                {     1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, // Y=2
                {     1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, // Y=3
                {     1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, // Y=4
                {     1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, // Y=5
                {     1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, // Y=6
                {     1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, // Y=7
                {     1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, // Y=8
                {     1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, // Y=9
            };

            // Массив типов блоков (0 = Normal, можно добавить другие типы)
            var types = new BrickType[,]
            {
                // X: 0  1  2  3  4  5  6  7  8  9
                {    0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, // Y=0
                {    0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, // Y=1
                {    0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, // Y=2
                {    0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, // Y=3
                {    0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, // Y=4
                {    0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, // Y=5
                {    0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, // Y=6
                {    0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, // Y=7
                {    0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, // Y=8
                {    0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, // Y=9
            };

            return BuildBricksFromArrays(presence, types, hp);
        }

        private List<BrickData> BuildBricksFromArrays(int[,] presence, BrickType[,] types, int[,] hp)
        {
            var bricks = new List<BrickData>();

            var rows = presence.GetLength(0);
            var cols = presence.GetLength(1);

            for (var y = 0; y < rows; y++)
            {
                for (var x = 0; x < cols; x++)
                {
                    if (presence[y, x] == 0)
                    {
                        continue;
                    }

                    bricks.Add(new BrickData
                    {
                        GridPosition = new Vector2Int(x, y),
                        HP = hp[y, x],
                        Type = types[y, x]
                    });
                }
            }

            return bricks;
        }
    }
}