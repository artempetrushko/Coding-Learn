using UnityEngine.Localization;

namespace MainMenu
{
    public abstract class SwitchesSettingPresenter : SettingPresenter
    {
        protected readonly SwitchesSettingView _settingView;

        protected int _currentValueOrderNumber;

        protected abstract int SettingValuesCount { get; }

        public SwitchesSettingPresenter(LocalizedString settingName, string saveKey, SwitchesSettingView view) : base(settingName, saveKey)
        {
            _settingView = view;

            _settingView.SetOptionTitleText(_settingName.GetLocalizedString());
            _settingView.NextValueButton.onClick.AddListener(() => SetNeighbouringValue(1));
            _settingView.PreviousValueButton.onClick.AddListener(() => SetNeighbouringValue(-1));
        }

        protected abstract void SetValue(int valueOrderNumber);

        private void SetNeighbouringValue(int orderNumberOffset)
        {
            if (_currentValueOrderNumber + orderNumberOffset > SettingValuesCount)
            {
                SetValue(1);
            }
            else if (_currentValueOrderNumber + orderNumberOffset <= 0)
            {
                SetValue(SettingValuesCount);
            }
            else
            {
                SetValue(_currentValueOrderNumber + orderNumberOffset);
            }
        }
    }
}
