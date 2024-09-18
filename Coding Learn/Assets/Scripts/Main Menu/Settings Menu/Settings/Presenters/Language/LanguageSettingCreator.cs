using UnityEngine;

namespace MainMenu
{
    [CreateAssetMenu(fileName = "Language Setting Creator", menuName = "Game Configs/Settings/Language")]
    public class LanguageSettingCreator : SwitchesSettingCreator
    {
        private LanguageSettingPresenter _languageSettingPresenter;

        public override SettingPresenter CreateSwitchesSetting(SwitchesSettingView settingView)
        {
            _languageSettingPresenter ??= new LanguageSettingPresenter(_name, _saveKey, settingView);
            return _languageSettingPresenter;
        }
    }
}
