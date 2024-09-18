using System.Linq;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace MainMenu
{
    public class LanguageSettingPresenter : SwitchesSettingPresenter
    {
        protected override int SettingValuesCount => LocalizationSettings.AvailableLocales.Locales.Count;

        public LanguageSettingPresenter(LocalizedString settingName, string saveKey, SwitchesSettingView view) : base(settingName, saveKey, view)
        {
            var startValue = ES3.Load<string>(_saveKey, LocalizationSettings.SelectedLocale.LocaleName);
            var startValueOrderNumber = LocalizationSettings.AvailableLocales.Locales
                .Select(locale => locale.LocaleName)
                .ToList()
                .IndexOf(startValue) + 1;
            SetValue(startValueOrderNumber);
        }

        public override void ApplyValue()
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[_currentValueOrderNumber - 1];

            ES3.Save(_saveKey, LocalizationSettings.SelectedLocale.LocaleName);
        }

        protected override void SetValue(int valueOrderNumber)
        {
            _currentValueOrderNumber = valueOrderNumber;

            var languageName = LocalizationSettings.AvailableLocales.Locales[_currentValueOrderNumber - 1].LocaleName.Split()[0];
            _settingView.SetOptionValueText(languageName);
        }
    }
}
