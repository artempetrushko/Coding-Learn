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

    [RequireComponent(typeof(Animator))]
    public class PadHandbookView : MonoBehaviour
    {
        [SerializeField]
        private Button previousHandbookPageButton;
        [Space, SerializeField]
        private HandbookThemeButton themeButtonPrefab;
        [SerializeField]
        private GameObject themeButtonsContainer;
        [SerializeField]
        private GameObject subThemeButtonsContainer; 
        [Space, SerializeField]
        private UnityEvent<int, int> onSubThemeButtonPressed;

        private Animator animator;

        public void OpenHandbook() => StartCoroutine(OpenHandbook_COR());

        public void CloseHandbook() => StartCoroutine(CloseHandbook_COR());

        public void ShowSubThemeButtons() => StartCoroutine(ShowSubThemeButtons_COR());

        public void ReturnToPreviousPage() => StartCoroutine(ReturnToPreviousPage_COR());

        public void CreateThemeButtons(TrainingThemeType themeType, List<string> themes, Action<int> themeButtonPressedAction)
        {
            for (var i = 1; i <= themes.Count; i++)
            {
                var themeNumber = i;
                var themeButton = Instantiate(themeButtonPrefab, themeType == TrainingThemeType.MainTheme 
                                                                    ? themeButtonsContainer.transform
                                                                    : subThemeButtonsContainer.transform);
                themeButton.SetInfo(themes[themeNumber - 1], () => themeButtonPressedAction(themeNumber));
            }
        }

        private void OnEnable()
        {
            animator = GetComponent<Animator>();
        }

        private IEnumerator OpenHandbook_COR()
        {
            previousHandbookPageButton.transform.parent.gameObject.SetActive(false);
            if (themeButtonsContainer.GetComponentInChildren<Scrollbar>() != null)
                themeButtonsContainer.GetComponentInChildren<Scrollbar>().value = 1;
            yield return StartCoroutine(PlayAnimation_COR("Show Handbook"));
            yield return StartCoroutine(PlayAnimation_COR("Show Main Theme Buttons"));
        }

        private IEnumerator CloseHandbook_COR()
        {
            yield return StartCoroutine(PlayAnimation_COR("Hide Handbook"));
        }

        private IEnumerator ShowSubThemeButtons_COR()
        {
            yield return StartCoroutine(PlayAnimation_COR("Hide Main Theme Buttons (Middle To Left)"));
            previousHandbookPageButton.gameObject.SetActive(true);
            yield return StartCoroutine(PlayAnimation_COR("Show SubTheme Buttons"));
        }

        private IEnumerator ReturnToPreviousPage_COR()
        {
            yield return StartCoroutine(PlayAnimation_COR("Hide SubTheme Buttons"));
            yield return StartCoroutine(PlayAnimation_COR("Hide Main Theme Buttons"));
            previousHandbookPageButton.gameObject.SetActive(false);
        }

        private IEnumerator PlayAnimation_COR(string animationName)
        {
            var clip = animator.runtimeAnimatorController.animationClips.Where(x => x.name == animationName).First();
            animator.Play(clip.name);
            yield return new WaitForSeconds(clip.length);
        }
    }
}
