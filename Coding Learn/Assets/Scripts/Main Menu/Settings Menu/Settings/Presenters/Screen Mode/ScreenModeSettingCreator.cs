using UnityEngine;

namespace MainMenu
{
    [CreateAssetMenu(fileName = "Screen Mode Setting Creator", menuName = "Game Configs/Settings/Screen Mode")]
    public class ScreenModeSettingCreator : SwitchesSettingCreator
    {
        [SerializeField] private ScreenModeData[] _screenModeDatas;

        private ScreenModeSettingPresenter _screenModeSettingPresenter;

        public override SettingPresenter CreateSwitchesSetting(SwitchesSettingView settingView)
        {
            _screenModeSettingPresenter ??= new ScreenModeSettingPresenter(_name, _saveKey, settingView, _screenModeDatas);
            return _screenModeSettingPresenter;
        }
    }
}
