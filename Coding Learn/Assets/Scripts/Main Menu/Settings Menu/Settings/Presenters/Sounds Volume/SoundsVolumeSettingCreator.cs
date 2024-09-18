using UnityEngine;

namespace MainMenu
{
    [CreateAssetMenu(fileName = "Sounds Volume Setting Creator", menuName = "Game Configs/Settings/Sounds Volume")]
    public class SoundsVolumeSettingCreator : SliderSettingCreator
    {
        private SoundsVolumeSettingPresenter _soundsVolumeSettingPresenter;

        public override SettingPresenter CreateSliderSetting(SliderSettingView sliderSettingView)
        {
            _soundsVolumeSettingPresenter ??= new SoundsVolumeSettingPresenter(_name, _saveKey, sliderSettingView, _minSettingValue, _maxSettingValue);
            return _soundsVolumeSettingPresenter;
        }
    }
}
