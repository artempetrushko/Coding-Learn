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
        [SerializeField] public int timeToNextTip = 180;
        [Header("Номер текущего задания")]
        [SerializeField] private int currentTaskNumber = 1;        
        [Header("Количество доступных тем в справочнике")]
        [SerializeField] private int availableThemesCount;
        [Header("Катсцены")]
        [SerializeField] private List<PlayableAsset> cutscenes;

        [HideInInspector] public bool IsTimerStopped;

        public int SceneIndex { get; private set; }  
        public int SpentTime { get; private set; }      
        public List<TipsData> AvailableTipsData { get; private set; } = new List<TipsData>();

        private PlayableDirector playableDirector;

        public int GetCurrentTaskNumber() => currentTaskNumber;

        public int GetTimeToNextTip() => timeToNextTip;

        public int GetAvailableThemesCount() => availableThemesCount;   

        public TaskText GetCurrentTask() => ResourcesData.TaskTexts[SceneIndex - 1][currentTaskNumber - 1];

        public string GetTests() => string.Copy(ResourcesData.Tests[currentTaskNumber - 1]);

        public void ChangeCutsceneCurrentTime(float newTime) => playableDirector.time = newTime;

        public string GetNewTipText()
        {
            AvailableTipsData[currentTaskNumber - 1].Amount--;
            AvailableTipsData[currentTaskNumber - 1].IsShown = true;
            var tipNumber = ResourcesData.Tips[currentTaskNumber - 1].Length - AvailableTipsData[currentTaskNumber - 1].Amount;
            return ResourcesData.Tips[currentTaskNumber - 1][tipNumber].Tip;
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

        private void OnEnable()
        {
            InitializeGameManager();
            SceneIndex = SceneManager.GetActiveScene().buildIndex;
            var currentLanguage = SaveManager.SaveData.Language;
            gameObject.GetComponent<UiLocalizationScript>().GetResourcesByCurrentLanguage(currentLanguage);
            for (var i = 0; i < ResourcesData.TaskTexts[SceneIndex - 1].Length; i++)
            {
                AvailableTipsData.Add(new TipsData(ResourcesData.Tips[i].Length));
            }
        }

        private void InitializeGameManager()
        {
            if (Instance == null)
                Instance = this;
        }
    }
}
