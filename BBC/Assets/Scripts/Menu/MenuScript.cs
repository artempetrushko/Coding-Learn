using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;
using System.Linq;

namespace Scripts
{
    public class MenuScript : MonoBehaviour
    {
        [Header("Авторизован ли пользователь")]
        [HideInInspector]
        public bool IsPlayerLoggedIn;

        [Header("UI-элементы меню")]
        [SerializeField] private GameObject StartBlackScreen;
        [SerializeField] private GameObject MainMenuButtons;
        [SerializeField] private Button ContinueButton;
        [Header("UI-элементы карты")]
        [SerializeField] private GameObject WorldMapPanel;
        private LoadLevel levelLoader;

        #region Переходы между экранами меню
        public void GoTo_Settings() => StartCoroutine(GoTo_Settings_COR());

        public void GoTo_Profile() => StartCoroutine(GoTo_Profile_COR());

        public void GoTo_Levels() => StartCoroutine(GoTo_Levels_COR());

        public void ReturnToMainMenuFrom_Settings() => StartCoroutine(ReturnToMainMenu_Settings_COR());

        public void ReturnToMainMenuFrom_Profile() => StartCoroutine(ReturnToMainMenu_Profile_COR());

        public void ReturnToMainMenuFrom_Levels() => StartCoroutine(ReturnToMainMenu_Levels_COR());

        public void Exit() => Application.Quit();
        #endregion

        public void Continue() => StartCoroutine(Continue_COR());

        public void Start_Level_Training() => StartCoroutine(Start_Level_COR(SceneManager.sceneCountInBuildSettings - 1));

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
            yield return StartCoroutine(PlayTimeline_COR(gameObject, "ShowSettings"));
        }

        private IEnumerator GoTo_Profile_COR()
        {
            yield return StartCoroutine(HideMainMenu_COR());
            gameObject.GetComponent<PlayerPanelBehaviour>().PlayerPanel.SetActive(false);
            StartCoroutine(gameObject.GetComponent<ProfileBehaviour>().ShowProfileScreen_COR());
        }

        private IEnumerator GoTo_Levels_COR()
        {
            yield return StartCoroutine(HideMainMenu_COR());
            StartCoroutine(ShowGlobalMap_COR());
        }

        private IEnumerator ReturnToMainMenu_Settings_COR()
        {
            yield return StartCoroutine(PlayTimeline_COR(gameObject, "HideSettings"));
            yield return StartCoroutine(ShowMainMenu_COR());
        }

        private IEnumerator ReturnToMainMenu_Profile_COR()
        {
            yield return StartCoroutine(gameObject.GetComponent<ProfileBehaviour>().HideProfileScreen_COR());
            gameObject.GetComponent<PlayerPanelBehaviour>().PlayerPanel.SetActive(true);
            yield return StartCoroutine(ShowMainMenu_COR());
        }

        private IEnumerator ReturnToMainMenu_Levels_COR()
        {
            yield return StartCoroutine(HideGlobalMap_COR());
            yield return StartCoroutine(ShowMainMenu_COR());
        }

        private IEnumerator ShowMainMenu_COR()
        {
            yield return StartCoroutine(PlayTimeline_COR(gameObject, "ShowMainMenu"));
        }

        private IEnumerator HideMainMenu_COR()
        {
            yield return StartCoroutine(PlayTimeline_COR(gameObject, "HideMainMenu"));
        }

        private IEnumerator Continue_COR()
        {
            yield return StartCoroutine(PlayTimeline_COR(gameObject, "HideMainMenu"));
            yield return StartCoroutine(PlayTimeline_COR(gameObject, "StartLevel"));
            StartCoroutine(levelLoader.LoadLevelAsync_COR(PlayerPrefs.GetInt("SceneIndex")));
        }

        private IEnumerator Start_Level_COR(int levelNumber)
        {
            PlayerPrefs.DeleteAll();
            yield return StartCoroutine(HideGlobalMap_COR());
            yield return StartCoroutine(PlayTimeline_COR(gameObject, "StartLevel"));
            StartCoroutine(levelLoader.LoadLevelAsync_COR(levelNumber));
        }

        private IEnumerator ShowGlobalMap_COR()
        {
            StartCoroutine(PlayTimeline_COR(gameObject, "ShowGlobalMap"));
            yield return new WaitForSeconds(0.75f);
            PlayMarkersAnimation("AppearMapMarker");           
        }

        private IEnumerator HideGlobalMap_COR()
        {
            PlayMarkersAnimation("EraseMapMarker");
            yield return StartCoroutine(PlayTimeline_COR(gameObject, "HideGlobalMap"));
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

        private IEnumerator PlayTimeline_COR(GameObject director, string playableAssetName = null)
        {
            var playableDirector = director.GetComponent<PlayableDirector>();
            if (playableAssetName != null)
                playableDirector.playableAsset = Resources.Load<PlayableAsset>("Timelines/Menu/" + playableAssetName);
            playableDirector.Play();
            yield return new WaitForSeconds((float)playableDirector.playableAsset.duration);
        }

        private void Start()
        {
            levelLoader = gameObject.GetComponent<LoadLevel>();
            if (!PlayerPrefs.HasKey("PositionX"))
                ContinueButton.gameObject.SetActive(false);
            StartCoroutine(PlayStartAnimation_COR());
            PlayMarkersAnimation("EraseMapMarker");
        }
    }
}