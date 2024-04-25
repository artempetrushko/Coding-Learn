using System.Linq;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace Scripts
{
    public class MainMenuSaveManager : SaveManager
    {
        public static GameProgressData GameProgressData => gameProgressData;
        public static SettingsData SettingsData => settingsData;

        public void LoadOrCreateAllSavedData(int levelsCount)
        {
            gameProgressData = LoadOrCreateSavedData(new GameProgressData()
            {
                LastAvailableLevelNumber = 1,
                AllChallengeStatuses = new LevelChallengesResults[levelsCount].Select(item => item = new LevelChallengesResults()).ToArray()
            });
            settingsData = LoadOrCreateSavedData(new SettingsData()
            {
                Resolution = string.Format(@"{0} x {1}", Screen.currentResolution.width, Screen.currentResolution.height),
                FullScreenMode = Screen.fullScreenMode,
                GraphicsQuality = QualitySettings.names[QualitySettings.GetQualityLevel()],
                Language = LocalizationSettings.SelectedLocale.LocaleName.Split()[0],
                SoundsVolume = 100,
                MusicVolume = 100,
            });
        }

        public void SaveSettings() => SerializeAndSaveData(settingsData);
    }
}
