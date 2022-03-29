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

        #region Legacy
        [Header("Игрок")]
        [HideInInspector] public GameObject Player;
        [Header("Текущие камеры")]
        [HideInInspector] public Camera CurrentSceneCamera;
        [HideInInspector] public Camera CurrentDialogCamera;
        [Header("Текущая цель")]
        [HideInInspector] public string Target;
        [HideInInspector] public bool IsTaskStarted;
        [HideInInspector] public List<InteractiveItem> ScriptItems = new List<InteractiveItem>();
        [HideInInspector] public List<InteractiveItem> OtherItems = new List<InteractiveItem>();
        [HideInInspector] public List<InteractiveItem> Notes = new List<InteractiveItem>();
        [HideInInspector] public InteractivePuzzle CurrentInteractivePuzzle;
        #endregion

        [Header("Время подготовки подсказки (в секундах)")]
        [SerializeField] private int timeToNextTip  = 180;
        [Header("Номер текущего задания")]
        [SerializeField] private int currentTaskNumber = 1;        
        [Header("Количество доступных тем в справочнике")]
        [SerializeField] private int availableThemesCount;
        [Header("Катсцены")]
        [SerializeField] private List<PlayableAsset> cutscenes;

        [HideInInspector] public bool IsTimerStopped;
        [HideInInspector] public List<bool> HasTasksCompleted = new List<bool>();

        public int SceneIndex { get; private set; }  
        public int SpentTime { get; private set; }      
        public List<TipsData> AvailableTipsData { get; private set; } = new List<TipsData>();

        private PlayableDirector playableDirector;

        public int GetCurrentTaskNumber() => currentTaskNumber;

        public int GetTimeToNextTip() => timeToNextTip;

        public int GetAvailableThemesCount() => availableThemesCount;   

        public TaskText GetCurrentTask() => TaskTexts[SceneIndex - 1][currentTaskNumber - 1];

        public CodingTrainingInfo[] GetCodingTrainingInfo(int themeNumber, int subThemeNumber) => CodingTrainingInfos[themeNumber][subThemeNumber];

        public string GetTests() => string.Copy(Tests[currentTaskNumber - 1]);

        public int GetTasksCount() => TaskTexts[SceneIndex - 1].Length;

        public void ChangeCutsceneCurrentTime(float newTime) => playableDirector.time = newTime;

        public string GetNewTipText()
        {
            AvailableTipsData[currentTaskNumber - 1].Amount--;
            AvailableTipsData[currentTaskNumber - 1].IsShown = true;
            var tipNumber = Tips[currentTaskNumber - 1].Length - AvailableTipsData[currentTaskNumber - 1].Amount;
            return Tips[currentTaskNumber - 1][tipNumber].Tip;
        }

        public void PlayNextCutscene()
        {
            currentTaskNumber++;
            PlayCutscene(currentTaskNumber);
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
            ChangeCutsceneCurrentTime(0);
            playableDirector.Play(cutscenes[cutsceneNumber - 1]);
        }

        private void Start()
        {
            playableDirector = GetComponent<PlayableDirector>();
            PlayCutscene(currentTaskNumber);
        }

        private void Awake()
        {
            InitializeGameManager();
            SceneIndex = SceneManager.GetActiveScene().buildIndex;
            SaveManager.SaveCurrentSceneIndex();
            GetDataFromFiles();
            for (var i = 0; i < TaskTexts[SceneIndex - 1].Length; i++)
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
            var currentLanguage = (Language)PlayerPrefs.GetInt("Language");
            ThemeTitles = GetResourcesAndWrite<ThemeTitle>("Data/" + currentLanguage.ToString() + "/Coding Training/Theme Titles");
            for (var i = 1; i <= SceneManager.sceneCountInBuildSettings - 1; i++)
                TaskTexts.Add(GetResourcesAndWrite<TaskText>("Data/" + currentLanguage.ToString() + "/Tasks/Tasks Level " + i));
            for (var i = 1; i <= TaskTexts[SceneIndex - 1].Length + 1; i++)
                StoryParts.Add(GetResourcesAndWrite<Story>("Data/" + currentLanguage.ToString() + "/Story/Level " + SceneIndex + "/Story Part " + i));
            for (var i = 1; i <= TaskTexts[SceneIndex - 1].Length; i++)
            {
                Tests.Add(Resources.Load<TextAsset>("Tests/Level " + SceneIndex + "/Tests Task " + i).text);
                Tips.Add(GetResourcesAndWrite<TipMessage>("Data/" + currentLanguage.ToString() + "/Tips/Level " + SceneIndex + "/Tips Task " + i));
                TaskChallenges.Add(GetResourcesAndWrite<Challenges>("Data/" + currentLanguage.ToString() + "/Challenges/Level " + SceneIndex + "/Challenges Task " + i));
            }
            for (var i = 1; i <= SceneIndex; i++)
            {
                CodingTrainingInfos.Add(new List<CodingTrainingInfo[]>());
                for (var j = 1; j <= TaskTexts[i - 1].Length; j++)
                    CodingTrainingInfos[i - 1].Add(GetResourcesAndWrite<CodingTrainingInfo>("Data/" + currentLanguage.ToString() + "/Coding Training/Level " + i + "/Coding Training Task " + j));
            }
            gameObject.GetComponent<UiLocalizationScript>().GetResourcesByCurrentLanguage(currentLanguage);
        }

        private T[] GetResourcesAndWrite<T>(string resourcePath)
        {
            var resources = Resources.Load<TextAsset>(resourcePath);
            return JsonHelper.FromJson<T>(resources.text);
        }
    }
}
