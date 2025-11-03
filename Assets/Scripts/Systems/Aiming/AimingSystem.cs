using BricksAndBalls.Core.Interfaces;
using BricksAndBalls.Core.Models;
using UnityEngine;
using Zenject;

namespace BricksAndBalls.Systems.Ball
{
    public class AimingSystem : ITickable
    {
        private readonly GameStateMachine _stateMachine;
        private readonly BallLauncher _ballLauncher;
        private readonly IAppLogger _logger;

        private Vector2? _touchStartPos;
        private bool _isDragging;
        private Vector2 _currentDirection;
        private bool _hasValidDirection;
        private Vector2 _smoothedDirection;

        private LineRenderer _trajectoryLine;
        private GameObject _trajectoryObject;

        private const float MIN_SWIPE_DISTANCE = 0.3f;
        private const float MAX_RAY_DISTANCE = 30f;
        private const float LINE_WIDTH = 0.08f;
        private const float REFLECTION_OFFSET = 0.02f;
        private const float AIM_SENSITIVITY = 0.2f;
        
        
        private const float SMOOTHING_SPEED = 50f;


        public Vector2 CurrentDirection => _currentDirection;
        public bool HasValidDirection => _hasValidDirection;

        public AimingSystem(
            GameStateMachine stateMachine,
            BallLauncher ballLauncher,
            IAppLogger logger)
        {
            _stateMachine = stateMachine;
            _ballLauncher = ballLauncher;
            _logger = logger;

            CreateTrajectoryVisualizer();
        }

        private void CreateTrajectoryVisualizer()
        {
            _trajectoryObject = new GameObject("TrajectoryLine");
            _trajectoryLine = _trajectoryObject.AddComponent<LineRenderer>();
            
            _trajectoryLine.startWidth = LINE_WIDTH;
            _trajectoryLine.endWidth = LINE_WIDTH;
            _trajectoryLine.positionCount = 0;
            
            _trajectoryLine.material = new Material(Shader.Find("Sprites/Default"));
            _trajectoryLine.startColor = new Color(1f, 1f, 0f, 0.8f);
            _trajectoryLine.endColor = new Color(1f, 1f, 0f, 0.3f);
            
            _trajectoryLine.sortingOrder = 100;
            _trajectoryLine.textureMode = LineTextureMode.Tile;
            _trajectoryLine.enabled = false;

            _trajectoryObject.transform.position = new Vector3(0, 0, -1);
        }

        public void Tick()
        {
            if (!_stateMachine.CanShoot())
            {
                HideTrajectory();
                _isDragging = false;
                _touchStartPos = null;
                return;
            }

            HandleInput();
        }

        private void HandleInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                StartAiming(Input.mousePosition);
            }
            else if (Input.GetMouseButton(0) && _isDragging)
            {
                UpdateAiming(Input.mousePosition);
            }
            else if (Input.GetMouseButtonUp(0) && _isDragging)
            {
                _isDragging = false;
                _touchStartPos = null;
            }

            if (Input.touchCount > 0)
            {
                var touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    StartAiming(touch.position);
                }
                else if (touch.phase == TouchPhase.Moved && _isDragging)
                {
                    UpdateAiming(touch.position);
                }
                else if ((touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) && _isDragging)
                {
                    _isDragging = false;
                    _touchStartPos = null;
                }
            }

            if (!_isDragging)
            {
                HideTrajectory();
            }
        }

        private void StartAiming(Vector2 screenPos)
        {
            _touchStartPos = screenPos;
            _isDragging = true;
            _hasValidDirection = false;
            
            _logger.Log($"Aiming started at {screenPos}");
        }

        private void UpdateAiming(Vector2 currentScreenPos)
        {
            if (!_touchStartPos.HasValue)
            {
                return;
            }

            if (_touchStartPos.Value.y > Screen.height * 0.6f)
            {
                return;
            }

            var rawDelta = currentScreenPos - _touchStartPos.Value;
            var rawDistance = rawDelta.magnitude;

            if (rawDistance < MIN_SWIPE_DISTANCE)
            {
                _hasValidDirection = false;
                HideTrajectory();
                return;
            }

            var delta = rawDelta * AIM_SENSITIVITY;
            var targetDir = delta.normalized;
            if (targetDir.y <= 0.25f)
            {
                _hasValidDirection = false;
                HideTrajectory();
                return;
            }

            _smoothedDirection = Vector2.Lerp(_smoothedDirection, targetDir, Time.deltaTime * SMOOTHING_SPEED);

            _currentDirection = _smoothedDirection.normalized;
            _hasValidDirection = true;
            ShowTrajectory(_ballLauncher.GetLaunchPosition(), _currentDirection);
        }

        private void ShowTrajectory(Vector3 startPos, Vector2 direction)
        {
            _trajectoryLine.enabled = true;
            var points = CalculateTrajectoryWithReflection(startPos, direction);
            
            _trajectoryLine.positionCount = points.Length;
            _trajectoryLine.SetPositions(points);
        }

        private Vector3[] CalculateTrajectoryWithReflection(Vector3 startPos, Vector2 direction)
        {
            var currentPos = startPos;
            var currentDir = direction.normalized;
            
            var layerMask = LayerMask.GetMask("Wall", "Brick");
            
            var hit = Physics2D.Raycast(
                currentPos, 
                currentDir, 
                MAX_RAY_DISTANCE,
                layerMask);
            
            if (!hit)
            {
                var endPos = currentPos + (Vector3)(currentDir * MAX_RAY_DISTANCE);
                return new Vector3[] { currentPos, endPos };
            }

            Vector3 firstHitPoint = hit.point;
            var reflectedDir = Vector2.Reflect(currentDir, hit.normal);
            var reflectionStart = firstHitPoint + (Vector3)(reflectedDir * REFLECTION_OFFSET);
            var secondHit = Physics2D.Raycast(
                reflectionStart, 
                reflectedDir, 
                MAX_RAY_DISTANCE,
                layerMask);
            
            Vector3 secondEndPos;
            if (secondHit)
            {
                secondEndPos = secondHit.point;
            }
            else
            {
                secondEndPos = reflectionStart + (Vector3)(reflectedDir * MAX_RAY_DISTANCE);
            }

            return new [] 
            { 
                currentPos, 
                firstHitPoint, 
                secondEndPos 
            };
        }

        private void HideTrajectory()
        {
            if (_trajectoryLine != null && _trajectoryLine.enabled)
            {
                _trajectoryLine.enabled = false;
            }
        }

        public void Dispose()
        {
            if (_trajectoryObject != null)
            {
                Object.Destroy(_trajectoryObject);
            }
        }

        public Vector2 GetLaunchDirection()
        {
            return _hasValidDirection ? _currentDirection : Vector2.up;
        }

        public void Reset()
        {
            _isDragging = false;
            _touchStartPos = null;
            _hasValidDirection = false;
            HideTrajectory();
        }
    }
}