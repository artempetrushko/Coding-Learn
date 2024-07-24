using UnityEngine;

namespace Scripts
{
    [CreateAssetMenu(fileName = "Settings Config", menuName = "Game Data/Settings/Settings Config")]
    public class SettingsConfig : ScriptableObject
    {
        [SerializeField] private SettingData[] settingDatas;

        public SettingData[] SettingDatas => settingDatas;
    }
}
