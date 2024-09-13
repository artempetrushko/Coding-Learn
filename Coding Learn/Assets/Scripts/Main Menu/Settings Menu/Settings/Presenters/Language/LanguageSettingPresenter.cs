using System.Linq;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace MainMenu
{
    public class LanguageSettingPresenter : SwitchesSettingPresenter
    {
        private Locale[] _settingValues;
        private int _currentValueOrderNumber;

        public LanguageSettingPresenter(string saveKey, SwitchesSettingView view) : base(saveKey, view)
        {
        }

        public override void ApplyValue()
        {
            var selectedLocale = _settingValues[_currentValueOrderNumber - 1];
            LocalizationSettings.SelectedLocale = selectedLocale;

            ES3.Save(_saveKey, selectedLocale.LocaleName.Split()[0]);
        }

        protected override void SetNeighbouringValue(int orderNumberOffset)
        {
            throw new System.NotImplementedException();
        }

        private string[] GetLanguageNames()
        {
            return LocalizationSettings.AvailableLocales.Locales
                    .Select(locale => locale.LocaleName.Split()[0])
                    .ToArray();
        }
    }
}
