using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scripts
{
    public class GraphicsQualitySetting : GameSetting
    {
        public GraphicsQualitySetting(SettingsOptionView view) : base(view) { }

        public override void ApplyValue()
        {
            QualitySettings.SetQualityLevel(QualitySettings.names.ToList().IndexOf(CurrentFormattedValue));
            SaveManager.SettingsData.GraphicsQuality = QualitySettings.names[QualitySettings.GetQualityLevel()];
        }

        protected override string GetCurrentValue() => SaveManager.SettingsData.GraphicsQuality;

        protected override List<string> GetFormattedSettingValues() => QualitySettings.names.ToList();
    }
}
