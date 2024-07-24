using System;
using System.Collections.Generic;
using System.Linq;

namespace Scripts
{
    public class MusicVolumeSetting : GameSetting
    {
        public MusicVolumeSetting(SettingsOptionView view) : base(view) { }

        public override void ApplyValue()
        {
            var musicVolume = int.Parse(CurrentFormattedValue);
            audioManager.SetMusicVolume(musicVolume);
            SaveManager.SettingsData.MusicVolume = musicVolume;
        }

        protected override string GetCurrentValue() => SaveManager.SettingsData.SoundsVolume.ToString();

        protected override List<string> GetFormattedSettingValues()
        {
            throw new NotImplementedException();
        }

        private List<string> GetFormattedNumbersRange(int minValue, int maxValue)
        {
            return Enumerable.Range(minValue, maxValue - minValue + 1)
                .Select(value => value.ToString())
                .ToList();
        }
    }
}
