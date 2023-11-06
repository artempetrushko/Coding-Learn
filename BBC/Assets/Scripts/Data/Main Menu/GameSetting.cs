using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    public enum SettingType
    {
        Resolution,
        ScreenMode,
        GraphicsQuality,
        Language,
        SoundsVolume,
        MusicVolume
    }

    public class GameSetting
    {
        public SettingType Type { get; private set; }
        public string Name { get; set; }
        public SettingsOptionView SettingView { get; private set; }

        public GameSetting(SettingType type, string name, SettingsOptionView settingView)
        {
            Type = type;
            Name = name;
            SettingView = settingView;
        }
    }
}
