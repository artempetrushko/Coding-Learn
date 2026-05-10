using UnityEngine;

namespace MainMenu
{
    [CreateAssetMenu(fileName = "Music Volume Setting Creator", menuName = "Game Configs/Settings/Music Volume")]
    public class MusicVolumeSettingCreator : SliderSettingCreator
    {
        private MusicVolumeSettingPresenter _musicVolumeSettingPresenter;

        public override SettingPresenter CreateSliderSetting(SliderSettingView sliderSettingView)
        {
            _musicVolumeSettingPresenter ??= new MusicVolumeSettingPresenter(_name, _saveKey, sliderSettingView, _minSettingValue, _maxSettingValue);
            return _musicVolumeSettingPresenter;
        }
    }
}
