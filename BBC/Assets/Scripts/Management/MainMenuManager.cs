using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Events;

namespace Scripts
{
    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField]
        private MainMenuSection mainMenuSection;
        [SerializeField]
        private List<MainMenuButtonData> buttonDatas = new List<MainMenuButtonData>();
        [Space]
        [SerializeField] private GameObject startBlackScreen;
        [SerializeField] private GameObject levelsPanel;
        [Space]
        [SerializeField] private UnityEvent<int> onLevelsPanelCalled;
        [SerializeField] private UnityEvent onPlayButtonPressed;
        [SerializeField] private UnityEvent onStatsPanelHid;

        private const string timelinesRootPath = "Timelines/Menu/";

        #region Переходы между экранами меню
        public void GoTo_Settings() => StartCoroutine(GoTo_Settings_COR());

        public void GoTo_Levels() => StartCoroutine(GoTo_Levels_COR());

        public void GoTo_Stats() => StartCoroutine(GoTo_Stats_COR());

        public void ReturnToMainMenuFrom_Settings() => StartCoroutine(ReturnToMainMenu_Settings_COR());

        public void ReturnToMainMenuFrom_Levels() => StartCoroutine(ReturnToMainMenu_Levels_COR());

        public void ReturnToMainMenuFrom_Stats() => StartCoroutine(ReturnToMainMenu_Stats_COR());

        public void Exit() => Application.Quit();
        #endregion

        public void StartLevel() => StartCoroutine(Start_Level_COR());

        private IEnumerator GoTo_Settings_COR()
        {
            yield return StartCoroutine(HideMainMenu_COR());
            yield return StartCoroutine(PlayTimeline_COR("ShowSettings"));
        }

        private IEnumerator GoTo_Levels_COR()
        {
            levelsPanel.SetActive(true);
            ChangeLevelDataToDefault();
            yield return StartCoroutine(HideMainMenu_COR());         
            yield return StartCoroutine(PlayTimeline_COR("ShowLevelsPanel"));
        }

        private IEnumerator GoTo_Stats_COR()
        {
            yield return StartCoroutine(HideMainMenu_COR());
            yield return StartCoroutine(PlayTimeline_COR("ShowLevelStatsPanel"));
        }

        private IEnumerator ReturnToMainMenu_Settings_COR()
        {
            yield return StartCoroutine(PlayTimeline_COR("HideSettings"));
            yield return StartCoroutine(ShowMainMenu_COR());
        }

        private IEnumerator ReturnToMainMenu_Levels_COR()
        {
            yield return StartCoroutine(PlayTimeline_COR("HideLevelsPanel"));
            yield return StartCoroutine(ShowMainMenu_COR());
        }

        private IEnumerator ReturnToMainMenu_Stats_COR()
        {
            yield return StartCoroutine(PlayTimeline_COR("HideLevelStatsPanel"));
            onStatsPanelHid.Invoke();
            yield return StartCoroutine(ShowMainMenu_COR());
        }

        private IEnumerator ShowMainMenu_COR()
        {
            yield return StartCoroutine(PlayTimeline_COR("ShowMainMenu"));
        }

        private IEnumerator HideMainMenu_COR()
        {
            yield return StartCoroutine(PlayTimeline_COR("HideMainMenu"));
        }

        private IEnumerator Start_Level_COR()
        {
            yield return StartCoroutine(PlayTimeline_COR("ShowLoadScreen"));
            onPlayButtonPressed.Invoke();
        }

        private void ChangeLevelDataToDefault()
        {
            var defaultLevelNumber = Math.Max(1, SaveManager.SaveData.LevelNumberToResume);
            onLevelsPanelCalled.Invoke(defaultLevelNumber);
        }

        private IEnumerator PlayStartAnimation_COR()
        {
            startBlackScreen.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            yield return StartCoroutine(PlayTimeline_COR(startBlackScreen));
            startBlackScreen.SetActive(false);
        }

        private IEnumerator PlayTimeline_COR(string playableAssetName = null)
        {
            yield return StartCoroutine(PlayTimeline_COR(gameObject, playableAssetName));
        }

        private IEnumerator PlayTimeline_COR(GameObject director, string playableAssetName = null)
        {
            var playableDirector = director.GetComponent<PlayableDirector>();
            if (playableAssetName != null)
                playableDirector.playableAsset = Resources.Load<PlayableAsset>(timelinesRootPath + playableAssetName);
            playableDirector.Play();
            yield return new WaitForSeconds((float)playableDirector.playableAsset.duration);
        }

        private void Start()
        {
            mainMenuSection.CreateButtons(buttonDatas);
            StartCoroutine(PlayStartAnimation_COR());
        }
    }
}