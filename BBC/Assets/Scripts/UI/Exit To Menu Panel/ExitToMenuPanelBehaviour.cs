using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            yield return StartCoroutine(PlayAnimation_COR(ExitToMenuPanel, "HideExitToMenuPanel"));
            isPressed = false;
        }

        private IEnumerator ExitToMenu_COR()
        {
            yield return StartCoroutine(PlayAnimation_COR(ExitToMenuPanel, "HideExitToMenuPanel"));
            BlackScreen.SetActive(true);
            yield return StartCoroutine(PlayAnimation_COR(BlackScreen, "AppearBlackScreen"));
            SceneManager.LoadScene(0);
        }

        private IEnumerator PlayAnimation_COR(GameObject animatorHolder, string animationName)
        {
            var animator = animatorHolder.GetComponent<Animator>();
            var clip = animator.runtimeAnimatorController.animationClips.Where(x => x.name == animationName).First();
            animator.Play(clip.name);
            yield return new WaitForSeconds(clip.length);
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.Escape) && !isPressed)
            {
                ExitToMenuPanel.GetComponent<Animator>().Play("AppearExitToMenuPanel");
                isPressed = true;
            }
        }
    }
}
