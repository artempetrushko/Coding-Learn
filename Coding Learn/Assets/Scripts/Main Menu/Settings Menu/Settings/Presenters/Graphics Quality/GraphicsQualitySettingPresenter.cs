using UnityEngine;
using UnityEngine.Localization;

namespace MainMenu
{
    public class GraphicsQualitySettingPresenter : SwitchesSettingPresenter
    {
        private (string qualityLevelName, LocalizedString localizedQualityLevelName)[] _settingValues;
        private int _currentValueOrderNumber;

        public GraphicsQualitySettingPresenter(string saveKey, SwitchesSettingView view, (string qualityLevelName, LocalizedString localizedQualityLevelName)[] settingValues) : base(saveKey, view)
        {
            _settingValues = settingValues;

            var startValue = ES3.Load(_saveKey, _settingValues[^1].qualityLevelName);
            ES3.Load(_saveKey).ToString();
        }

        public override void ApplyValue()
        {
            QualitySettings.SetQualityLevel(_currentValueOrderNumber);

            ES3.Save(_saveKey, _currentValueOrderNumber);
        }

        protected override void SetNeighbouringValue(int orderNumberOffset)
        {
            if (_currentValueOrderNumber + orderNumberOffset > _settingValues.Length)
            {
                SetNewValue(1);
            }
            else if (_currentValueOrderNumber + orderNumberOffset <= 0)
            {
                SetNewValue(_settingValues.Length);
            }
            else
            {
                SetNewValue(_currentValueOrderNumber + orderNumberOffset);
            }
        }

        private void SetNewValue(int valueOrderNumber)
        {
            _currentValueOrderNumber = valueOrderNumber;
            //_optionView.SetOptionValue(formattedSettingValues[_currentValueOrderNumber - 1]);
        }
    }
}
