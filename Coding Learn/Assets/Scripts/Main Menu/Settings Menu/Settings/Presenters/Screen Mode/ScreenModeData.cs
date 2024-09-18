using System;
using UnityEngine;
using UnityEngine.Localization;

namespace MainMenu
{
    [Serializable]
    public class ScreenModeData
    {
        [SerializeField] private FullScreenMode _screenMode;
        [SerializeField] private LocalizedString _localizedScreenModeName;

        public FullScreenMode ScreenMode => _screenMode;
        public LocalizedString LocalizedName => _localizedScreenModeName;
    }
}
