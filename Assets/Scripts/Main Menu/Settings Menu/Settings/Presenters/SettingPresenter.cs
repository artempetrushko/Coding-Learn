using UnityEngine.Localization;

namespace MainMenu
{
    public abstract class SettingPresenter
    {
        protected readonly LocalizedString _settingName;
        protected readonly string _saveKey;

        public SettingPresenter(LocalizedString settingName, string saveKey)
        {
            _settingName = settingName;
            _saveKey = saveKey;
        }

        public abstract void ApplyValue();
    }
}
