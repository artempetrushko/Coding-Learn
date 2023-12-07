using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace Scripts
{
    public class MainMenuSaveManager : SaveManager
    {
        public static GameProgressData GameProgressData => gameProgressData;
        public static SettingsData SettingsData => settingsData;

        public void LoadOrCreateGameProgressData(int levelsCount)
        {
            var gameProgress = LoadSavedData<GameProgressData>();
            if (gameProgress != null)
            {
                gameProgressData = gameProgress;
            }
            else
            {
                gameProgressData = new GameProgressData()
                {
                    LastAvailableLevelNumber = 1,
                    AllChallengeStatuses = new LevelChallengesResults[levelsCount].Select(item => item = new LevelChallengesResults()).ToArray()
                };
                SerializeAndSaveData(gameProgressData);
            }
        }

        public void LoadOrCreateSettingsData()
        {
            var settings = LoadSavedData<SettingsData>();
            if (settings != null)
            {
                settingsData = settings;
            }
            else
            {
                settingsData = new SettingsData()
                {
                    Resolution = string.Format(@"{0} x {1}", Screen.currentResolution.width, Screen.currentResolution.height),
                    FullScreenMode = Screen.fullScreenMode,
                    GraphicsQuality = QualitySettings.names[QualitySettings.GetQualityLevel()],
                    LanguageCode = LocalizationSettings.SelectedLocale.Identifier.Code,
                    SoundsVolume = 100,
                    MusicVolume = 100,
                };
                SerializeAndSaveData(settingsData);
            }
        }

        public void SaveSettings() => SerializeAndSaveData(settingsData);
    }
}
