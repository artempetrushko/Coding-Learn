using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts
{
    public class ExitToMenuManager : MonoBehaviour
    {
        [SerializeField]
        private ExitToMenuSectionView exitToMenuSectionView;
        [SerializeField]
        private UnityEvent onBlackScreenShown;

        private bool isMenuEnabled;
        private bool isMenuAnimationPlaying;

        public void HideExitToMenuView() => StartCoroutine(HideExitToMenuView_COR());

        public void ExitToMenu() => StartCoroutine(ExitToMenu_COR());

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !isMenuAnimationPlaying)
            {
                StartCoroutine(PlayMenuAnimation_COR(isMenuEnabled ? HideExitToMenuView_COR : ShowExitToMenuView_COR));
            }
        }

        private IEnumerator ShowExitToMenuView_COR()
        {
            Time.timeScale = 0f;
            exitToMenuSectionView.gameObject.SetActive(true);
            yield return StartCoroutine(exitToMenuSectionView.ChangeVisibility_COR(true));
        }

        private IEnumerator HideExitToMenuView_COR()
        {
            yield return StartCoroutine(exitToMenuSectionView.ChangeVisibility_COR(false));
            exitToMenuSectionView.gameObject.SetActive(false);
            Time.timeScale = 1f;
        }

        private IEnumerator PlayMenuAnimation_COR(Func<IEnumerator> animation)
        {
            isMenuAnimationPlaying = true;
            yield return StartCoroutine(animation());
            isMenuAnimationPlaying = false;
        }

        private IEnumerator ExitToMenu_COR()
        {
            yield return StartCoroutine(exitToMenuSectionView.ShowBlackScreen_COR());
            onBlackScreenShown.Invoke();
        }
    }
}
