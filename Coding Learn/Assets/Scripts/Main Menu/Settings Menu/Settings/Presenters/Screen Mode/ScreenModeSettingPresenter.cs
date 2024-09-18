using System;
using UnityEngine;
using UnityEngine.Localization;

namespace MainMenu
{

    public class ScreenModeSettingPresenter : SwitchesSettingPresenter
    {
        private ScreenModeData[] _settingValues;

        protected override int SettingValuesCount => _settingValues.Length;

        public ScreenModeSettingPresenter(LocalizedString settingName, string saveKey, SwitchesSettingView view, ScreenModeData[] settingValues) : base(settingName, saveKey, view)
        {
            _settingValues = settingValues;

            var startScreenMode = ES3.Load(_saveKey, FullScreenMode.ExclusiveFullScreen);
            var startValueOrderNumber = Array.IndexOf(_settingValues, startScreenMode) + 1;
            SetValue(startValueOrderNumber);
        }

        public override void ApplyValue()
        {
            Screen.fullScreenMode = _settingValues[_currentValueOrderNumber - 1].ScreenMode;

            ES3.Save(_saveKey, Screen.fullScreenMode);
        }

        protected override void SetValue(int valueOrderNumber)
        {
            _currentValueOrderNumber = valueOrderNumber;
            _settingView.SetOptionValueText(_settingValues[_currentValueOrderNumber - 1].LocalizedName.GetLocalizedString());
        }
    }
}
