using System;
using BricksAndBalls.Core.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace BricksAndBalls.UI.Popups
{
    public class SettingsPopup : BasePopup
    {
        [SerializeField] private Button _closeAreaButton;
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _saveButton;
        [SerializeField] private Button _renameButton;
        [SerializeField] private TextMeshProUGUI _userNameText;
        [SerializeField] private TMP_InputField _inputUserNameText;
        
        [Inject] private IAppLogger _logger;

        public event Action OnSaveClicked;
        public event Action<string> OnUserNameChanged;
        private string _currentUserName;
        
        
        private void Start()
        {
            _closeAreaButton.onClick.AddListener(HandleSaveClick);
            _closeButton.onClick.AddListener(HandleCloseClick);
            _saveButton.onClick.AddListener(HandleCloseClick);
            _renameButton.onClick.AddListener(HandleRenameClick);
            _inputUserNameText.onEndEdit.AddListener(HandleUserNameEndEdit);
            
            _inputUserNameText.gameObject.SetActive(false);
        }

        private void HandleRenameClick()
        {
            _renameButton.gameObject.SetActive(false);
            _userNameText.gameObject.SetActive(false);
            _saveButton.gameObject.SetActive(false);
            
            _inputUserNameText.gameObject.SetActive(true);
            _inputUserNameText.text = _currentUserName;
            _inputUserNameText.Select();
            _inputUserNameText.ActivateInputField();
        }

        private void HandleUserNameEndEdit(string newName)
        {
            newName = newName.Trim();
            if (!string.IsNullOrEmpty(newName) && newName != _currentUserName)
            {
                _currentUserName = newName;
                _userNameText.text = newName;
                OnUserNameChanged?.Invoke(newName);
                _logger.Log($"SettingsPopup: username changed to '{newName}'");
            }

            _inputUserNameText.gameObject.SetActive(false);
            
            _renameButton.gameObject.SetActive(true);
            _userNameText.gameObject.SetActive(true);
            _saveButton.gameObject.SetActive(true);
        }

        public void Setup(string userName)
        {
            _currentUserName = userName;
            _userNameText.text = userName;
            _inputUserNameText.text = userName;
        }
        
        private void HandleSaveClick()
        {
            _logger.Log("SettingsPopup: Play Next clicked");
            OnSaveClicked?.Invoke();
            Close();
        }
        private void HandleCloseClick()
        {
            _logger.Log("SettingsPopup: Close clicked");
            Close();
        }
        
        protected void OnDestroy()
        {
            _closeAreaButton.onClick.RemoveAllListeners();
            _closeButton.onClick.RemoveAllListeners();
            _saveButton.onClick.RemoveAllListeners();
            _renameButton.onClick.RemoveAllListeners();
            _inputUserNameText.onEndEdit.RemoveAllListeners();
        }
    }
}