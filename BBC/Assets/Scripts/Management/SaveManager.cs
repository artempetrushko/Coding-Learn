using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Leguar.TotalJSON;

namespace Scripts
{
    public class SaveManager : MonoBehaviour
    {
        public static SaveData SaveData { get; private set; }

        private static int sceneIndex;

        public static void ChangeLanguageData(Language newLanguage)
        {
            SaveData.Language = newLanguage;
            SerializeAndWriteData(SaveData);
        }

        public static void SaveTemporaryChallengeProgress(int challengeNumber)
        {
            var taskNumber = GameManager.Instance.GetCurrentTaskNumber();
            PlayerPrefs.SetInt("Temporary Level " + sceneIndex + " Task " + taskNumber + " Challenge " + challengeNumber + " completed", 1);
        }

        public static void SaveFinishedLevelProgress()
        {
            var currentLevelTaskChallenges = ResourcesData.TaskChallenges[sceneIndex - 1];
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
        
        public static void SerializeAndWriteData(SaveData savedData)
        {
            var serializedData = JSON.Serialize(savedData).CreatePrettyString();
            File.WriteAllText(Application.persistentDataPath + "/save.json", serializedData);
            Debug.Log("Данные сохранены в " + Application.persistentDataPath + "/save.json");
        }

        public static SaveData LoadSavedData()
        {
            try
            {
                var serializedData = File.ReadAllText(Application.persistentDataPath + "/save.json");
                return JSON.ParseString(serializedData).Deserialize<SaveData>();
            }
            catch
            {
                return null;
            }
        }    

        private static void SaveCurrentLevelNumber(int currentLevelNumber)
        {
            if (currentLevelNumber > SaveData.LastAvailableLevelNumber)
                SaveData.LastAvailableLevelNumber = currentLevelNumber;
            SaveData.LevelNumberToResume = currentLevelNumber;
            SerializeAndWriteData(SaveData);
            Debug.Log("Сохранён номер текущего уровня!");
        }

        private static void CreateDefaultSaveFile()
        {
            var savedData = new SaveData();
            savedData.Language = Language.EN;
            savedData.LevelNumberToResume = -1;
            savedData.LastAvailableLevelNumber = 1;
            savedData.ChallengeCompletingStatuses = new List<List<List<bool>>> { new List<List<bool>> { new List<bool> { } } };
            SaveData = savedData;
        }

        private void Awake()
        {
            var savedData = LoadSavedData();
            if (savedData != null)
            {
                SaveData = savedData;
            }
            else
            {
                CreateDefaultSaveFile();
            }

            sceneIndex = SceneManager.GetActiveScene().buildIndex;
            if (sceneIndex > 0)
                SaveCurrentLevelNumber(sceneIndex);
        }
    }
}
