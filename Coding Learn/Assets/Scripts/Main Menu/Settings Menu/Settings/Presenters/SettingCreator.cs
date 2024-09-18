using UnityEngine;
using UnityEngine.Localization;

namespace MainMenu
{
    public abstract class SettingCreator : ScriptableObject
    {
        [SerializeField] protected LocalizedString _name;
        [SerializeField] protected string _saveKey;

        public abstract SettingView SettingViewPrefab { get; }

        public abstract SettingPresenter CreateSetting(SettingView settingView);
    }
}
