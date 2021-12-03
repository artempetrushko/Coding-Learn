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

            var player = GameManager.Instance.Player;
            PlayerPrefs.SetFloat("PositionX", player.transform.position.x);
            PlayerPrefs.SetFloat("PositionY", player.transform.position.y);
            PlayerPrefs.SetFloat("PositionZ", player.transform.position.z);

            PlayerPrefs.SetFloat("RotationX", player.transform.eulerAngles.x);
            PlayerPrefs.SetFloat("RotationY", player.transform.eulerAngles.y);
            PlayerPrefs.SetFloat("RotationZ", player.transform.eulerAngles.z);

            PlayerPrefs.SetInt("CoinsCount", GameManager.Instance.CoinsCount);
            PlayerPrefs.SetInt("SceneIndex", SceneManager.GetActiveScene().buildIndex);

            var availableTipsCounts = GameManager.Instance.AvailableTipsCounts;
            for (var i = 0; i < availableTipsCounts.Count; i++)
                PlayerPrefs.SetInt("Available Tips Count (Task " + (i + 1) + ")", availableTipsCounts[i]);

            var coins = GameObject.Find("Coins");
            if (coins != null)
            {
                for (var i = 0; i < coins.transform.childCount; i++)
                    PlayerPrefs.SetInt("Coin " + i + " collected", coins.transform.GetChild(i).gameObject.activeInHierarchy ? 1 : 0);
            }
            Debug.Log("Сохранено!");
        }

        public static void Save_NextLevel()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.SetInt("CoinsCount", GameManager.Instance.CoinsCount);
            PlayerPrefs.SetInt("IsTransitToNextLevel", 1);
        }

        public static void Load()
        {
            var player = GameManager.Instance.Player;
            player.transform.position = new Vector3(PlayerPrefs.GetFloat("PositionX"), PlayerPrefs.GetFloat("PositionY"), PlayerPrefs.GetFloat("PositionZ"));
            player.transform.eulerAngles = new Vector3(PlayerPrefs.GetFloat("RotationX"), PlayerPrefs.GetFloat("RotationY"), PlayerPrefs.GetFloat("RotationZ"));
            GameManager.Instance.CoinsCount = PlayerPrefs.GetInt("CoinsCount");
            for (var i = 0; i < GameManager.Instance.HasTasksCompleted.Count; i++)
            {
                GameManager.Instance.HasTasksCompleted[i] = PlayerPrefs.GetInt("Task " + (i + 1) + " completed") == 1;
                var taskTriggers = GameManager.Instance.Player.GetComponentInChildren<TriggersBehaviour>().TaskTriggers;
                if (GameManager.Instance.HasTasksCompleted[i])
                    taskTriggers.transform.GetChild(i).gameObject.SetActive(false);
            }
            var coins = GameObject.Find("Coins");
            if (coins != null)
            {
                for (var i = 0; i < coins.transform.childCount; i++)
                    coins.transform.GetChild(i).gameObject.SetActive(PlayerPrefs.GetInt("Coin " + i + " collected") == 1);
            }
        }

        public static void Load_NextLevel()
        {
            GameManager.Instance.CoinsCount = PlayerPrefs.GetInt("CoinsCount");
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
