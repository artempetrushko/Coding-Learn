using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    public class SettingsData : SavedData
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
