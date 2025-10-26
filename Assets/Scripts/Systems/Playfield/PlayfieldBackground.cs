using UnityEngine;
using Zenject;

namespace BricksAndBalls.Systems.Playfield
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class PlayfieldBackground : MonoBehaviour
    {
        private SpriteRenderer _renderer;
        private PlayfieldManager _playfieldManager;

        [Inject]
        public void Construct(PlayfieldManager playfieldManager)
        {
            _playfieldManager = playfieldManager;
        }

        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            AlignToWorldRect(_playfieldManager.WorldRect);
        }

        public void AlignToWorldRect(Rect worldRect)
        {
            if (_renderer == null || _renderer.sprite == null)
            {
                return;
            }

            transform.position = new Vector3(worldRect.center.x, worldRect.center.y, 10f);

            _renderer.drawMode = SpriteDrawMode.Tiled;
            _renderer.size = new Vector2(worldRect.width, worldRect.height);
        }
    }
}