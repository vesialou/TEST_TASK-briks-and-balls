using BricksAndBalls.Core.Interfaces;
using UnityEngine;

namespace BricksAndBalls.Systems.Playfield
{
    public class CameraPlayfieldFitter
    {
        private readonly Camera _camera;
        private readonly IAppLogger _logger;
        private readonly float _paddingPercent;
        private readonly float _topUIMargin;
        private readonly float _bottomUIMargin;

        public CameraPlayfieldFitter(
            Camera camera, 
            IAppLogger logger,
            float paddingPercent = 0.1f,
            float topUIMargin = 2f,
            float bottomUIMargin = 2f)
        {
            _camera = camera;
            _logger = logger;
            _paddingPercent = paddingPercent;
            _topUIMargin = topUIMargin;
            _bottomUIMargin = bottomUIMargin;
        }
        public void FitToPlayfield(Rect worldRect)
        {
            // 1️⃣ Учитываем padding
            var paddedWidth = worldRect.width * (1f + _paddingPercent * 2f);
            var paddedHeight = worldRect.height * (1f + _paddingPercent * 2f);

            // 2️⃣ Вычисляем orthographicSize, чтобы камера точно вмещала поле
            var requiredSizeByWidth = paddedWidth / (2f * _camera.aspect);
            var requiredSizeByHeight = paddedHeight / 2f;

            // Берём большее значение — гарантированно влезет всё поле
            var orthographicSize = Mathf.Max(requiredSizeByWidth, requiredSizeByHeight);
            _camera.orthographicSize = orthographicSize;

            // 3️⃣ Центрируем камеру
            var cameraPosition = _camera.transform.position;
            cameraPosition.x = worldRect.center.x;
            cameraPosition.y = worldRect.center.y;
            _camera.transform.position = cameraPosition;

            _logger.Log($"[CameraPlayfieldFitter] Camera fitted: orthoSize={orthographicSize:F2}, aspect={_camera.aspect:F2}");
        }
        
        public void old_FitToPlayfield(Rect worldRect)
        {
            var requiredWidth = worldRect.width * (1f + _paddingPercent * 2f);
            
            var orthographicSize = requiredWidth / (2f * _camera.aspect);
            
            _camera.orthographicSize = orthographicSize;

            var cameraPosition = _camera.transform.position;
            cameraPosition.x = worldRect.center.x;
            
            var yOffset = (_topUIMargin - _bottomUIMargin) / 2f;
            cameraPosition.y = worldRect.center.y - yOffset;
            
            _camera.transform.position = cameraPosition;
        }
        
        public float GetVisibleHeight()
        {
            return _camera.orthographicSize * 2f;
        }
        
        public float GetVisibleWidth()
        {
            return GetVisibleHeight() * _camera.aspect;
        }
        
        public Rect GetVisibleRect()
        {
            var camPos = _camera.transform.position;
            var width = GetVisibleWidth();
            var height = GetVisibleHeight();

            return new Rect(
                camPos.x - width / 2f,
                camPos.y - height / 2f,
                width,
                height
            );
        }
        
        public Rect GetPlayableRect()
        {
            var camPos = _camera.transform.position;
            var width = GetVisibleWidth();
            var height = GetVisibleHeight();

            return new Rect(
                camPos.x - width / 2f,
                camPos.y - height / 2f + _bottomUIMargin,
                width,
                height - _topUIMargin - _bottomUIMargin
            );
        }
        
        public bool IsRectFullyVisible(Rect rect)
        {
            var playableRect = GetPlayableRect();
            
            return rect.xMin >= playableRect.xMin &&
                   rect.xMax <= playableRect.xMax &&
                   rect.yMin >= playableRect.yMin &&
                   rect.yMax <= playableRect.yMax;
        }
        
        public float GetPlayableTopY()
        {
            var camPos = _camera.transform.position;
            var height = GetVisibleHeight();
            return camPos.y + height / 2f - _topUIMargin;
        }
        
        public float GetPlayableBottomY()
        {
            var camPos = _camera.transform.position;
            var height = GetVisibleHeight();
            return camPos.y - height / 2f + _bottomUIMargin;
        }
    }
}