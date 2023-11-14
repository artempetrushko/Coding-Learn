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
            gameSettings.Add(new GameSetting(SettingType.Resolution, "����������", settingsSectionView.CreateSwitchesOption()));
            gameSettings.Add(new GameSetting(SettingType.ScreenMode, "�����", settingsSectionView.CreateSwitchesOption()));
            gameSettings.Add(new GameSetting(SettingType.GraphicsQuality, "�������� �������", settingsSectionView.CreateSwitchesOption()));
            gameSettings.Add(new GameSetting(SettingType.Language, "����", settingsSectionView.CreateSwitchesOption()));
            gameSettings.Add(new GameSetting(SettingType.SoundsVolume, "��������� ������", settingsSectionView.CreateSliderOption()));
            gameSettings.Add(new GameSetting(SettingType.MusicVolume, "��������� ������", settingsSectionView.CreateSliderOption()));
        }

        //public List<Resolution> 

        private void Start()
        {
            CreateSettingsViews();
        }
    }
}
