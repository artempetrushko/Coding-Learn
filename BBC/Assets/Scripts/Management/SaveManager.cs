using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Scripts
{
    public class SaveManager : MonoBehaviour
    {
        public static void SaveCurrentSceneIndex()
        {
            var currentSceneIndex = GameManager.Instance.SceneIndex;
            if (currentSceneIndex > PlayerPrefs.GetInt("LastAvailableLevelNumber"))
                PlayerPrefs.SetInt("LastAvailableLevelNumber", currentSceneIndex);
            PlayerPrefs.SetInt("LevelNumberToResume", currentSceneIndex);
            Debug.Log("Сохранён номер текущего уровня!");
        }

        public static void SaveTemporaryChallengeProgress(int challengeNumber)
        {
            var sceneIndex = GameManager.Instance.SceneIndex;
            var taskNumber = GameManager.Instance.GetCurrentTaskNumber();
            PlayerPrefs.SetInt("Temporary Level " + sceneIndex + " Task " + taskNumber + " Challenge " + challengeNumber + " completed", 1);
        }

        public static void SaveFinishedLevelProgress()
        {
            var sceneIndex = GameManager.Instance.SceneIndex;
            for (var i = 1; i <= GameManager.Instance.GetTasksCount(); i++)
            {
                for (var j = 1; j <= 3; j++)
                {
                    var completedChallengeSaveKey = "Level " + sceneIndex + " Task " + i + " Challenge " + j + " completed";
                    var completedChallengeTemporarySaveKey = "Temporary " + completedChallengeSaveKey;
                    if (PlayerPrefs.HasKey(completedChallengeTemporarySaveKey))
                    {
                        PlayerPrefs.SetInt(completedChallengeSaveKey, PlayerPrefs.GetInt(completedChallengeTemporarySaveKey));
                        PlayerPrefs.DeleteKey(completedChallengeTemporarySaveKey);
                    }
                    else PlayerPrefs.SetInt(completedChallengeSaveKey, 0);
                }                
            }
            Debug.Log("Прогресс уровня сохранён!");
        }

        public static void DeleteSavedDialogueData()
        {
            var sceneIndex = GameManager.Instance.SceneIndex;
            var searchPattern = "DialogTrigger_" + sceneIndex + "_*";
            var dialogueDataFiles = Directory.GetFiles(Application.dataPath, searchPattern, SearchOption.AllDirectories);
            if (dialogueDataFiles.Length != 0)
            {
                foreach (var file in dialogueDataFiles)
                    File.Delete(file);
                Debug.Log(string.Format("Файлы сохраненных диалогов ({0} шт.) удалены!", dialogueDataFiles.Length / 2));
            }
        }
    }
}
