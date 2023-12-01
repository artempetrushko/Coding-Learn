using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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

        public IEnumerator ChangeVisibility_COR(bool isVisible)
        {
            if (isVisible)
            {
                previousHandbookPageButton.transform.parent.gameObject.SetActive(false);
                SetContainerScrollbarDefaultValue(mainThemeButtonsContainer);
                yield return StartCoroutine(animator.ShowHandbook_COR());
            }
            else
            {
                yield return StartCoroutine(animator.HideHandbook_COR());
            }
        }

        public IEnumerator ShowSubThemeButtons_COR()
        {
            SetContainerScrollbarDefaultValue(subThemeButtonsContainer);
            yield return StartCoroutine(animator.GoToSubThemeButtons_COR());
            previousHandbookPageButton.gameObject.SetActive(true);
        }

        public IEnumerator ReturnToMainThemeButtons_COR()
        {
            SetContainerScrollbarDefaultValue(mainThemeButtonsContainer);
            yield return StartCoroutine(animator.ReturnToMainThemeButtons_COR());
            previousHandbookPageButton.gameObject.SetActive(false);
        }

        public void CreateThemeButtons(TrainingThemeType themeType, List<string> themes, Action<int> themeButtonPressedAction)
        {
            var buttonsContainer = themeType == TrainingThemeType.MainTheme ? mainThemeButtonsContainer : subThemeButtonsContainer;
            ClearButtonsContainer(buttonsContainer);
            for (var i = 1; i <= themes.Count; i++)
            {
                var themeNumber = i;
                var themeButton = Instantiate(themeButtonPrefab, buttonsContainer.transform);
                themeButton.SetInfo(themes[themeNumber - 1], () => themeButtonPressedAction(themeNumber));
            }
            SetContainerScrollbarDefaultValue(buttonsContainer);
        }

        private void ClearButtonsContainer(GameObject buttonsContainer)
        {
            for (var i = buttonsContainer.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(buttonsContainer.transform.GetChild(i).gameObject);
            }
            buttonsContainer.transform.DetachChildren();
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
