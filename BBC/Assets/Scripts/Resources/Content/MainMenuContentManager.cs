using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace Scripts
{
    public class MainMenuContentManager : MonoBehaviour
    {
        private string contentRootFolderPath = "Data/";

        private static List<TaskInfo[]> taskInfos;
        private static LevelInfo[] levelInfos;
        private static List<Sprite> loadingScreens;

        public static TaskInfo GetTaskInfo(int levelNumber, int taskNumber) => taskInfos[levelNumber - 1][taskNumber -  1];

        public static TaskInfo[] GetLevelTaskInfos(int levelNumber) => taskInfos[levelNumber - 1];

        public static LevelInfo GetLevelInfo(int levelNumber) => levelInfos[levelNumber - 1];

        public static Sprite GetLoadingScreen(int levelNumber) => loadingScreens[levelNumber - 1];

        public void LoadContentFromResources(int availableLevelsCount)
        {
            var localizedContentFolderPath = contentRootFolderPath + LocalizationSettings.SelectedLocale.Identifier.Code;
            levelInfos = LoadDatasFromFile<LevelInfo>(localizedContentFolderPath + "/Main Menu/Level Infos").Take(availableLevelsCount).ToArray();
            loadingScreens = Resources.LoadAll<Sprite>("Loading Screens").ToList();
            taskInfos = LoadDatasFromFiles<TaskInfo>(localizedContentFolderPath + "/Game/Tasks", availableLevelsCount);
        }

        private List<T[]> LoadDatasFromFiles<T>(string folderPath, int filesCount)
        {
            var data = new List<T[]>();
            var files = Resources.LoadAll<TextAsset>(folderPath);
            for (var i = 0; i < filesCount; i++)
            {
                try
                {
                    data.Add(JsonConvert.DeserializeObject<T[]>(files[i].text));
                }
                catch
                {
                    Debug.LogError("Некорректный текст JSON в файле " + files[i].name);
                    data.Add(null);
                }
            }
            return data;
        }

        private T[] LoadDatasFromFile<T>(string resourcePath)
        {
            var resources = Resources.Load<TextAsset>(resourcePath);
            try
            {
                return JsonConvert.DeserializeObject<T[]>(resources.text);
            }
            catch
            {
                Debug.LogError("Некорректный текст JSON в файле " + resources.name);
                return null;
            }
        }
    }
}
