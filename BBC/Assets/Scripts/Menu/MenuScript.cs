using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unisave.Facades;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Backend;
using UnityEngine.Playables;

namespace Scripts
{
    public class MenuScript : MonoBehaviour
    {
        [Header("Авторизован ли пользователь")]
        [HideInInspector]
        public bool IsPlayerLoggedIn;

        [Header("Для анимаций и переходов")]
        [SerializeField] private GameObject MenuCamera;
        [SerializeField] private GameObject Robot;
        [SerializeField] private GameObject FireLight;
        [SerializeField] private GameObject BlackScreen;
        [SerializeField] private GameObject StartBlackScreen;
        [SerializeField] private GameObject MainMenuButtons;
        [SerializeField] private Button ContinueButton;
        private GameObject loginAndRegistrationPanel;
        private GameObject playerInfoPanel;
        private LoadLevel levelLoader;
        private GlobalMapBehaviour mapBehaviour;

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
            yield return StartCoroutine(ChangeMenuChapter_COR(1));
            for (var i = 1; i <= 2; i++)
            {
                GameObject.Find("SettingsBackground_Part_" + i).GetComponent<Animator>().Play("DrawChapter");
                yield return new WaitForSeconds(0.15f);
            }
            GameObject.Find("SettingsBackground_BackToMenu").GetComponent<Animator>().Play("DrawChapter");
            GameObject.Find("SettingsButtons").GetComponent<Animator>().Play("MoveSettingsDown");
            GameObject.Find("BackToMainMenuButton_Settings").GetComponent<Animator>().Play("MoveBackToMenuButtonUp");
        }

        private IEnumerator GoTo_Profile_COR()
        {
            yield return StartCoroutine(ChangeMenuChapter_COR(2));
            gameObject.GetComponent<PlayerPanelBehaviour>().PlayerPanel.SetActive(false);
            StartCoroutine(gameObject.GetComponent<ProfileBehaviour>().ShowProfileScreen_COR());
        }

        private IEnumerator GoTo_Levels_COR()
        {
            yield return StartCoroutine(ChangeMenuChapter_COR(3));
            StartCoroutine(mapBehaviour.ShowGlobalMap_COR());
        }

        private IEnumerator ReturnToMainMenu_Settings_COR()
        {
            GameObject.Find("SettingsBackground_BackToMenu").GetComponent<Animator>().Play("EraseChapter");
            GameObject.Find("SettingsButtons").GetComponent<Animator>().Play("MoveSettingsUp");
            GameObject.Find("BackToMainMenuButton_Settings").GetComponent<Animator>().Play("MoveBackToMenuButtonDown");
            yield return new WaitForSeconds(0.5f);
            for (var i = 2; i >= 1; i--)
            {
                GameObject.Find("SettingsBackground_Part_" + i).GetComponent<Animator>().Play("EraseChapter");
                yield return new WaitForSeconds(0.15f);
            }
            yield return StartCoroutine(ReturnToMainMenu_COR(1));
        }

        private IEnumerator ReturnToMainMenu_Profile_COR()
        {
            yield return StartCoroutine(gameObject.GetComponent<ProfileBehaviour>().HideProfileScreen_COR());
            gameObject.GetComponent<PlayerPanelBehaviour>().PlayerPanel.SetActive(true);
            yield return StartCoroutine(ReturnToMainMenu_COR(2));
        }

        private IEnumerator ReturnToMainMenu_Levels_COR()
        {
            yield return StartCoroutine(mapBehaviour.HideGlobalMap_COR());
            yield return StartCoroutine(ReturnToMainMenu_COR(3));
        }

        private IEnumerator ReturnToMainMenu_COR(int cameraNumber)
        {
            MenuCamera.GetComponent<Animator>().Play("MoveBackCamera_" + cameraNumber);
            yield return new WaitForSeconds(2f);
            for (var i = 1; i <= 5; i++)
            {
                GameObject.Find("MainMenuBackground_Part_" + i).GetComponent<Animator>().Play("DrawMainMenu");
                yield return new WaitForSeconds(0.15f);
            }
            GameObject.Find("Content").GetComponent<Animator>().Play("MoveMainMenuDown");
            if (IsPlayerLoggedIn)
                playerInfoPanel.GetComponent<Animator>().Play("MovePlayerInfoPanel_Left");
            else loginAndRegistrationPanel.GetComponent<Animator>().Play("MoveLoginAndRegistrationPanel_Left");
        }

