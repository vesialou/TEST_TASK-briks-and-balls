using System;
using UnityEngine;

namespace BricksAndBalls.Core.Models
{
    public class BrickModel
    {
        public int ID { get; set; }
        public Vector2Int GridPosition { get; set; }
        public int MaxHP { get; set; }
        public int CurrentHP { get; set; }
        public bool IsAlive { get; set; }
        public BrickType Type { get; set; }

        public event Action<int> OnHPChanged;
        public event Action OnDeath;

        public void TakeDamage(int damage)
        {
            CurrentHP -= damage;
            OnHPChanged?.Invoke(CurrentHP);

            if (CurrentHP <= 0)
            {
                IsAlive = false;
                OnDeath?.Invoke();
            }
        }
    }

    public enum BrickType
    {
        Normal = 0,
    }
}
