using System.Collections.Generic;
using BricksAndBalls.Core.Interfaces;
using BricksAndBalls.Services.Leaderboard;
using BricksAndBalls.UI.Popups.Leaderboard;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace BricksAndBalls.UI.Popups
{
    public class LeaderboardPopup : BasePopup
    {
        [SerializeField] private Button _CloseButton;
        [SerializeField] private GameObject _entityPrefab;
        
        [SerializeField] private LeaderboardScroll _scroll;

        [Inject]
        public void Construct(IAppLogger logger)
        {
            
        }
        
        public void ShowLeaderboard(List<LeaderboardEntry> players)
        {
            _CloseButton.onClick.AddListener(Close);
            _scroll.SetData(players);
        }

    }
}