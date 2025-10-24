using UnityEngine;
using Zenject;

namespace BricksAndBalls.Systems.Ball
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class LaunchPointMarker : MonoBehaviour
    {
        private SpriteRenderer _renderer;
        private BallLauncher _launcher;

        [Inject]
        public void Construct(BallLauncher launcher)
        {
            _launcher = launcher;
        }

        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            UpdatePosition( _launcher.GetLaunchPosition());
        }

        public void UpdatePosition(Vector3 newPos)
        {
            transform.position = newPos;
        }    
        
        public void Show()
        {
            if (_renderer != null)
            {
                _renderer.enabled = true;
            }
        }

        public void Hide()
        {
            if (_renderer != null)
            {
                _renderer.enabled = false;
            }
        }
    }
}