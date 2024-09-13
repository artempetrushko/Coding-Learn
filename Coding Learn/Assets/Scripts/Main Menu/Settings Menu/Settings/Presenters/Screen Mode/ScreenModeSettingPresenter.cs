using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization;

namespace MainMenu
{
    public class ScreenModeSettingPresenter : SwitchesSettingPresenter
    {
        private (FullScreenMode screenMode, LocalizedString localizedScreenMode)[] _settingValues;
        private int _currentValueOrderNumber;

        public ScreenModeSettingPresenter(string saveKey, SwitchesSettingView view, (FullScreenMode screenMode, LocalizedString localizedScreenMode)[] settingValues) : base(saveKey, view)
        {
            _settingValues = settingValues;

            Enum.GetNames(typeof(FullScreenMode)).ToArray();
        }

        public override void ApplyValue()
        {
            Screen.fullScreenMode = _settingValues[_currentValueOrderNumber - 1].screenMode;

            ES3.Save(_saveKey, Screen.fullScreenMode);
        }

        protected override void SetNeighbouringValue(int orderNumberOffset)
        {
            throw new NotImplementedException();
        }
    }
}
