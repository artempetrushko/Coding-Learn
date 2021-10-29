using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public class ScenarioMessage : Letter { }

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
    [HideInInspector] public ScenarioMessage[] ScenarioMessages;
    [HideInInspector] public LevelMessage[] StartMessages;
    [HideInInspector] public LevelMessage[] FinishMessages;
    [HideInInspector] public List<HandbookLetter[]> HandbookLetters;
    [HideInInspector] public List<TipMessage[]> Tips;
    [HideInInspector] public ThemeTitle[] ThemeTitles;
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
    public int TipsCount;  
    [Header("Номер текущего задания")]
    public int CurrentTaskNumber;
    [Header("Текущая цель")]
    public string Target;
    [Header("Количество доступных тем в справочнике")]
    public int AvailableThemesCount;
    [Header("Параметры панели подсказок")]
    [Tooltip("Стоимость одной подсказки")]
    public int TipPrice = 3;
    [Tooltip("Скидка при покупки нескольких подсказок (0 = 0%, 1 = 100%")]
    [Range(0, 1)] public float Sale = 0.15f;
    [Header("Предметы в инвентаре")]
    public List<InteractiveItem> ScriptItems = new List<InteractiveItem>();
    public List<InteractiveItem> OtherItems = new List<InteractiveItem>();
    public List<InteractiveItem> Notes = new List<InteractiveItem>();
    
    [HideInInspector] public InteractivePuzzle CurrentInteractivePuzzle;
    [HideInInspector] public int SceneIndex;    
    [Tooltip("Кол-во предметов, необходимых для прохождения задания")]
    [HideInInspector] public int TaskItemsCount;
    [HideInInspector] public bool IsTaskStarted;
    [HideInInspector] public List<bool> HasTasksCompleted;   
    [HideInInspector] public List<int> AvailableTipsCounts;  

    public Test GetTests() => Tests[CurrentTaskNumber - 1];

    public string GetNewTipText()
    {
        TipsCount--;
        AvailableTipsCounts[CurrentTaskNumber - 1]--;
        var tipNumber = Tips[CurrentTaskNumber - 1].Length - AvailableTipsCounts[CurrentTaskNumber - 1];
        return Tips[CurrentTaskNumber - 1][tipNumber].Tip;      
    }

    public void BuyTips(int tipsAmount)
    {
        TipsCount += tipsAmount;
        var totalCost = tipsAmount > 1 ? TipPrice : (int)(TipPrice * tipsAmount * (1 - Sale));
        CoinsCount -= totalCost;
    }

    private void Awake()
    {
        InitializeGameManager();
        TaskItemsCount = 0;
        SceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (SceneIndex == SceneManager.sceneCountInBuildSettings - 1)
            SceneIndex = 0;
        GetDataFromFiles();
        HasTasksCompleted = new List<bool>();
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
        if (SceneIndex > 0)
        {
            for (var i = 0; i < TaskTexts.Length; i++)
                AvailableTipsCounts.Add(Tips[i].Length);
        }
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
        var tasksFile = Resources.Load<TextAsset>("Tasks/Tasks Level " + SceneIndex);
        var testsFile = Resources.Load<TextAsset>("Tests/Tests Level " + SceneIndex);
        var scenarioMessagesFile = Resources.Load<TextAsset>("Scenario Messages Level " + SceneIndex);
        var startMessagesFile = Resources.Load<TextAsset>("Start Messages");
        var finishMessagesFile = Resources.Load<TextAsset>("Finish Messages");
        var themeTitles = Resources.Load<TextAsset>("Handbook Files/Theme Titles");
        TaskTexts = JsonHelper.FromJson<TaskText>(tasksFile.text);
        Tests = JsonHelper.FromJson<Test>(testsFile.text);
        StartMessages = JsonHelper.FromJson<LevelMessage>(startMessagesFile.text);
        FinishMessages = JsonHelper.FromJson<LevelMessage>(finishMessagesFile.text);
        ThemeTitles = JsonHelper.FromJson<ThemeTitle>(themeTitles.text);
        HandbookLetters = new List<HandbookLetter[]>();
        for (var i = 0; i < SceneManager.sceneCountInBuildSettings - 1; i++)
        {
            var handbookLettersFile = Resources.Load<TextAsset>("Handbook Files/Handbook Letters Level " + i);
            HandbookLetters.Add(JsonHelper.FromJson<HandbookLetter>(handbookLettersFile.text));
        }
        if (SceneIndex > 0)
        {
            Tips = new List<TipMessage[]>();
            for (var i = 1; i <= TaskTexts.Length; i++)
            {
                var tipsFile = Resources.Load<TextAsset>("Tips/Tips Level " + SceneIndex + " Task " + i);
                Tips.Add(JsonHelper.FromJson<TipMessage>(tipsFile.text));
            }
        }
        if (SceneIndex == 3)
            ScenarioMessages = JsonHelper.FromJson<ScenarioMessage>(scenarioMessagesFile.text);
    }
}
