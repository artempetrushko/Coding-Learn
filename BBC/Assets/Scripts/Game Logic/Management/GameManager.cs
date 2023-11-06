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

        [Header("Номер текущего задания")]
        [SerializeField] private int currentTaskNumber = 1;        
        [Header("Количество доступных тем в справочнике")]
        [SerializeField] private int availableThemesCount;
        [Header("Время подготовки подсказки (в секундах)")]
        [SerializeField] private int timeToNextTip = 180;
        [Header("Время до возможности пропустить задание")]
        [SerializeField] private int timeToSkipTask = 720;

        public static int CurrentLevelNumber { get; private set; }  
        public List<TipsData> AvailableTipsData { get; private set; } = new List<TipsData>();

        public int GetCurrentTaskNumber() => currentTaskNumber;

        public int GetTimeToNextTip() => timeToNextTip;

        public int GetTimeToSkipTask() => timeToSkipTask;

        public int GetAvailableThemesCount() => availableThemesCount;   

        public TaskText GetCurrentTask() => ResourcesData.TaskTexts[CurrentLevelNumber - 1][currentTaskNumber - 1];

        public TipMessage[] GetCurrentTaskTips() => ResourcesData.Tips[currentTaskNumber - 1];

        public TipsData GetCurrentTaskTipsData() => AvailableTipsData[currentTaskNumber - 1];

        public string GetTests() => string.Copy(ResourcesData.Tests[currentTaskNumber - 1]); 

        public string GetNewTipText()
        {
            var currentTaskTipsData = GetCurrentTaskTipsData();
            var currentTaskTips = GetCurrentTaskTips();
            var tipNumber = currentTaskTips.Length - currentTaskTipsData.Amount;
            currentTaskTipsData.Amount--;
            currentTaskTipsData.IsShown = true;            
            return currentTaskTips[tipNumber].Tip;
        }

        public void PlayNextCutscene()
        {
            currentTaskNumber++;
            //PlayCutscene(currentTaskNumber);
        }

        private void OnEnable()
        {
            InitializeGameManager();
            CurrentLevelNumber = SceneManager.GetActiveScene().buildIndex;
            var currentLanguage = SaveManager.SaveData.Language;
            gameObject.GetComponent<UiLocalizationScript>().GetResourcesByCurrentLanguage(currentLanguage);
            for (var i = 0; i < ResourcesData.TaskTexts[CurrentLevelNumber - 1].Length; i++)
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
