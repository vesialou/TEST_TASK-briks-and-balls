using System;
using BricksAndBalls.Core.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace BricksAndBalls.UI.Popups
{
    public class GameOverPopup : BasePopup
    {
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private Button _menuButton;
        [SerializeField] private Button _playAgainButton;
        [SerializeField] private Button _playNextButton;

        [Inject] private IAppLogger _logger;

        public event Action OnMenuClicked;
        public event Action OnPlayAgainClicked;
        public event Action OnPlayNextClicked;

        private void Start()
        {
            _menuButton.onClick.AddListener(HandleMenuClick);
            _playAgainButton.onClick.AddListener(HandlePlayAgainClick);
            _playNextButton.onClick.AddListener(HandlePlayNextClick);
        }

        public void Setup(bool isWin, int finalScore, bool hasNextLevel)
        {
            _titleText.text = isWin ? "Victory!" : "Game Over";
            _scoreText.text = $"Score: {finalScore}";
            
            _playNextButton.gameObject.SetActive(hasNextLevel);
        }

        private void HandleMenuClick()
        {
            _logger.Log("GameOverPopup: Menu clicked");
            OnMenuClicked?.Invoke();
            Close();
        }

        private void HandlePlayAgainClick()
        {
            _logger.Log("GameOverPopup: Play Again clicked");
            OnPlayAgainClicked?.Invoke();
            Close();
        }

        private void HandlePlayNextClick()
        {
            _logger.Log("GameOverPopup: Play Next clicked");
            OnPlayNextClicked?.Invoke();
            Close();
        }

        protected void OnDestroy()
        {
            _menuButton.onClick.RemoveAllListeners();
            _playAgainButton.onClick.RemoveAllListeners();
            _playNextButton.onClick.RemoveAllListeners();
        }
    }
}