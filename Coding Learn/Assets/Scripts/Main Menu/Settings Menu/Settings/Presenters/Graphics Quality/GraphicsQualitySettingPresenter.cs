using UnityEngine;
using UnityEngine.Localization;

namespace MainMenu
{
    public class GraphicsQualitySettingPresenter : SwitchesSettingPresenter
    {
        private LocalizedString[] _settingValues;

        protected override int SettingValuesCount => _settingValues.Length;

        public GraphicsQualitySettingPresenter(LocalizedString settingName, string saveKey, SwitchesSettingView view, LocalizedString[] settingValues) : base(settingName, saveKey, view)
        {
            _settingValues = settingValues;

            var startValueOrderNumber = ES3.Load(_saveKey, QualitySettings.GetQualityLevel());
            SetValue(startValueOrderNumber);
        }

        public override void ApplyValue()
        {
            QualitySettings.SetQualityLevel(_currentValueOrderNumber - 1);

            ES3.Save(_saveKey, _currentValueOrderNumber);
        }

        protected override void SetValue(int valueOrderNumber)
        {
            _currentValueOrderNumber = valueOrderNumber;
            _settingView.SetOptionValueText(_settingValues[_currentValueOrderNumber - 1].GetLocalizedString());
        }
    }
}
