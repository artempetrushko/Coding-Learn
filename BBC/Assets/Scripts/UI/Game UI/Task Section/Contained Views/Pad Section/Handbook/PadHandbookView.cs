using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Scripts
{
    public enum TrainingThemeType
    {
        MainTheme,
        SubTheme
    }

    public class PadHandbookView : MonoBehaviour
    {
        [SerializeField]
        private Button previousHandbookPageButton;
        [Space, SerializeField]
        private HandbookThemeButton themeButtonPrefab;
        [SerializeField]
        private GameObject mainThemeButtonsContainer;
        [SerializeField]
        private GameObject subThemeButtonsContainer;
        [Space, SerializeField]
        private PadHandbookAnimator animator;

        public void OpenHandbook() => StartCoroutine(OpenHandbook_COR());

        public void CloseHandbook() => StartCoroutine(CloseHandbook_COR());

        public void ShowSubThemeButtons() => StartCoroutine(ShowSubThemeButtons_COR());

        public void ReturnToMainThemeButtons() => StartCoroutine(ReturnToMainThemeButtons_COR());

        public void CreateThemeButtons(TrainingThemeType themeType, List<string> themes, Action<int> themeButtonPressedAction)
        {
            var buttonsContainer = themeType == TrainingThemeType.MainTheme ? mainThemeButtonsContainer : subThemeButtonsContainer;
            for (var i = 1; i <= themes.Count; i++)
            {
                var themeNumber = i;
                var themeButton = Instantiate(themeButtonPrefab, buttonsContainer.transform);
                themeButton.SetInfo(themes[themeNumber - 1], () => themeButtonPressedAction(themeNumber));
            }
            SetContainerScrollbarDefaultValue(buttonsContainer);
        }

        private IEnumerator OpenHandbook_COR()
        {
            previousHandbookPageButton.transform.parent.gameObject.SetActive(false);
            SetContainerScrollbarDefaultValue(mainThemeButtonsContainer);
            yield return StartCoroutine(animator.ShowHandbook_COR());
        }

        private IEnumerator CloseHandbook_COR()
        {
            yield return StartCoroutine(animator.HideHandbook_COR());
        }

        private IEnumerator ShowSubThemeButtons_COR()
        {
            SetContainerScrollbarDefaultValue(subThemeButtonsContainer);
            yield return StartCoroutine(animator.GoToSubThemeButtons_COR());
            previousHandbookPageButton.gameObject.SetActive(true);
        }

        private IEnumerator ReturnToMainThemeButtons_COR()
        {
            SetContainerScrollbarDefaultValue(mainThemeButtonsContainer);
            yield return StartCoroutine(animator.ReturnToMainThemeButtons_COR());
            previousHandbookPageButton.gameObject.SetActive(false);
        }

        private void SetContainerScrollbarDefaultValue(GameObject buttonsContainer)
        {
            var scrollbar = buttonsContainer.GetComponentInChildren<Scrollbar>();
            if (scrollbar != null)
            {
                scrollbar.value = 1;
            }
        }
    }
}
