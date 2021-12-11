using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts
{
    public class SaveManager : MonoBehaviour
    {
        public static void Save()
        {
            for (var i = 0; i < GameManager.Instance.HasTasksCompleted.Count; i++)
                PlayerPrefs.SetInt("Task " + (i + 1) + " completed", GameManager.Instance.HasTasksCompleted[i] ? 1 : 0);

            PlayerPrefs.SetInt("SceneIndex", SceneManager.GetActiveScene().buildIndex);

            var availableTipsCounts = GameManager.Instance.AvailableTipsCounts;
            for (var i = 0; i < availableTipsCounts.Count; i++)
                PlayerPrefs.SetInt("Available Tips Count (Task " + (i + 1) + ")", availableTipsCounts[i]);

            Debug.Log("Сохранено!");
        }

        public static void Load()
        {
            for (var i = 0; i < GameManager.Instance.HasTasksCompleted.Count; i++)
            {
                GameManager.Instance.HasTasksCompleted[i] = PlayerPrefs.GetInt("Task " + (i + 1) + " completed") == 1;
                var taskTriggers = GameManager.Instance.Player.GetComponentInChildren<TriggersBehaviour>().TaskTriggers;
                if (GameManager.Instance.HasTasksCompleted[i])
                    taskTriggers.transform.GetChild(i).gameObject.SetActive(false);
            }
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
