using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Scripts
{
    public class SettingsManager : IMainMenuSectionManager
    {
        public event Action SettingsApplied;

        private SettingsSectionController _settingsSectionController;
        private SettingsConfig _settingsConfig;
        private List<GameSetting> _gameSettings = new();

        public SettingsManager(SettingsSectionController settingsSectionController)
        {
            _settingsSectionController = settingsSectionController;
        }

        public async UniTask ShowSectionAsync()
        {
            SetSettingsCurrentValues();
            await _settingsSectionController.ChangeVisibilityAsync(true);
        }

        public async UniTask HideSectionAsync()
        {
            await _settingsSectionController.ChangeVisibilityAsync(false);
        }

        public void InitializeSettings()
        {
            _gameSettings.Add(new ResolutionSetting(CreateOptionView(GameSettingType.Resolution)));
            _gameSettings.Add(new ScreenModeSetting(CreateOptionView(GameSettingType.ScreenMode)));
            _gameSettings.Add(new GraphicsQualitySetting(CreateOptionView(GameSettingType.GraphicsQuality)));
            _gameSettings.Add(new LanguageSetting(CreateOptionView(GameSettingType.Language)));
            _gameSettings.Add(new SoundsVolumeSetting(CreateOptionView(GameSettingType.SoundsVolume)));
            _gameSettings.Add(new MusicVolumeSetting(CreateOptionView(GameSettingType.MusicVolume)));

            ApplySettings();
        }

        public void SetSettingsCurrentValues()
        {
            _gameSettings.ForEach(setting => setting.SetCurrentValue());
        }

        public void ApplySettings()
        {
            _gameSettings.ForEach(setting => setting.ApplyValue());
            SettingsApplied?.Invoke();
        }

        private SettingsOptionView CreateOptionView(GameSettingType settingType)
        {
            return _settingsSectionController.CreateOptionView(_settingsConfig.SettingDatas.First(data => data.Type == settingType));
        }
    }
}
