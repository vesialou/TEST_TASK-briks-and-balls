using BricksAndBalls.Core.Interfaces;
using BricksAndBalls.Services.Scene;
using BricksAndBalls.Services.Settings;
using BricksAndBalls.UI.Popups;
using BricksAndBalls.UI.Presenters;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace BricksAndBalls.UI
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _exitButton;
        [SerializeField] private Button _settingsButton;

        private SceneLoader _sceneLoader;
        private IAppLogger _logger;
        private IPopupService _popupService;
        private ISettingsService _settingsService;

        [Inject]
        public void Construct(
            ISettingsService  settingsService,
            SceneLoader sceneLoader,
            IAppLogger logger,
            IPopupService popupService)
        {
            _sceneLoader  = sceneLoader;
            _logger =  logger;
            _popupService =  popupService;
            _settingsService = settingsService;
        }

        private void Start()
        {
            _playButton.onClick.AddListener(OnPlayClicked);
            _exitButton.onClick.AddListener(OnExitClicked);
            _settingsButton.onClick.AddListener(OnSettingsClicked);
            _logger.Log("MainMenuController initialized");
        }

        private void OnSettingsClicked()
        {
            var data = new SettingsPopupData()
            {
                UserName = _settingsService.GetPlayerName()
            };
            var result = _popupService.ShowAsync<SettingsPopupPresenter, SettingsPopupData, SettingsPopupResult>(data);
        }

        private void OnPlayClicked()
        {
            _logger.Log("MainMenuController: Loading Game scene");
            _sceneLoader.LoadGame();
        }

        private void OnExitClicked()
        {
            Application.Quit();
        }
    }
}