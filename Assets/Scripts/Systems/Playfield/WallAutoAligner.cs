using System.Collections.Generic;
using UnityEngine;

namespace BricksAndBalls.Systems.Playfield
{
    public class WallAutoAligner
    {
        private readonly List<WallBounds> _walls;

        public WallAutoAligner(List<WallBounds> walls)
        {
            _walls = walls;
        }

        public void AlignToWorldRect(Rect worldRect, float wallThickness = 0.5f)
        {
            foreach (var wall in _walls)
            {
                switch (wall.Type)
                {
                    case WallBounds.WallType.Top:
                        wall.transform.position = new Vector3(worldRect.center.x, worldRect.yMax + wallThickness / 2f, 0);
                        wall.Collider.size = new Vector2(worldRect.width, wallThickness);
                        break;

                    case WallBounds.WallType.Bottom:
                        wall.transform.position = new Vector3(worldRect.center.x, worldRect.yMin - wallThickness / 2f, 0);
                        wall.Collider.size = new Vector2(worldRect.width, wallThickness);
                        break;

                    case WallBounds.WallType.Left:
                        wall.transform.position = new Vector3(worldRect.xMin - wallThickness / 2f, worldRect.center.y, 0);
                        wall.Collider.size = new Vector2(wallThickness, worldRect.height);
                        break;

                    case WallBounds.WallType.Right:
                        wall.transform.position = new Vector3(worldRect.xMax + wallThickness / 2f, worldRect.center.y, 0);
                        wall.Collider.size = new Vector2(wallThickness, worldRect.height);
                        break;
                    case WallBounds.WallType.Background:
                        var bg = wall.GetComponent<SpriteRenderer>();
                        if (bg != null)
                        {
                            var size = bg.sprite.bounds.size;
                            var scaleX = worldRect.width / size.x;
                            var scaleY = worldRect.height / size.y;

                            wall.transform.position = new Vector3(worldRect.center.x, worldRect.center.y, 10f);
                            wall.transform.localScale = new Vector3(scaleX, scaleY, 1f);
                        }
                        break;
                }
            }
        }
    }
}