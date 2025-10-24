using UnityEngine;

namespace BricksAndBalls.Configs
{
    [CreateAssetMenu(menuName = "Game/Gameplay Objects Config")]
    public class GameplayObjectsConfig : ScriptableObject
    {
        public GameObject BrickPrefab;
        public GameObject BallPrefab;
    }
}