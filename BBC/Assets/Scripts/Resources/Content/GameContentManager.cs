using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization.Settings;
using Newtonsoft.Json;

namespace Scripts
{
    public class GameContentManager : MonoBehaviour
    {
        private string contentRootFolderPath = "Data/";

        private static StoryInfo[] storyParts;
        private static TrainingTheme[] codingTrainingInfos;
        private static TaskInfo[] taskInfos;
        private static string[] tests;     

        public static StoryInfo GetStoryInfo(int cutsceneNumber) => storyParts[cutsceneNumber - 1];

        public static TrainingTheme GetCodingTrainingTheme(int themeNumber) => codingTrainingInfos[themeNumber - 1];

        public static TrainingTheme[] GetFirstCodingTrainingThemes(int themesCount) => codingTrainingInfos.Take(themesCount).ToArray();

        public static TaskInfo GetTaskInfo(int taskNumber) => taskInfos[taskNumber - 1];

        public static string GetTests(int taskNumber) => tests[taskNumber - 1];

        public void LoadContentFromResources(int currentLevelNumber)
        {
            var localizedContentFolderPath = contentRootFolderPath + LocalizationSettings.SelectedLocale.Identifier.Code;
            storyParts = LoadDatasFromFile<StoryInfo>(localizedContentFolderPath + "/Game/Story/Story Level " + currentLevelNumber);
            taskInfos = LoadDatasFromFile<TaskInfo>(localizedContentFolderPath + "/Game/Tasks/Tasks Level " + currentLevelNumber);
            tests = Resources.LoadAll<TextAsset>("Tests/Level " + currentLevelNumber).Select(file => file.text).ToArray();

            codingTrainingInfos = Resources.LoadAll<TextAsset>(localizedContentFolderPath + "/Game/Coding Training")
                .Take(currentLevelNumber)
                .Select(file =>
                {
                    try
                    {
                        return JsonConvert.DeserializeObject<TrainingTheme>(file.text);
                    }
                    catch
                    {
                        Debug.LogError("Некорректный текст JSON в файле " + file.name);
                        return null;
                    }
                })
                .ToArray();
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
