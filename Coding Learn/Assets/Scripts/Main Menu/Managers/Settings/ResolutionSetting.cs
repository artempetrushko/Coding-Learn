using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scripts
{
    public class ResolutionSetting : GameSetting
    {
        public ResolutionSetting(SettingsOptionView view) : base(view) { }

        public override void ApplyValue()
        {
            var resolutionParams = CurrentFormattedValue.Split('x').Select(text => int.Parse(text)).ToList();
            Screen.SetResolution(resolutionParams[0], resolutionParams[1], Screen.fullScreen);
            SaveManager.SettingsData.Resolution = CurrentFormattedValue;
        }

        protected override string GetCurrentValue() => SaveManager.SettingsData.Resolution;

        protected override List<string> GetFormattedSettingValues()
        {
            return Screen.resolutions
                .Where(resolution => Mathf.Abs(((float)resolution.width / resolution.height) - (16f / 9)) < 1e-5)
                .Select(resolution => string.Format(@"{0} x {1}", resolution.width, resolution.height))
                .ToList();
        }
    }
}
