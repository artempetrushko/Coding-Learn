using System.Collections.Generic;
using System.Linq;

namespace Scripts
{
    public class SoundsVolumeSetting : GameSetting
    {
        public SoundsVolumeSetting(SettingsOptionView view) : base(view) { }

        public override void ApplyValue()
        {
            var soundsVolume = int.Parse(CurrentFormattedValue);
            AudioManager.SetSoundsVolume(soundsVolume);
            SaveManager.SettingsData.SoundsVolume = soundsVolume;
        }

        protected override string GetCurrentValue() => SaveManager.SettingsData.MusicVolume.ToString();

        protected override List<string> GetFormattedSettingValues() => GetFormattedNumbersRange(0, 100);

        private List<string> GetFormattedNumbersRange(int minValue, int maxValue)
        {
            return Enumerable.Range(minValue, maxValue - minValue + 1)
                .Select(value => value.ToString())
                .ToList();
        }
    }
}
