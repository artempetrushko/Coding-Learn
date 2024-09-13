using Sounds;

namespace MainMenu
{
    public class MusicVolumeSettingPresenter : SliderSettingPresenter
    {
        public MusicVolumeSettingPresenter(string saveKey, SliderSettingView view, int minSettingValue, int maxSettingValue) : base(saveKey, view, minSettingValue, maxSettingValue) { }

        public override void ApplyValue()
        {
            SoundsManager.SetMusicVolume(_currentSettingValue);

            ES3.Save(_saveKey, _currentSettingValue);
        }
    }
}
