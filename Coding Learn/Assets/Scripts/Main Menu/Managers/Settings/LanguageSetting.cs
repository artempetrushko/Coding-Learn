using System.Collections.Generic;
using System.Linq;
using UnityEngine.Localization.Settings;

namespace Scripts
{
    public class LanguageSetting : GameSetting
    {
        public LanguageSetting(SettingsOptionView view) : base(view) { }

        public override void ApplyValue()
        {
            var selectedLocale = LocalizationSettings.AvailableLocales.Locales
                        .Where(locale => locale.LocaleName.Split()[0] == CurrentFormattedValue)
                        .FirstOrDefault();
            if (selectedLocale != null)
            {
                LocalizationSettings.SelectedLocale = selectedLocale;
                SaveManager.SettingsData.Language = selectedLocale.LocaleName.Split()[0];
            }
        }

        protected override string GetCurrentValue()
            => GetLanguageNames()
                .Where(language => language == SaveManager.SettingsData.Language)
                .First();

        protected override List<string> GetFormattedSettingValues() => GetLanguageNames();

        private List<string> GetLanguageNames()
        {
            return LocalizationSettings.AvailableLocales.Locales
                    .Select(locale => locale.LocaleName.Split()[0])
                    .ToList();
        }
    }
}
