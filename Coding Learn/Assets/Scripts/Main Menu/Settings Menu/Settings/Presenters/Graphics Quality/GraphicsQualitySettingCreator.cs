using UnityEngine;
using UnityEngine.Localization;

namespace MainMenu
{
    [CreateAssetMenu(fileName = "Graphics Quality Setting Creator", menuName = "Game Configs/Settings/Graphics Quality")]
    public class GraphicsQualitySettingCreator : SwitchesSettingCreator
    {
        [SerializeField] private LocalizedString[] _localizedQualityLevelNames;

        private GraphicsQualitySettingPresenter _graphicsQualitySettingPresenter;

        public override SettingPresenter CreateSwitchesSetting(SwitchesSettingView settingView)
        {
            _graphicsQualitySettingPresenter ??= _graphicsQualitySettingPresenter = new GraphicsQualitySettingPresenter(_name, _saveKey, settingView, _localizedQualityLevelNames);
            return _graphicsQualitySettingPresenter;
        }
    }
}
