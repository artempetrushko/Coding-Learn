using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace Scripts
{
    public class SettingsManager : MainMenuSectionManager
    {
        [SerializeField]
        private SettingsSectionView settingsSectionView;

        private List<GameSetting> gameSettings = new List<GameSetting>();

        public override IEnumerator ShowSectionView_COR()
        {
            yield return StartCoroutine(settingsSectionView.ChangeVisibility_COR(true));
        }

        public override IEnumerator HideSectionView_COR()
        {
            yield return StartCoroutine(settingsSectionView.ChangeVisibility_COR(false));
        }

        public void CreateSettingsViews()
        {
            gameSettings.Add(new GameSetting(settingsSectionView.CreateOptionView("Resolution Setting Label", SettingViewType.Switches), 
                GetFormattedResolutions(),
                (string formattedValue) =>
                {
                    var resolutionParams = formattedValue.Split('x').Select(text => int.Parse(text)).ToList();
                    Screen.SetResolution(resolutionParams[0], resolutionParams[1], Screen.fullScreen);
                }));

            gameSettings.Add(new GameSetting(settingsSectionView.CreateOptionView("Screen Mode Setting Label", SettingViewType.Switches), 
                Enum.GetNames(typeof(FullScreenMode)).ToList(),
                (string formattedValue) =>
                {
                    Screen.fullScreenMode = (FullScreenMode)Enum.Parse(typeof(FullScreenMode), formattedValue);
                }));

            gameSettings.Add(new GameSetting(settingsSectionView.CreateOptionView("Graphics Quality Setting Label", SettingViewType.Switches), 
                QualitySettings.names.ToList(),
                (string formattedValue) =>
                {
                    QualitySettings.SetQualityLevel(QualitySettings.names.ToList().IndexOf(formattedValue));
                }));

            gameSettings.Add(new GameSetting(settingsSectionView.CreateOptionView("Language Setting Label", SettingViewType.Switches), 
                LocalizationSettings.AvailableLocales.Locales
                    .Select(locale => locale.Identifier.CultureInfo.ToString())
                    .ToList(),
                (string formattedValue) =>
                {
                    
                }));

            gameSettings.Add(new GameSetting(settingsSectionView.CreateOptionView("Sounds Volume Setting Label", SettingViewType.Slider), 
                Enumerable.Range(1, 10).Select(value => value.ToString()).ToList(),
                (string formattedValue) =>
                {
                    
                }));

            gameSettings.Add(new GameSetting(settingsSectionView.CreateOptionView("Music Volume Setting Label", SettingViewType.Slider), 
                Enumerable.Range(1, 10).Select(value => value.ToString()).ToList(),
                (string formattedValue) =>
                {
                    
                }));
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
