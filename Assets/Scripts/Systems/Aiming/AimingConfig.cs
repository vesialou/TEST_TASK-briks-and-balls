using UnityEngine;

namespace BricksAndBalls.Configs
{
    [CreateAssetMenu(fileName = "AimingConfig", menuName = "Game/Aiming Config")]
    public class AimingConfig : ScriptableObject
    {
        [Header("Input Settings")]
        [SerializeField] [Range(0.1f, 2f)] 
        private float _minSwipeDistance = 0.5f;
        
        [Header("Trajectory Visualization")]
        [SerializeField] [Range(5f, 50f)] 
        private float _maxRayDistance = 20f;
        
        [SerializeField] [Range(0.01f, 0.2f)] 
        private float _lineWidth = 0.05f;
        
        [SerializeField] 
        private Color _lineStartColor = new Color(1f, 1f, 1f, 0.8f);
        
        [SerializeField] 
        private Color _lineEndColor = new Color(1f, 1f, 1f, 0.2f);
        
        [SerializeField] [Range(0, 200)] 
        private int _lineSortingOrder = 100;

        [Header("Reflection Settings")]
        [SerializeField] [Range(0f, 0.1f)]
        private float _reflectionOffset = 0.01f;
        
        [SerializeField]
        private bool _showSecondReflection = false;

        public float MinSwipeDistance => _minSwipeDistance;
        public float MaxRayDistance => _maxRayDistance;
        public float LineWidth => _lineWidth;
        public Color LineStartColor => _lineStartColor;
        public Color LineEndColor => _lineEndColor;
        public int LineSortingOrder => _lineSortingOrder;
        public float ReflectionOffset => _reflectionOffset;
        public bool ShowSecondReflection => _showSecondReflection;
    }
}