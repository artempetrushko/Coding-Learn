using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization.Settings;

namespace Scripts
{
    public class SettingsController : IMainMenuSectionController
    {
        private MainMenuSaveController saveManager;

        [SerializeField]
        private SettingsSectionView settingsSectionView;
        [Space, SerializeField]
        private AudioManager audioManager;
        [Space, SerializeField]
        private UnityEvent onSettingsApplied;

        private List<GameSetting> gameSettings = new();

        public SettingsController(SettingsSectionView settingsSectionView, AudioManager audioManager)
        {
            this.settingsSectionView = settingsSectionView;
            this.audioManager = audioManager;
        }

        public async UniTask ShowSectionViewAsync()
        {
            SetSettingsCurrentValues();
            await settingsSectionView.ChangeVisibilityAsync(true);
        }

        public async UniTask HideSectionViewAsync()
        {
            await settingsSectionView.ChangeVisibilityAsync(false);
        }

        public void InitializeSettings()
        {
            gameSettings.Add(new GameSetting(settingsSectionView.CreateOptionView(("Main Menu UI", "Resolution Setting Label"), SettingViewType.Switches), 
                GetFormattedResolutions(),
                () => saveManager.SettingsData.Resolution,
                (string formattedValue) =>
                {
                    var resolutionParams = formattedValue.Split('x').Select(text => int.Parse(text)).ToList();
                    Screen.SetResolution(resolutionParams[0], resolutionParams[1], Screen.fullScreen);
                    saveManager.SettingsData.Resolution = formattedValue;
                }));

            gameSettings.Add(new GameSetting(settingsSectionView.CreateOptionView(("Main Menu UI", "Screen Mode Setting Label"), SettingViewType.Switches), 
                Enum.GetNames(typeof(FullScreenMode)).ToList(),
                () => saveManager.SettingsData.FullScreenMode.ToString(),
                (string formattedValue) =>
                {
                    Screen.fullScreenMode = (FullScreenMode)Enum.Parse(typeof(FullScreenMode), formattedValue);
                    saveManager.SettingsData.FullScreenMode = Screen.fullScreenMode;
                }));

            gameSettings.Add(new GameSetting(settingsSectionView.CreateOptionView(("Main Menu UI", "Graphics Quality Setting Label"), SettingViewType.Switches), 
                QualitySettings.names.ToList(),
                () => saveManager.SettingsData.GraphicsQuality,
                (string formattedValue) =>
                {
                    QualitySettings.SetQualityLevel(QualitySettings.names.ToList().IndexOf(formattedValue));
                    saveManager.SettingsData.GraphicsQuality = QualitySettings.names[QualitySettings.GetQualityLevel()];
                }));

            gameSettings.Add(new GameSetting(settingsSectionView.CreateOptionView(("Main Menu UI", "Language Setting Label"), SettingViewType.Switches), 
                GetLanguageNames(),
                () => GetLanguageNames()
                        .Where(language => language == saveManager.SettingsData.Language)
                        .First(),
                (string formattedValue) =>
                {
                    var selectedLocale = LocalizationSettings.AvailableLocales.Locales
                        .Where(locale => locale.LocaleName.Split()[0] == formattedValue)
                        .FirstOrDefault();
                    if (selectedLocale != null)
                    {
                        LocalizationSettings.SelectedLocale = selectedLocale;
                        saveManager.SettingsData.Language = selectedLocale.LocaleName.Split()[0];
                    }
                }));

            gameSettings.Add(new GameSetting(settingsSectionView.CreateOptionView(("Main Menu UI", "Sounds Volume Setting Label"), SettingViewType.Slider), 
                GetFormattedNumbersRange(0, 100),
                () => saveManager.SettingsData.SoundsVolume.ToString(),
                (string formattedValue) =>
                {
                    var soundsVolume = int.Parse(formattedValue);
                    audioManager.SetSoundsVolume(soundsVolume);
                    saveManager.SettingsData.SoundsVolume = soundsVolume;
                }));

            gameSettings.Add(new GameSetting(settingsSectionView.CreateOptionView(("Main Menu UI", "Music Volume Setting Label"), SettingViewType.Slider),
                GetFormattedNumbersRange(0, 100),
                () => saveManager.SettingsData.MusicVolume.ToString(),
                (string formattedValue) =>
                {
                    var musicVolume = int.Parse(formattedValue);
                    audioManager.SetMusicVolume(musicVolume);
                    saveManager.SettingsData.MusicVolume = musicVolume;
                }));

            ApplySettings();
        }

        public void SetSettingsCurrentValues()
        {
            gameSettings.ForEach(setting => setting.SetCurrentValue());
        }

        public void ApplySettings()
        {
            gameSettings.ForEach(setting => setting.ApplyValue());
            onSettingsApplied?.Invoke();
        }

        private List<string> GetFormattedResolutions()
        {
            return Screen.resolutions
                .Where(resolution => Mathf.Abs(((float)resolution.width / resolution.height) - (16f / 9)) < 1e-5)
                .Select(resolution => string.Format(@"{0} x {1}", resolution.width, resolution.height))
                .ToList();
        }

        private List<string> GetLanguageNames()
        {
            return LocalizationSettings.AvailableLocales.Locales
                    .Select(locale => locale.LocaleName.Split()[0])
                    .ToList();
        }

        private List<string> GetFormattedNumbersRange(int minValue, int maxValue)
        {
            return Enumerable.Range(minValue, maxValue - minValue + 1)
                .Select(value => value.ToString())
                .ToList();
        }
    }
}
