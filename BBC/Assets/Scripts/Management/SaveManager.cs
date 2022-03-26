using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts
{
    public class SaveManager : MonoBehaviour
    {
        public static void SaveCurrentSceneIndex()
        {
            PlayerPrefs.SetInt("SceneIndexToResume", GameManager.Instance.SceneIndex);
            Debug.Log("Сохранён номер текущего уровня!");
        }

        public static void SaveTemporaryTaskProgress(int earnedStarsCount)
        {
            var sceneIndex = GameManager.Instance.SceneIndex;
            var taskNumber = GameManager.Instance.GetCurrentTaskNumber();
            var currentTaskEarnedStarsCountKey = "Level " + sceneIndex + " Task " + taskNumber + " Stars Earned";         
            if (!PlayerPrefs.HasKey(currentTaskEarnedStarsCountKey) || earnedStarsCount > PlayerPrefs.GetInt(currentTaskEarnedStarsCountKey))
                PlayerPrefs.SetInt("Temporary " + currentTaskEarnedStarsCountKey, earnedStarsCount);
            Debug.Log("Текущий прогресс временно сохранён!");
        }

        public static void SaveFinishedLevelProgress()
        {
            var sceneIndex = GameManager.Instance.SceneIndex;
            for (var i = 1; i <= GameManager.Instance.GetTasksCount(); i++)
            {
                var currentTaskEarnedStarsCountKey = "Level " + sceneIndex + " Task " + i + " Stars Earned";
                PlayerPrefs.SetInt(currentTaskEarnedStarsCountKey, PlayerPrefs.GetInt("Temporary " + currentTaskEarnedStarsCountKey));
                PlayerPrefs.DeleteKey("Temporary " + currentTaskEarnedStarsCountKey);
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
