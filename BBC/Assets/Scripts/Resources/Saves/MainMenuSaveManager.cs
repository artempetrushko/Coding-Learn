using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Scripts
{
    public class MainMenuSaveManager : SaveManager
    {
        public static SaveData SaveData => saveData;

        public void LoadOrCreateSaveData(int levelsCount)
        {
            var savedData = LoadSavedData();
            if (savedData != null)
            {
                saveData = savedData;
            }
            else
            {
                saveData = new SaveData()
                {
                    LanguageCode = "en",
                    TotalLevelsCount = levelsCount,
                    LastAvailableLevelNumber = 1,
                    AllChallengeStatuses = new LevelChallengesResults[levelsCount].Select(item => item = new LevelChallengesResults()).ToArray()
                };
                SerializeAndSaveData(saveData);
            }
        }

        public void ChangeLanguageData(string newLanguageCode)
        {
            saveData.LanguageCode = newLanguageCode;
            SerializeAndSaveData(saveData);
        }
    }
}
