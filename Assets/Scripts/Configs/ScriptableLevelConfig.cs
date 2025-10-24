using System.Collections.Generic;
using BricksAndBalls.Core.Interfaces;
using BricksAndBalls.Core.Models;
using UnityEngine;

namespace BricksAndBalls.Configs
{
    [CreateAssetMenu(fileName = "Level_00000", menuName = "Game/LevelConfig")]
    public class ScriptableLevelConfig : ScriptableObject, ILevelConfig
    {
        [SerializeField] private List<BrickData> _briks;
        public List<BrickData> GetLevelData() => _briks;
    }
}