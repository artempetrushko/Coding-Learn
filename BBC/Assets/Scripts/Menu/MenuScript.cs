using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;
using UnityEngine.Events;

namespace Scripts
{
    public class MenuScript : MonoBehaviour
    {
        [Header("UI-элементы меню")]
        [SerializeField] private GameObject StartBlackScreen;
        [SerializeField] private GameObject MainMenuButtons;
        [SerializeField] private Button ContinueButton;
        [Header("UI-элементы карты")]
        [SerializeField] private GameObject WorldMapPanel;
        [SerializeField] private GameObject levelsPanel;
        [SerializeField] private UnityEvent<int> onLevelsPanelCalled;
        [SerializeField] private UnityEvent onPlayButtonPressed;
        private LoadLevel levelLoader;
        private const string timelinesRootPath = "Timelines/Menu/";

        #region Переходы между экранами меню
        public void GoTo_Settings() => StartCoroutine(GoTo_Settings_COR());

        public void GoTo_Levels() => StartCoroutine(GoTo_Levels_COR());

        public void ReturnToMainMenuFrom_Settings() => StartCoroutine(ReturnToMainMenu_Settings_COR());

        public void ReturnToMainMenuFrom_Levels() => StartCoroutine(ReturnToMainMenu_Levels_COR());

        public void Exit() => Application.Quit();
        #endregion

        public void Continue() => StartCoroutine(Continue_COR());

        public void StartLevel() => StartCoroutine(Start_Level_COR(0));

        public void Start_Level(int levelNumber) => StartCoroutine(Start_Level_COR(levelNumber));

        public void ChangeMainMenuButtonsAvailability(bool areAvailable)
        {
            var textColor = areAvailable ? Color.white : Color.gray;
            for (var i = 0; i < MainMenuButtons.transform.childCount - 1; i++)
            {
                var button = MainMenuButtons.transform.GetChild(i).GetComponent<Button>();
                button.interactable = areAvailable;
                button.transform.GetChild(0).GetComponent<Text>().color = textColor;
            }
        }

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
            //StartCoroutine(ShowGlobalMap_COR());           
            yield return StartCoroutine(PlayTimeline_COR("ShowLevelsPanel"));
        }

        private IEnumerator ReturnToMainMenu_Settings_COR()
        {
            yield return StartCoroutine(PlayTimeline_COR("HideSettings"));
            yield return StartCoroutine(ShowMainMenu_COR());
        }

        private IEnumerator ReturnToMainMenu_Levels_COR()
        {
            //yield return StartCoroutine(HideGlobalMap_COR());
            yield return StartCoroutine(PlayTimeline_COR("HideLevelsPanel"));
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

        private IEnumerator Continue_COR()
        {
            yield return StartCoroutine(PlayTimeline_COR("HideMainMenu"));
            yield return StartCoroutine(PlayTimeline_COR("StartLevel"));
            StartCoroutine(levelLoader.LoadLevelAsync_COR(PlayerPrefs.GetInt("SceneIndexToResume")));
        }

        private IEnumerator Start_Level_COR(int levelNumber)
        {
            //yield return StartCoroutine(HideGlobalMap_COR());
            yield return StartCoroutine(PlayTimeline_COR("ShowLoadScreen"));
            onPlayButtonPressed.Invoke();
            //StartCoroutine(levelLoader.LoadLevelAsync_COR(levelNumber));
        }

        private IEnumerator ShowGlobalMap_COR()
        {
            StartCoroutine(PlayTimeline_COR("ShowGlobalMap"));
            yield return new WaitForSeconds(0.75f);
            PlayMarkersAnimation("AppearMapMarker");           
        }

        private IEnumerator HideGlobalMap_COR()
        {
            PlayMarkersAnimation("EraseMapMarker");
            yield return StartCoroutine(PlayTimeline_COR("HideGlobalMap"));
        }

        private void ChangeLevelDataToDefault()
        {
            var defaultLevelNumber = PlayerPrefs.HasKey("LevelNumberToResume") ? PlayerPrefs.GetInt("LevelNumberToResume") : 1;
            onLevelsPanelCalled.Invoke(defaultLevelNumber);
        }

        private void PlayMarkersAnimation(string animationName)
        {
            var markersHolder = WorldMapPanel.transform.GetChild(0).GetChild(0);
            for (var i = 0; i < markersHolder.childCount; i++)
                markersHolder.GetChild(i).GetComponent<Animator>().Play(animationName);
        }

        private IEnumerator PlayStartAnimation_COR()
        {
            StartBlackScreen.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            yield return StartCoroutine(PlayTimeline_COR(StartBlackScreen));
            StartBlackScreen.SetActive(false);
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

        private IEnumerator PlayAnimation_COR(GameObject animatorHolder, string animationName)
        {
            var animator = animatorHolder.GetComponent<Animator>();
            var clip = animator.runtimeAnimatorController.animationClips.Where(x => x.name == animationName).First();
            animator.Play(clip.name);
            yield return new WaitForSeconds(clip.length);
        }

        private void Start()
        {
            levelLoader = gameObject.GetComponent<LoadLevel>();
            StartCoroutine(PlayStartAnimation_COR());
            PlayMarkersAnimation("EraseMapMarker");
        }
    }
}