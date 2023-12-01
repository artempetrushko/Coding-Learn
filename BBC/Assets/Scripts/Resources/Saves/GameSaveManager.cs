using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    public class GameSaveManager : SaveManager
    {
        public static TaskChallengesResults GetCurrentTaskChallengesResults(int currentTaskNumber)
        {
            saveData.AllChallengeStatuses[GameManager.CurrentLevelNumber - 1].TasksChallengesResults[currentTaskNumber - 1] ??= new TaskChallengesResults();
            return saveData.AllChallengeStatuses[GameManager.CurrentLevelNumber - 1].TasksChallengesResults[currentTaskNumber - 1];
        }

        public void SaveProgress(int currentLevelNumber)
        {
            if (currentLevelNumber > saveData.LastAvailableLevelNumber)
            {
                saveData.LastAvailableLevelNumber = Mathf.Clamp(currentLevelNumber + 1, 1, saveData.TotalLevelsCount);
            }
            SerializeAndSaveData(saveData);
        }

        public void LoadSaveData() => saveData = LoadSavedData();
    }
}
