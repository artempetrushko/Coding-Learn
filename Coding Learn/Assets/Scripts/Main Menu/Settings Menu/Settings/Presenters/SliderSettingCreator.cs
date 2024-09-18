using UnityEngine;

namespace MainMenu
{
    public abstract class SliderSettingCreator : SettingCreator
    {
        [SerializeField] protected SliderSettingView _settingViewPrefab;
        [Space]
        [SerializeField] protected int _minSettingValue;
        [SerializeField] protected int _maxSettingValue;

        public override SettingView SettingViewPrefab => _settingViewPrefab;

        public abstract SettingPresenter CreateSliderSetting(SliderSettingView sliderSettingView);

        public sealed override SettingPresenter CreateSetting(SettingView settingView)
        {
            return settingView is SliderSettingView sliderSettingView
                ? CreateSliderSetting(sliderSettingView) 
                : null;
        }
    }
}
