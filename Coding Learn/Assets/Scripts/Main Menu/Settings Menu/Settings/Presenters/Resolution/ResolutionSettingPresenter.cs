using System;
using UnityEngine;
using UnityEngine.Localization;

namespace MainMenu
{
    public class ResolutionSettingPresenter : SwitchesSettingPresenter
    {
        protected override int SettingValuesCount => Screen.resolutions.Length;

        public ResolutionSettingPresenter(LocalizedString settingName, string saveKey, SwitchesSettingView view) : base(settingName, saveKey, view)
        {
            var startValue = ES3.Load(_saveKey, Screen.currentResolution);
            var startValueOrderNumber = Array.IndexOf(Screen.resolutions, startValue) + 1;
            SetValue(startValueOrderNumber);
        }

        public override void ApplyValue()
        {
            var selectedResolution = Screen.resolutions[_currentValueOrderNumber - 1];
            Screen.SetResolution(selectedResolution.width, selectedResolution.height, Screen.fullScreen);

            ES3.Save(_saveKey, selectedResolution);
        }

        protected override void SetValue(int valueOrderNumber)
        {
            _currentValueOrderNumber = valueOrderNumber;

            var selectedResolution = Screen.resolutions[_currentValueOrderNumber - 1];
            _settingView.SetOptionValueText($"{selectedResolution.width} x {selectedResolution.height}");
        }
    }
}
