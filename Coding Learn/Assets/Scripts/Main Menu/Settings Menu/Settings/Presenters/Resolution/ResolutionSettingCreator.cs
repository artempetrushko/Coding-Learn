using UnityEngine;

namespace MainMenu
{
    [CreateAssetMenu(fileName = "Resolution Setting Creator", menuName = "Game Configs/Settings/Resolution")]
    public class ResolutionSettingCreator : SwitchesSettingCreator
    {
        private ResolutionSettingPresenter _resolutionSettingPresenter;

        public override SettingPresenter CreateSwitchesSetting(SwitchesSettingView settingView)
        {
            _resolutionSettingPresenter ??= new ResolutionSettingPresenter(_name, _saveKey, settingView);
            return _resolutionSettingPresenter;
        }
    }
}
