using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Leguar.TotalJSON;

namespace Scripts
{
    public class SaveManager : MonoBehaviour
    {
        public static SaveData SaveData { get; private set; }

        private int sceneIndex;

        private string SaveFilePath => Application.persistentDataPath + "/save.json";

        public void LoadOrCreateSaveData()
        {
            var savedData = LoadSavedData();
            SaveData = savedData != null ? savedData : CreateDefaultSaveFile();
        }

        public void SaveCurrentLevelNumber(int currentLevelNumber)
        {
            if (currentLevelNumber > SaveData.LastAvailableLevelNumber)
                SaveData.LastAvailableLevelNumber = currentLevelNumber;
            SaveData.LevelNumberToResume = currentLevelNumber;
            SerializeAndWriteData(SaveData);
            Debug.Log("Сохранён номер текущего уровня!");
        }

        public void ChangeLanguageData(Language newLanguage)
        {
            SaveData.Language = newLanguage;
            SerializeAndWriteData(SaveData);
        }

        public void SaveTemporaryChallengeProgress(int currentTaskNumber, int challengeNumber, bool isChallengeCompleted)
        {
            PlayerPrefs.SetInt(string.Format(@"Temporary Level {0} Task {1} Challenge {2} completed", sceneIndex, currentTaskNumber, challengeNumber), isChallengeCompleted ? 1 : 0);
        }   

        public void SaveFinishedLevelProgress()
        {
            var currentLevelTaskChallenges = ContentManager.TaskChallenges[sceneIndex - 1];
            if (SaveData.ChallengeCompletingStatuses.Count < sceneIndex)
            {
                SaveData.ChallengeCompletingStatuses.Add(new List<List<bool>>());
            }
            for (var i = 1; i <= currentLevelTaskChallenges.Count; i++)
            {
                var savedCurrentLevelTaskChallenges = SaveData.ChallengeCompletingStatuses[sceneIndex - 1];
                if (savedCurrentLevelTaskChallenges.Count < i)
                {
                    savedCurrentLevelTaskChallenges.Add(new List<bool>());
                }
                for (var j = 1; j <= currentLevelTaskChallenges[i - 1].Length; j++)
                {
                    var challengeTemporarySaveKey = "Temporary Level " + sceneIndex + " Task " + i + " Challenge " + j + " completed";
                    var savedTaskChallenges = savedCurrentLevelTaskChallenges[i - 1];
                    if (savedTaskChallenges.Count < j)
                    {
                        savedTaskChallenges.Add(PlayerPrefs.GetInt(challengeTemporarySaveKey) == 1);
                    }
                    else
                    {
                        savedTaskChallenges[j - 1] = PlayerPrefs.GetInt(challengeTemporarySaveKey) == 1;
                    }
                }                
            }
            Debug.Log("Прогресс уровня сохранён!");
        }
        
        private void SerializeAndWriteData(SaveData savedData)
        {
            var serializedData = JSON.Serialize(savedData).CreatePrettyString();
            File.WriteAllText(SaveFilePath, serializedData);
            Debug.Log("Данные сохранены в " + SaveFilePath);
        }       

        private SaveData CreateDefaultSaveFile()
        {
            var savedData = new SaveData();
            savedData.Language = Language.EN;
            savedData.LevelNumberToResume = -1;
            savedData.LastAvailableLevelNumber = 1;
            savedData.ChallengeCompletingStatuses = new List<List<List<bool>>> { new List<List<bool>> { new List<bool> { } } };
            return savedData;
        }

        private SaveData LoadSavedData()
        {
            try
            {
                var serializedData = File.ReadAllText(SaveFilePath);
                return JSON.ParseString(serializedData).Deserialize<SaveData>();
            }
            catch
            {
                return null;
            }
        }
    }
}
