using UnityEngine;

namespace MainMenu
{
    public abstract class SwitchesSettingCreator : SettingCreator
    {
        [SerializeField] protected SwitchesSettingView _settingViewPrefab;

        public override SettingView SettingViewPrefab => _settingViewPrefab;

        public abstract SettingPresenter CreateSwitchesSetting(SwitchesSettingView settingView);

        public sealed override SettingPresenter CreateSetting(SettingView settingView)
        {
            return settingView is SwitchesSettingView switchesSettingView
                ? CreateSwitchesSetting(switchesSettingView) 
                : null;
        }
    }
}
