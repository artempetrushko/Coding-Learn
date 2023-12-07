using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    public class GameSaveManager : SaveManager
    {
        public static TaskChallengesResults GetCurrentTaskChallengesResults(int currentTaskNumber)
        {
            var tasksChallengesResults = gameProgressData.AllChallengeStatuses[GameManager.CurrentLevelNumber - 1].TasksChallengesResults;
            if (currentTaskNumber > tasksChallengesResults.Count)
            {
                tasksChallengesResults.Add(new TaskChallengesResults());
            }
            return tasksChallengesResults[currentTaskNumber - 1];
        }

        public void SaveProgress(int lastAvailableNumber)
        {
            if (lastAvailableNumber > gameProgressData.LastAvailableLevelNumber)
            {
                gameProgressData.LastAvailableLevelNumber = lastAvailableNumber;
            }          
            SerializeAndSaveData(gameProgressData);
        }

        public void LoadSaveData() => gameProgressData = LoadSavedData<GameProgressData>();
    }
}
