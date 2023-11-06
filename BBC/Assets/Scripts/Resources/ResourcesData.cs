using Leguar.TotalJSON;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Scripts
{
    public class ResourcesData : MonoBehaviour
    {
        public static List<Story[]> StoryParts { get; private set; }
        public static ThemeTitle[] ThemeTitles { get; private set; }       
        public static List<TaskText[]> TaskTexts { get; private set; } 
        public static List<string> Tests { get; private set; }
        public static List<TipMessage[]> Tips { get; private set; }
        public static List<List<Challenges[]>> TaskChallenges { get; private set; }
        public static List<List<CodingTrainingInfo[]>> CodingTrainingInfos { get; private set; }

        private string contentRootFolderPath = "Data/";

        public static CodingTrainingInfo[] GetCodingTrainingInfo(int themeNumber, int subThemeNumber) => CodingTrainingInfos[themeNumber][subThemeNumber];

        public void LoadContentFromResources(int currentLevelNumber, Language selectedLanguage)
        {
            var localizedContentFolderPath = contentRootFolderPath + selectedLanguage.ToString();
            StoryParts = LoadDataFromResources<Story>(localizedContentFolderPath + "/Story/Level " + currentLevelNumber);
            Tips = LoadDataFromResources<TipMessage>(localizedContentFolderPath + "/Tips/Level " + currentLevelNumber);
            TaskTexts = LoadDataFromResources<TaskText>(localizedContentFolderPath + "/Tasks");
            ThemeTitles = GetResourcesAndWrite<ThemeTitle>(localizedContentFolderPath + "/Coding Training/Theme Titles");
            TaskChallenges = LoadManyDataFromResources<Challenges>(localizedContentFolderPath + "/Challenges/Level ", currentLevelNumber);
            CodingTrainingInfos = LoadManyDataFromResources<CodingTrainingInfo>(localizedContentFolderPath + "/Coding Training/Level ", currentLevelNumber);

            Tests = new List<string>();
            var files = Resources.LoadAll<TextAsset>("Tests/Level " + currentLevelNumber);
            foreach (var file in files)
            {
                Tests.Add(file.text);
            }
        }

        /*private void LoadData_MenuRequired()
        {
            var language = Language.EN;
            var levelsCount = 1;
            var savedData = LoadSavedData();
            if (savedData != null)
            {
                language = savedData.Language;
                levelsCount = savedData.LastAvailableLevelNumber;
            }
            LoadChallenges(language, levelsCount);
            LoadTasks(language);
        }*/

        private List<List<T[]>> LoadManyDataFromResources<T>(string commonFolderPath, int foldersCount)
        {
            var data = new List<List<T[]>>();
            for (var i = 1; i <= foldersCount; i++)
            {
                data.Add(LoadDataFromResources<T>(commonFolderPath + i));
            }
            return data;
        }

        private List<T[]> LoadDataFromResources<T>(string folderPath)
        {
            var data = new List<T[]>();
            var files = Resources.LoadAll<TextAsset>(folderPath);
            foreach (var file in files)
            {
                try 
                { 
                    data.Add(JsonHelper.FromJson<T>(file.text)); 
                }
                catch
                {
                    Debug.LogError("Некорректный текст JSON в файле " + file.name);
                }               
            }
            return data;
        }

        private T[] GetResourcesAndWrite<T>(string resourcePath)
        {
            var resources = Resources.Load<TextAsset>(resourcePath);
            try
            {
                var deserializedResource = JsonHelper.FromJson<T>(resources.text);
                return deserializedResource;
            }
            catch
            {
                Debug.LogError("Некорректный текст JSON в файле " + resources.name);
                return null;
            }
        }
    }
}
