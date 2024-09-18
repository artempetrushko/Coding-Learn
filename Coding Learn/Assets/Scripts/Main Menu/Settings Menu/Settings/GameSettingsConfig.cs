using UnityEngine;

namespace MainMenu
{
    [CreateAssetMenu(fileName = "Settings Config", menuName = "Game Configs/Settings/Settings Config")]
    public class GameSettingsConfig : ScriptableObject
    {
        [SerializeField] private SettingCreator[] _settingCreators;

        public SettingCreator[] SettingCreators => _settingCreators;
    }
}
