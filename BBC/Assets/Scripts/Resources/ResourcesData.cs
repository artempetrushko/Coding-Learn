using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        public static CodingTrainingInfo[] GetCodingTrainingInfo(int themeNumber, int subThemeNumber) => CodingTrainingInfos[themeNumber][subThemeNumber];

        private void LoadData()
        {
            var levelNumber = SceneManager.GetActiveScene().buildIndex;
            var language = Language.EN;
            var savedData = SaveManager.LoadSavedData();
            if (savedData != null)
            {
                language = savedData.Language;
            }
            LoadStoryParts(language, levelNumber);
            LoadChallenges(language, levelNumber);
            LoadTips(language, levelNumber);
            LoadTasks(language);
            LoadThemeTitles(language);
            LoadTests(levelNumber);
            LoadCodingTrainingInfos(language, levelNumber);
        }

        private void LoadData_MenuRequired()
        {
            var language = Language.EN;
            var levelsCount = 1;
            var savedData = SaveManager.LoadSavedData();
            if (savedData != null)
            {
                language = savedData.Language;
                levelsCount = savedData.LastAvailableLevelNumber;
            }
            LoadChallenges(language, levelsCount);
            LoadTasks(language);
        }

        private void LoadStoryParts(Language currentLanguage, int levelNumber) => StoryParts = LoadDataFromResources<Story>("Data/" + currentLanguage.ToString() + "/Story/Level " + levelNumber);

        private void LoadTips(Language currentLanguage, int levelNumber) => Tips = LoadDataFromResources<TipMessage>("Data/" + currentLanguage.ToString() + "/Tips/Level " + levelNumber);

        private void LoadTasks(Language currentLanguage) => TaskTexts = LoadDataFromResources<TaskText>("Data/" + currentLanguage.ToString() + "/Tasks");

        private void LoadThemeTitles(Language currentLanguage) => ThemeTitles = GetResourcesAndWrite<ThemeTitle>("Data/" + currentLanguage.ToString() + "/Coding Training/Theme Titles");

        private void LoadChallenges(Language currentLanguage, int levelsCount) => TaskChallenges = LoadManyDataFromResources<Challenges>("Data/" + currentLanguage.ToString() + "/Challenges/Level ", levelsCount);

        private void LoadCodingTrainingInfos(Language currentLanguage, int levelsCount) => CodingTrainingInfos = LoadManyDataFromResources<CodingTrainingInfo>("Data/" + currentLanguage.ToString() + "/Coding Training/Level ", levelsCount);

        private void LoadTests(int levelNumber)
        {
            Tests = new List<string>();
            var files = Resources.LoadAll<TextAsset>("Tests/Level " + levelNumber);
            foreach (var file in files)
            {
                Tests.Add(file.text);
            }
        }

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
                data.Add(JsonHelper.FromJson<T>(file.text));
            }
            return data;
        }  

        private T[] GetResourcesAndWrite<T>(string resourcePath)
        {
            var resources = Resources.Load<TextAsset>(resourcePath);
            return JsonHelper.FromJson<T>(resources.text);
        }

        private void Awake()
        {
            var sceneIndex = SceneManager.GetActiveScene().buildIndex;
            if (sceneIndex == 0)
            {
                LoadData_MenuRequired();
            }
            else
            {
                LoadData();
            }
        }
    }
}
