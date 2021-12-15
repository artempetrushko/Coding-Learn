using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

namespace Scripts
{
    public class TipsData
    {
        public int Amount;
        public bool IsShown;

        public TipsData(int count)
        {
            Amount = count;
            IsShown = false;
        }
    }

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
            public string StartCode;
        }

        [Serializable]
        public class Story
        {
            public string Script;
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

        [Serializable]
        public class Challenges
        {
            public string Challenge;
            public double CheckValue;
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

        #region Данные из JSON и текстовых файлов
        [HideInInspector] public List<Story[]> StoryParts = new List<Story[]>();
        [HideInInspector] public ThemeTitle[] ThemeTitles;
        [HideInInspector] public List<Challenges[]> TaskChallenges = new List<Challenges[]>();
        [HideInInspector] public List<TaskText[]> TaskTexts = new List<TaskText[]>();
        [HideInInspector] public List<string> Tests = new List<string>();     
        [HideInInspector] public List<TipMessage[]> Tips = new List<TipMessage[]>();     
        [HideInInspector] public List<List<CodingTrainingInfo[]>> CodingTrainingInfos = new List<List<CodingTrainingInfo[]>>();     
        #endregion

        [Tooltip("Ссылка на GameManager для доступа из других скриптов")]
        public static GameManager Instance = null;

        [Header("Игрок")]
        [HideInInspector] public GameObject Player;
        [Header("Текущие камеры")]
        [HideInInspector] public Camera CurrentSceneCamera;
        [HideInInspector] public Camera CurrentDialogCamera;        
        [HideInInspector] public int CoinsCount;
        [Header("Счётчики")]
        public int TimeToNextTip = 180;
        [Header("Номер текущего задания")]
        public int CurrentTaskNumber = 1;
        [Header("Текущая цель")]
        [HideInInspector] public string Target;
        [Header("Количество доступных тем в справочнике")]
        public int AvailableThemesCount;
        [Header("Катсцены")]
        public List<PlayableAsset> Cutscenes;

        [HideInInspector] public List<InteractiveItem> ScriptItems = new List<InteractiveItem>();
        [HideInInspector] public List<InteractiveItem> OtherItems = new List<InteractiveItem>();
        [HideInInspector] public List<InteractiveItem> Notes = new List<InteractiveItem>();
        [HideInInspector] public InteractivePuzzle CurrentInteractivePuzzle;
        [HideInInspector] public ScriptTrigger CurrentScriptTrigger;

        [HideInInspector] public int SceneIndex;
        [HideInInspector] public bool IsTaskStarted;
        [HideInInspector] public int SpentTime;
        [HideInInspector] public bool IsTimerStopped;
        [HideInInspector] public List<bool> HasTasksCompleted = new List<bool>();
        [HideInInspector] public List<TipsData> AvailableTipsData = new List<TipsData>();

        public TaskText GetCurrentTask() => TaskTexts[SceneIndex][CurrentTaskNumber - 1];

        public CodingTrainingInfo[] GetCurrentCodingTrainingInfo() => CodingTrainingInfos[SceneIndex][CurrentTaskNumber - 1];

        public CodingTrainingInfo[] GetCodingTrainingInfo(int themeNumber, int subThemeNumber) => CodingTrainingInfos[themeNumber][subThemeNumber];

        public string GetTests() => string.Copy(Tests[CurrentTaskNumber - 1]);

        public string GetNewTipText()
        {
            AvailableTipsData[CurrentTaskNumber - 1].Amount--;
            AvailableTipsData[CurrentTaskNumber - 1].IsShown = true;
            var tipNumber = Tips[CurrentTaskNumber - 1].Length - AvailableTipsData[CurrentTaskNumber - 1].Amount;
            return Tips[CurrentTaskNumber - 1][tipNumber].Tip;
        }

        public void PlayNextCutscene()
        {
            CurrentTaskNumber++;
            PlayCutscene(CurrentTaskNumber);
        }

        public IEnumerator StartTimer_COR()
        {
            SpentTime = 0;
            IsTimerStopped = false;
            while (!IsTimerStopped)
            {
                yield return new WaitForSeconds(1f);
                SpentTime++;
            }
        }

        private void PlayCutscene(int cutsceneNumber)
        {
            var playableDirector = gameObject.GetComponent<PlayableDirector>();
            playableDirector.time = 0;
            playableDirector.Play(Cutscenes[cutsceneNumber - 1]);
        }

        private void Start()
        {
            PlayCutscene(CurrentTaskNumber);
        }

        private void Awake()
        {
            InitializeGameManager();
            SceneIndex = SceneManager.GetActiveScene().buildIndex;
            if (SceneIndex == SceneManager.sceneCountInBuildSettings - 1)
                SceneIndex = 0;
            GetDataFromFiles();
            for (var i = 0; i < TaskTexts[SceneIndex].Length; i++)
            {
                AvailableTipsData.Add(new TipsData(Tips[i].Length));
                HasTasksCompleted.Add(false);
            }
        }

        private void InitializeGameManager()
        {
            if (Instance == null)
                Instance = this;
        }

        private void GetDataFromFiles()
        {         
            ThemeTitles = GetResourcesAndWrite<ThemeTitle>("Data/Handbook Files/Theme Titles");
            for (var i = 0; i < SceneManager.sceneCountInBuildSettings - 1; i++)
                TaskTexts.Add(GetResourcesAndWrite<TaskText>("Data/Tasks/Tasks Level " + i));
            for (var i = 1; i <= TaskTexts[SceneIndex].Length + 1; i++)
                StoryParts.Add(GetResourcesAndWrite<Story>("Data/Story/Level " + SceneIndex + "/Story Level " + SceneIndex + " Part " + i));
            for (var i = 1; i <= TaskTexts[SceneIndex].Length; i++)
            {
                Tests.Add(Resources.Load<TextAsset>("Data/Tests/Tests Level " + SceneIndex + " Task " + i).text);
                Tips.Add(GetResourcesAndWrite<TipMessage>("Data/Tips/Level " + SceneIndex + "/Tips Level " + SceneIndex + " Task " + i));
                TaskChallenges.Add(GetResourcesAndWrite<Challenges>("Data/Challenges/Level " + SceneIndex + "/Challenges Level " + SceneIndex + " Task " + i));
            }
            for (var i = 0; i <= SceneIndex; i++)
            {
                CodingTrainingInfos.Add(new List<CodingTrainingInfo[]>());
                for (var j = 1; j <= TaskTexts[i].Length; j++)
                    CodingTrainingInfos[i].Add(GetResourcesAndWrite<CodingTrainingInfo>("Data/Coding Training/Level " + i + "/Coding Training Level " + i + " Task " + j));
            }
        }

        private T[] GetResourcesAndWrite<T>(string resourcePath)
        {
            var resources = Resources.Load<TextAsset>(resourcePath);
            return JsonHelper.FromJson<T>(resources.text);
        }
    }
}
