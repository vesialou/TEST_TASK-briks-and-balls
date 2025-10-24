using BricksAndBalls.Core.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace BricksAndBalls.UI.Popups
{
    public class MultiplayerPopup : BasePopup
    {
        [SerializeField] private Button _multiplier1xButton;
        [SerializeField] private Button _multiplier2xButton;
        [SerializeField] private Button _multiplier3xButton;
        [SerializeField] private TextMeshProUGUI _scoreText;

        private IAppLogger _logger;
        private Button[] _buttons;
        public int SelectedMult { get; private set; }

        [Inject]
        public void Construct(IAppLogger logger)
        {
            _logger = logger;
            _buttons = new[] { _multiplier1xButton, _multiplier2xButton, _multiplier3xButton };
        }

        public void Initialize(int[] configMultipliers, int score)
        {
            _scoreText.SetText($"Score: {score}");
            if (configMultipliers == null ||  configMultipliers.Length < 3 )
            {
                _logger.LogError("MultiplayerPopup: Invalid config multipliers!");
                Close();
                return;
            }

            for (var i = 0; i < configMultipliers.Length & i < _buttons.Length; i++)
            {
                SetText(_buttons[i], $"x{configMultipliers[i]}");
                var index = i;
                _buttons[i].onClick.AddListener(() => SelectMultiplier(index));
            }
        }

        private void SetText(Button btn, string text)
        {
            var tmp = btn.GetComponentInChildren<TMP_Text>();
            tmp.SetText(text);
        }

        private void SelectMultiplier(int multiplier)
        {
            _logger.Log($"Multiplier selected: {multiplier}x");
            SelectedMult = multiplier;
            Close();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            _multiplier1xButton.onClick.RemoveAllListeners();
            _multiplier2xButton.onClick.RemoveAllListeners();
            _multiplier3xButton.onClick.RemoveAllListeners();
        }
    }
}