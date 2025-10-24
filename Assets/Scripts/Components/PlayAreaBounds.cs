using UnityEngine;
using Zenject;

namespace BricksAndBalls.Components
{
    public class PlayAreaBounds : MonoBehaviour
    {
        [Inject] private Camera _camera;
        [Inject] private ILogger _logger;

        private void Start()
        {
            CreateBounds();
        }

        private void CreateBounds()
        {
            var cameraHeight = _camera.orthographicSize * 2f;
            var cameraWidth = cameraHeight * _camera.aspect;
            var cameraCenter = _camera.transform.position;

            var boundThickness = 0.5f;

            CreateBound("TopBound",
                new Vector3(cameraCenter.x, cameraCenter.y + cameraHeight / 2f, 0),
                new Vector2(cameraWidth, boundThickness),
                false);

            CreateBound("LeftBound",
                new Vector3(cameraCenter.x - cameraWidth / 2f, cameraCenter.y, 0),
                new Vector2(boundThickness, cameraHeight),
                false);

            CreateBound("RightBound",
                new Vector3(cameraCenter.x + cameraWidth / 2f, cameraCenter.y, 0),
                new Vector2(boundThickness, cameraHeight),
                false);

            CreateBound("BottomBound",
                new Vector3(cameraCenter.x, cameraCenter.y - cameraHeight / 2f, 0),
                new Vector2(cameraWidth, boundThickness),
                true);

            _logger.Log("PlayAreaBounds created");
        }

        private void CreateBound(string name, Vector3 position, Vector2 size, bool isTrigger)
        {
            var bound = new GameObject(name);
            bound.transform.position = position;
            bound.transform.SetParent(transform);

            var collider = bound.AddComponent<BoxCollider2D>();
            collider.size = size;
            collider.isTrigger = isTrigger;

            if (isTrigger)
            {
                bound.tag = "PlayArea";
            }
        }
    }
}