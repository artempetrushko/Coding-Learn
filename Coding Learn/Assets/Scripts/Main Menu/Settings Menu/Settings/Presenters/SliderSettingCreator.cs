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
    }
}
