using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization.Settings;

namespace Scripts
{
    public class SettingsManager : MainMenuSectionManager
    {
        [SerializeField]
        private SettingsSectionView settingsSectionView;
        [Space, SerializeField]
        private UnityEvent onSettingsApplied;

        private List<GameSetting> gameSettings = new List<GameSetting>();

        public override IEnumerator ShowSectionView_COR()
        {
            SetSettingsCurrentValues();
            yield return StartCoroutine(settingsSectionView.ChangeVisibility_COR(true));
        }

        public override IEnumerator HideSectionView_COR()
        {
            yield return StartCoroutine(settingsSectionView.ChangeVisibility_COR(false));
        }

        public void CreateSettingsViews()
        {
            gameSettings.Add(new GameSetting(settingsSectionView.CreateOptionView(("Main Menu UI", "Resolution Setting Label"), SettingViewType.Switches), 
                GetFormattedResolutions(),
                () => MainMenuSaveManager.SettingsData.Resolution,
                (string formattedValue) =>
                {
                    var resolutionParams = formattedValue.Split('x').Select(text => int.Parse(text)).ToList();
                    Screen.SetResolution(resolutionParams[0], resolutionParams[1], Screen.fullScreen);
                    MainMenuSaveManager.SettingsData.Resolution = formattedValue;
                }));
            gameSettings.Add(new GameSetting(settingsSectionView.CreateOptionView(("Main Menu UI", "Screen Mode Setting Label"), SettingViewType.Switches), 
                Enum.GetNames(typeof(FullScreenMode)).ToList(),
                () => MainMenuSaveManager.SettingsData.FullScreenMode.ToString(),
                (string formattedValue) =>
                {
                    Screen.fullScreenMode = (FullScreenMode)Enum.Parse(typeof(FullScreenMode), formattedValue);
                    MainMenuSaveManager.SettingsData.FullScreenMode = Screen.fullScreenMode;
                }));
            gameSettings.Add(new GameSetting(settingsSectionView.CreateOptionView(("Main Menu UI", "Graphics Quality Setting Label"), SettingViewType.Switches), 
                QualitySettings.names.ToList(),
                () => MainMenuSaveManager.SettingsData.GraphicsQuality,
                (string formattedValue) =>
                {
                    QualitySettings.SetQualityLevel(QualitySettings.names.ToList().IndexOf(formattedValue));
                    MainMenuSaveManager.SettingsData.GraphicsQuality = QualitySettings.names[QualitySettings.GetQualityLevel()];
                }));
            gameSettings.Add(new GameSetting(settingsSectionView.CreateOptionView(("Main Menu UI", "Language Setting Label"), SettingViewType.Switches), 
                LocalizationSettings.AvailableLocales.Locales
                    .Select(locale => locale.Identifier.CultureInfo.ToString())
                    .ToList(),
                () => MainMenuSaveManager.SettingsData.LanguageCode,
                (string formattedValue) =>
                {
                   
                }));
            gameSettings.Add(new GameSetting(settingsSectionView.CreateOptionView(("Main Menu UI", "Sounds Volume Setting Label"), SettingViewType.Slider), 
                Enumerable.Range(1, 100).Select(value => value.ToString()).ToList(),
                () => MainMenuSaveManager.SettingsData.SoundsVolume.ToString(),
                (string formattedValue) =>
                {
                    
                }));
            gameSettings.Add(new GameSetting(settingsSectionView.CreateOptionView(("Main Menu UI", "Music Volume Setting Label"), SettingViewType.Slider), 
                Enumerable.Range(1, 100).Select(value => value.ToString()).ToList(),
                () => MainMenuSaveManager.SettingsData.MusicVolume.ToString(),
                (string formattedValue) =>
                {
                    
                }));
            SetSettingsCurrentValues();
        }

        public void SetSettingsCurrentValues()
        {
            gameSettings.ForEach(setting => setting.SetCurrentValue());
        }

        public void ApplySettings()
        {
            gameSettings.ForEach(setting => setting.ApplyValue());
            onSettingsApplied.Invoke();
        }

        private List<string> GetFormattedResolutions()
        {
            return Screen.resolutions
                .Where(resolution => Mathf.Abs(((float)resolution.width / resolution.height) - (16f / 9)) < 1e-5)
                .Select(resolution => string.Format(@"{0} x {1}", resolution.width, resolution.height))
                .ToList();
        }
    }
}
