using UnityEngine;

namespace MainMenu
{
    public abstract class SwitchesSettingCreator : SettingCreator
    {
        [SerializeField] protected SwitchesSettingView _settingViewPrefab;

        public override SettingView SettingViewPrefab => _settingViewPrefab;
    }
}
