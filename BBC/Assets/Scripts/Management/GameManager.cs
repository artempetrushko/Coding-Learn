using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts
{
    public class GameManager : MonoBehaviour
    {
        #region Сериализуемые классы  
        public class Letter
        {
            public string Title;
            public string Description;
        }

        [Serializable]
        public class TaskText : Letter
        {
            public int ID;
            public string ExtendedDescription;
            public string StartCode;
        }

        [Serializable]
        public class LevelMessage : Letter
        {
            public int LevelNumber;
        }

        [Serializable]
        public class HandbookLetter : Letter { }

        [Serializable]
        public class Test
        {
            public int TaskNumber;
            public string ExtraCode;
            public string TestCode;
        }

        [Serializable]
        public class TipMessage
        {
            public string Tip;
        }

        [Serializable]
        public class ThemeTitle
        {
            public string Title;
        }

        [Serializable]
        public class CodingTrainingInfo
        {
            public string Title;
            public string Info;
            public string VideoTitles;
        }

        public static class JsonHelper
        {
            public static T[] FromJson<T>(string json)
            {
                Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
                return wrapper.Items;
            }

            public static string ToJson<T>(T[] array, bool prettyPrint = false)
            {
                Wrapper<T> wrapper = new Wrapper<T>();
                wrapper.Items = array;
                return JsonUtility.ToJson(wrapper, prettyPrint);
            }

            [Serializable]
            private class Wrapper<T>
            {
                public T[] Items;
            }
        }
        #endregion

        #region Данные из JSON-файлов
        [HideInInspector] public TaskText[] TaskTexts;
        [HideInInspector] public Test[] Tests;
        [HideInInspector] public LevelMessage[] StartMessages;
        [HideInInspector] public LevelMessage[] FinishMessages;
        [HideInInspector] public List<HandbookLetter[]> HandbookLetters = new List<HandbookLetter[]>();
        [HideInInspector] public List<TipMessage[]> Tips = new List<TipMessage[]>();
        [HideInInspector] public ThemeTitle[] ThemeTitles;
        [HideInInspector] public List<CodingTrainingInfo[]> CodingTrainingInfos = new List<CodingTrainingInfo[]>();
        #endregion

        [Tooltip("Ссылка на GameManager для доступа из других скриптов")]
        public static GameManager Instance = null;

        [Header("Игрок")]
        public GameObject Player;
        [Header("Текущие камеры")]
        public Camera CurrentSceneCamera;
        public Camera CurrentDialogCamera;
        [Header("Счётчики")]
        public int CoinsCount;
        public int TimeToNextTip = 180;
        [Header("Номер текущего задания")]
        public int CurrentTaskNumber;
        [Header("Текущая цель")]
        public string Target;
        [Header("Количество доступных тем в справочнике")]
        public int AvailableThemesCount;

        [HideInInspector] public List<InteractiveItem> ScriptItems = new List<InteractiveItem>();
        [HideInInspector] public List<InteractiveItem> OtherItems = new List<InteractiveItem>();
        [HideInInspector] public List<InteractiveItem> Notes = new List<InteractiveItem>();
        [HideInInspector] public InteractivePuzzle CurrentInteractivePuzzle;

        [HideInInspector] public int SceneIndex;
        [Tooltip("Кол-во предметов, необходимых для прохождения задания")]
        [HideInInspector] public int TaskItemsCount;
        [HideInInspector] public bool IsTaskStarted;
        [HideInInspector] public List<bool> HasTasksCompleted = new List<bool>();
        [HideInInspector] public List<int> AvailableTipsCounts = new List<int>();

        public Test GetTests() => Tests[CurrentTaskNumber - 1];

        public string GetNewTipText()
        {
            AvailableTipsCounts[CurrentTaskNumber - 1]--;
            var tipNumber = Tips[CurrentTaskNumber - 1].Length - AvailableTipsCounts[CurrentTaskNumber - 1];
            return Tips[CurrentTaskNumber - 1][tipNumber].Tip;
        }

        private void Awake()
        {
            InitializeGameManager();
            CurrentTaskNumber = 1;
            TaskItemsCount = 0;
            SceneIndex = SceneManager.GetActiveScene().buildIndex;
            if (SceneIndex == SceneManager.sceneCountInBuildSettings - 1)
                SceneIndex = 0;
            GetDataFromFiles();
            for (var i = 0; i <= TaskTexts.Length; i++)
                HasTasksCompleted.Add(false);
            if (PlayerPrefs.HasKey("CoinsCount"))
            {
                if (PlayerPrefs.HasKey("IsTransitToNextLevel"))
                {
                    SaveManager.Load_NextLevel();
                    PlayerPrefs.DeleteAll();
                }
                else
                {
                    if (PlayerPrefs.GetInt("SceneIndex") == SceneIndex && SceneIndex <= 4) // второе условие позже убрать
                        SaveManager.Load();
                }
            }
            for (var i = 0; i < TaskTexts.Length; i++)
                AvailableTipsCounts.Add(Tips[i].Length);
            if (PlayerPrefs.HasKey("PositionX"))
            {
                for (var i = 0; i < AvailableTipsCounts.Count; i++)
                    AvailableTipsCounts[i] = PlayerPrefs.GetInt("Available Tips Count (Task " + (i + 1) + ")");
            }
        }

        private void InitializeGameManager()
        {
            if (Instance == null)
                Instance = this;
        }

        private void GetDataFromFiles()
        {
            TaskTexts = GetResourcesAndWrite<TaskText>("Tasks/Tasks Level " + SceneIndex);
            Tests = GetResourcesAndWrite<Test>("Tests/Tests Level " + SceneIndex);
            StartMessages = GetResourcesAndWrite<LevelMessage>("Start Messages");
            FinishMessages = GetResourcesAndWrite<LevelMessage>("Finish Messages");
            ThemeTitles = GetResourcesAndWrite<ThemeTitle>("Handbook Files/Theme Titles");
            for (var i = 0; i < SceneManager.sceneCountInBuildSettings - 1; i++)
                HandbookLetters.Add(GetResourcesAndWrite<HandbookLetter>("Handbook Files/Handbook Letters Level " + i));
            for (var i = 1; i <= TaskTexts.Length; i++)
            {
                Tips.Add(GetResourcesAndWrite<TipMessage>("Tips/Tips Level " + SceneIndex + " Task " + i));
                CodingTrainingInfos.Add(GetResourcesAndWrite<CodingTrainingInfo>("Coding Training/Coding Training Level " + SceneIndex + " Task " + i));
            }
        }

        private T[] GetResourcesAndWrite<T>(string resourcePath)
        {
            var resources = Resources.Load<TextAsset>(resourcePath);
            return JsonHelper.FromJson<T>(resources.text);
        }
    }
}
