using System.Linq;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace Scripts
{
    public class MainMenuSaveController : SaveController
    {
        public SettingsData SettingsData { get; private set; }

        public void LoadOrCreateAllSavedData(int levelsCount)
        {
            GameProgressData = LoadOrCreateSavedData(new GameProgressData()
            {
                LastAvailableLevelNumber = 1,
                AllChallengeStatuses = new LevelChallengesResults[levelsCount].Select(item => item = new LevelChallengesResults()).ToArray()
            });
            SettingsData = LoadOrCreateSavedData(new SettingsData()
            {
                Resolution = string.Format(@"{0} x {1}", Screen.currentResolution.width, Screen.currentResolution.height),
                FullScreenMode = Screen.fullScreenMode,
                GraphicsQuality = QualitySettings.names[QualitySettings.GetQualityLevel()],
                Language = LocalizationSettings.SelectedLocale.LocaleName.Split()[0],
                SoundsVolume = 100,
                MusicVolume = 100,
            });
        }

        public void SaveSettings() => SerializeAndSaveData(SettingsData);
    }
}
