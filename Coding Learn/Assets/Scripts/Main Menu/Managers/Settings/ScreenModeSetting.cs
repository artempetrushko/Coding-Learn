using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scripts
{
    public class ScreenModeSetting : GameSetting
    {
        public ScreenModeSetting(SettingsOptionView view) : base(view) { }

        public override void ApplyValue()
        {
            Screen.fullScreenMode = (FullScreenMode)Enum.Parse(typeof(FullScreenMode), CurrentFormattedValue);
            SaveManager.SettingsData.FullScreenMode = Screen.fullScreenMode;
        }

        protected override string GetCurrentValue() => SaveManager.SettingsData.FullScreenMode.ToString();

        protected override List<string> GetFormattedSettingValues()
        {
            return Enum.GetNames(typeof(FullScreenMode)).ToList();
        }
    }
}
