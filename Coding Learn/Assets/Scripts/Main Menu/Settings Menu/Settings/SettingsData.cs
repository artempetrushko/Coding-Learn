using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine;

namespace MainMenu
{
    public class SettingsData
    {
        public string Resolution;
        [JsonConverter(typeof(StringEnumConverter))]
        public FullScreenMode FullScreenMode;
        public string GraphicsQuality;
        public string Language;
        public int SoundsVolume;
        public int MusicVolume;
    }
}
