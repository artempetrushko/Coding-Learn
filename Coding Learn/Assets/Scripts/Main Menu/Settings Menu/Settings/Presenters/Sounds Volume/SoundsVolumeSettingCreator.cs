using UnityEngine;

namespace MainMenu
{
    [CreateAssetMenu(fileName = "Sounds Volume Setting Creator", menuName = "Game Configs/Settings/Sounds Volume")]
    public class SoundsVolumeSettingCreator : SliderSettingCreator
    {
        public override SettingPresenter CreateSetting(SettingView settingView)
        {
            return settingView is SliderSettingView sliderSettingView
                ? new SoundsVolumeSettingPresenter(_saveKey, sliderSettingView, _minSettingValue, _maxSettingValue) 
                : null;
        }
    }
}
