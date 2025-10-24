using BricksAndBalls.Services.Leaderboard;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BricksAndBalls.UI.Popups.Leaderboard
{
    public class PlayerItemView : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private TMP_Text _rank;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _score;
        [SerializeField] private Image _background;

        [Header("Colors")]
        [SerializeField] private Color _firstPlaceColor = new Color(1f, 0.84f, 0f, 0.8f);
        [SerializeField] private Color _secondPlaceColor = new Color(0.75f, 0.75f, 0.75f, 0.8f);
        [SerializeField] private Color _thirdPlaceColor = new Color(0.8f, 0.5f, 0.2f, 0.8f);
        [SerializeField] private Color _playerColor = new Color(0.2f, 0.8f, 1f, 0.9f);
        [SerializeField] private Color _defaultColor = new Color(1f, 1f, 1f, 0.4f);

        public RectTransform Rect => (RectTransform)transform;

        public void SetData(LeaderboardEntry data, int rank)
        {
            _rank.text = rank.ToString();
            _name.text = data.PlayerName;
            _score.text = data.Score.ToString();

            ApplyColorByRank(rank, data.IsPlayer);
        }

        private void ApplyColorByRank(int rank, bool isPlayer)
        {
            if (_background == null)
            {
                Debug.LogWarning("Background Image not assigned in PlayerItemView!");
                return;
            }

            if (isPlayer)
            {
                _background.color = _playerColor;
                return;
            }

            switch (rank)
            {
                case 1:
                    _background.color = _firstPlaceColor;
                    break;
                case 2:
                    _background.color = _secondPlaceColor;
                    break;
                case 3:
                    _background.color = _thirdPlaceColor;
                    break;
                default:
                    _background.color = _defaultColor;
                    break;
            }
        }
    }
}