using Sounds;
using UnityEngine.Localization;

namespace MainMenu
{
    public class SoundsVolumeSettingPresenter : SliderSettingPresenter
    {
        public SoundsVolumeSettingPresenter(LocalizedString settingName, string saveKey, SliderSettingView view, int minSettingValue, int maxSettingValue) : base(settingName, saveKey, view, minSettingValue, maxSettingValue) { }

        public override void ApplyValue()
        {
            SoundsManager.SetSoundsVolume(_currentSettingValue);

            ES3.Save(_saveKey, _currentSettingValue);
        }
    }
}
