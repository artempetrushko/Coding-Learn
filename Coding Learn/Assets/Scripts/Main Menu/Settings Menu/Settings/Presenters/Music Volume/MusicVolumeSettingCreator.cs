using UnityEngine;

namespace MainMenu
{
    [CreateAssetMenu(fileName = "Music Volume Setting Creator", menuName = "Game Configs/Settings/Music Volume")]
    public class MusicVolumeSettingCreator : SliderSettingCreator
    {
        public override SettingPresenter CreateSetting(SettingView settingView)
        {
            return settingView is SliderSettingView sliderSettingView
                ? new MusicVolumeSettingPresenter(_saveKey, sliderSettingView, _minSettingValue, _maxSettingValue)
                : null;
        }
    }
}
