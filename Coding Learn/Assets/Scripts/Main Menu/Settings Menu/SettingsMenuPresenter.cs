using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Object = UnityEngine.Object;

namespace MainMenu
{
    public class SettingsMenuPresenter : IMainMenuSectionPresenter
    {
        public event Action SectionDisabled;
        public event Action SettingsApplied;

        private const float VISIBILITY_CHANGING_TIME = 0.75f;

        private SettingsMenuView _settingsMenuView;
        private GameSettingsConfig _gameSettingsConfig;
        private SettingPresenter[] _gameSettings;

        public SettingsMenuPresenter(SettingsMenuView settingsMenuView, GameSettingsConfig settingsConfig)
        {
            _settingsMenuView = settingsMenuView;
            _gameSettingsConfig = settingsConfig;

            _settingsMenuView.ApplySettingsButton.onClick.AddListener(OnApplySettingsButtonPressed);
            _settingsMenuView.CloseViewButton.onClick.AddListener(OnCloseViewButtonPressed);
        }

        public async UniTask ShowSectionAsync()
        {
            _settingsMenuView.SetActive(true);
            await SetSectionVisibilityAsync(true);
        }

        public async UniTask HideSectionAsync()
        {
            await SetSectionVisibilityAsync(false);
            _settingsMenuView.SetActive(false);

            SectionDisabled?.Invoke();
        }

		public void Initialize()
        {
            _gameSettings = new SettingPresenter[_gameSettingsConfig.SettingCreators.Length];
            for (var i = 0; i < _gameSettings.Length; i++)
            {
                var optionView = Object.Instantiate(_gameSettingsConfig.SettingCreators[i].SettingViewPrefab, _settingsMenuView.SettingViewsContainer.transform);
                _gameSettings[i] = _gameSettingsConfig.SettingCreators[i].CreateSetting(optionView);
            }

            ApplySettings();
        }

        private async UniTask SetSectionVisibilityAsync(bool isVisible)
        {
            await _settingsMenuView.transform
                .DOLocalMoveY(isVisible ? 0 : _settingsMenuView.GetSectionHeight(), VISIBILITY_CHANGING_TIME)
                .AsyncWaitForCompletion();
        }

        private void ApplySettings()
        {
            foreach (var setting in _gameSettings)
            {
                setting.ApplyValue();
            }
            SettingsApplied?.Invoke();
        }

        private void OnApplySettingsButtonPressed() => ApplySettings();

        private void OnCloseViewButtonPressed() => HideSectionAsync().Forget();
    }
}
