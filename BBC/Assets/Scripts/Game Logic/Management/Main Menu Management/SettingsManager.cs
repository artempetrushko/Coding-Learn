using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    public class SettingsManager : MonoBehaviour
    {
        [SerializeField]
        private SettingsSectionView settingsSectionView;

        private List<GameSetting> gameSettings = new List<GameSetting>();

        public void CreateSettingsViews()
        {
            gameSettings.Add(new GameSetting(SettingType.Resolution, "Разрешение", settingsSectionView.CreateSwitchesOption()));
            gameSettings.Add(new GameSetting(SettingType.ScreenMode, "Экран", settingsSectionView.CreateSwitchesOption()));
            gameSettings.Add(new GameSetting(SettingType.GraphicsQuality, "Качество графики", settingsSectionView.CreateSwitchesOption()));
            gameSettings.Add(new GameSetting(SettingType.Language, "Язык", settingsSectionView.CreateSwitchesOption()));
            gameSettings.Add(new GameSetting(SettingType.SoundsVolume, "Громкость звуков", settingsSectionView.CreateSliderOption()));
            gameSettings.Add(new GameSetting(SettingType.MusicVolume, "Громкость музыки", settingsSectionView.CreateSliderOption()));
        }

        //public List<Resolution> 

        private void Start()
        {
            CreateSettingsViews();
        }
    }
}
