using BricksAndBalls.Core.Models;
using BricksAndBalls.Core.Signals;
using TMPro;
using UnityEngine;
using Zenject;

namespace BricksAndBalls.UI
{
    public class GameHUD : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private TextMeshProUGUI _roundText;

        [Inject] private SignalBus _signalBus;
        [Inject] private GameSession _gameSession;

        private void Start()
        {
            _signalBus.Subscribe<ScoreChangedSignal>(OnScoreChanged);
            _signalBus.Subscribe<RoundEndedSignal>(OnRoundEnded);
            UpdateRoundDisplay();
            UpdateScoreDisplay(0);
        }

        private void OnDestroy()
        {
            _signalBus.TryUnsubscribe<ScoreChangedSignal>(OnScoreChanged);
            _signalBus.TryUnsubscribe<RoundEndedSignal>(OnRoundEnded);
        }

        private void OnRoundEnded()
        {
            UpdateRoundDisplay();
        }

        private void OnScoreChanged(ScoreChangedSignal signal)
        {
            UpdateScoreDisplay(signal.Score);
        }

        public void UpdateRoundDisplay()
        {
            if (_roundText != null)
            {
                _roundText.text = $"Round: {_gameSession.CurrentRound + 1}/{_gameSession.MaxRounds}";
            }
        }

        private void UpdateScoreDisplay(int score)
        {
            if (_scoreText != null)
            {
                _scoreText.text = $"Score: {score}";
            }
        }
    }
}