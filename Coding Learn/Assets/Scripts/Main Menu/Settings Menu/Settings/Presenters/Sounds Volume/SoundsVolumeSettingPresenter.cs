using Sounds;

namespace MainMenu
{
    public class SoundsVolumeSettingPresenter : SliderSettingPresenter
    {
        public SoundsVolumeSettingPresenter(string saveKey, SliderSettingView view, int minSettingValue, int maxSettingValue) : base(saveKey, view, minSettingValue, maxSettingValue) { }

        public override void ApplyValue()
        {
            SoundsManager.SetSoundsVolume(_currentSettingValue);

            ES3.Save(_saveKey, _currentSettingValue);
        }
    }
}
