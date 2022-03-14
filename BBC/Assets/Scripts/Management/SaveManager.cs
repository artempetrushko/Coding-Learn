using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts
{
    public class SaveManager : MonoBehaviour
    {
        public static void SaveTaskProgress(int earnedStarsCount)
        {
            var sceneIndex = GameManager.Instance.SceneIndex;
            var taskNumber = GameManager.Instance.CurrentTaskNumber;
            var currentTaskEarnedStarsCountKey = "Level " + sceneIndex + " Task " + taskNumber + " Stars Earned";
            PlayerPrefs.SetInt("SceneIndex", SceneManager.GetActiveScene().buildIndex);
            PlayerPrefs.SetInt("Is Level " + sceneIndex + " Task " + taskNumber + " Completed", 1);            
            if (PlayerPrefs.HasKey(currentTaskEarnedStarsCountKey) && earnedStarsCount > PlayerPrefs.GetInt(currentTaskEarnedStarsCountKey))
                PlayerPrefs.SetInt(currentTaskEarnedStarsCountKey, earnedStarsCount);
            PlayerPrefs.SetInt("Level " + sceneIndex + "Task Number To Resume", taskNumber + 1);
            Debug.Log("Сохранено!");
        }

        public static int LoadTaskNumberToResume(int sceneIndex) => PlayerPrefs.GetInt("Level " + sceneIndex + "Task Number To Resume");

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
