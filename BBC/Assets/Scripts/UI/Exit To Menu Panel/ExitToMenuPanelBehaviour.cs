using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts
{
    public class ExitToMenuPanelBehaviour : MonoBehaviour
    {
        [Header("Панель выхода в меню")]
        public GameObject ExitToMenuPanel;
        public GameObject BlackScreen;

        private bool isPressed = false;

        public void ReturnToGame() => StartCoroutine(ReturnToGame_COR());

        public void ExitToMenu() => StartCoroutine(ExitToMenu_COR());

        private IEnumerator ReturnToGame_COR()
        {
            ExitToMenuPanel.GetComponent<Animator>().Play("ScaleExitToMenuPanelDown");
            yield return new WaitForSeconds(0.75f);
            isPressed = false;
        }

        private IEnumerator ExitToMenu_COR()
        {
            SaveManager.DeleteSavedDialogueData();
            ExitToMenuPanel.GetComponent<Animator>().Play("ScaleExitToMenuPanelDown");
            yield return new WaitForSeconds(0.75f);
            BlackScreen.SetActive(true);
            BlackScreen.GetComponent<Animator>().Play("AppearBlackScreen");
            yield return new WaitForSeconds(1.4f);
            SceneManager.LoadScene(0);
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.Escape) && !isPressed)
            {
                ExitToMenuPanel.GetComponent<Animator>().Play("ScaleExitToMenuPanelUp");
                isPressed = true;
            }
        }
    }
}