        private IEnumerator ChangeMenuChapter_COR(int cameraNumber)
        {
            GameObject.Find("Content").GetComponent<Animator>().Play("MoveMainMenuUp");
            if (IsPlayerLoggedIn)
                playerInfoPanel.GetComponent<Animator>().Play("MovePlayerInfoPanel_Right");
            else loginAndRegistrationPanel.GetComponent<Animator>().Play("MoveLoginAndRegistrationPanel_Right");
            yield return new WaitForSeconds(0.5f);
            for (var i = 5; i >= 1; i--)
            {
                GameObject.Find("MainMenuBackground_Part_" + i).GetComponent<Animator>().Play("EraseMainMenu");
                yield return new WaitForSeconds(0.15f);
            }
            MenuCamera.GetComponent<Animator>().Play("MoveCamera_" + cameraNumber);
            yield return new WaitForSeconds(2f);
        }

        private IEnumerator Continue_COR()
        {
            GameObject.Find("Content").GetComponent<Animator>().Play("MoveMainMenuUp");
            yield return new WaitForSeconds(0.5f);
            for (var i = 5; i >= 1; i--)
            {
                GameObject.Find("MainMenuBackground_Part_" + i).GetComponent<Animator>().Play("EraseMainMenu");
                yield return new WaitForSeconds(0.15f);
            }
            Robot.GetComponent<Animator>().Play("Walk_MainMenu");
            yield return new WaitForSeconds(5f);
            BlackScreen.GetComponent<Animator>().Play("AppearBlackScreen");
            yield return new WaitForSeconds(1.4f);
            StartCoroutine(levelLoader.LoadLevelAsync_COR(PlayerPrefs.GetInt("SceneIndex")));
        }

        private IEnumerator Start_Level_COR(int levelNumber)
        {
            PlayerPrefs.DeleteAll();
            yield return StartCoroutine(mapBehaviour.HideGlobalMap_COR());
            MenuCamera.GetComponent<Animator>().Play("MoveBackCamera_3");
            yield return new WaitForSeconds(2f);
            Robot.GetComponent<Animator>().Play("Walk_MainMenu");
            yield return new WaitForSeconds(5f);
            BlackScreen.GetComponent<Animator>().Play("AppearBlackScreen");
            yield return new WaitForSeconds(1.4f);
            StartCoroutine(levelLoader.LoadLevelAsync_COR(levelNumber));
        }

        /*private async void Awake()
        {
            ChangeMainMenuButtonsAvailability(false);
            var response = await OnFacet<PlayerDataFacet>.CallAsync<PlayerEntity>(
                nameof(PlayerDataFacet.GetAuthorizedPlayer));
            IsPlayerLoggedIn = response != null;
            if (IsPlayerLoggedIn)
                gameObject.GetComponent<PlayerPanelBehaviour>().ShowPlayerInfo_PlayerAlreadyLoggedIn(response.Nickname, response.TotalScore);  
        }*/

        private IEnumerator PlayStartAnimation_COR()
        {
            StartBlackScreen.SetActive(true);
            yield return new WaitForSeconds(1f);
            var director = StartBlackScreen.GetComponent<PlayableDirector>();
            director.Play();
            yield return new WaitForSeconds((float)director.playableAsset.duration);
            StartBlackScreen.SetActive(false);
        }

        private void Start()
        {
            mapBehaviour = gameObject.GetComponent<GlobalMapBehaviour>();
            levelLoader = gameObject.GetComponent<LoadLevel>();
            loginAndRegistrationPanel = gameObject.GetComponent<PlayerPanelBehaviour>().LoginAndRegistrationPanel;
            playerInfoPanel = gameObject.GetComponent<PlayerPanelBehaviour>().PlayerInfoPanel;
            FireLight.GetComponent<Animator>().Play("Fire");
            if (!PlayerPrefs.HasKey("PositionX"))
                ContinueButton.gameObject.SetActive(false);
            StartCoroutine(PlayStartAnimation_COR());
        }
    }
}