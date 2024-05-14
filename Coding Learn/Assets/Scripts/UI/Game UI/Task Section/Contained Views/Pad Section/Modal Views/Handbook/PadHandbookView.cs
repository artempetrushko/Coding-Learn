using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
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
        [SerializeField]
        private PadHandbookAnimator animator;

        public void SetVisibility(bool isVisible)
        {
            if (isVisible)
            {
                previousHandbookPageButton.gameObject.SetActive(false);
                SetContainerScrollbarDefaultValue(mainThemeButtonsContainer);
                animator.ChangeVisibilityAsync(true);
            }
            else
            {
                animator.ChangeVisibilityAsync(false);
            }
        }

        public async UniTask ShowSubThemeButtonsAsync()
        {
            SetContainerScrollbarDefaultValue(subThemeButtonsContainer);
            await animator.GoToSubThemeButtonsAsync();
            previousHandbookPageButton.gameObject.SetActive(true);
        }

        public async UniTask ReturnToMainThemeButtonsAsync()
        {
            previousHandbookPageButton.gameObject.SetActive(false);
            SetContainerScrollbarDefaultValue(mainThemeButtonsContainer);
            await animator.ReturnToMainThemeButtonsAsync();          
        }

        public void CreateThemeButtons<T>(T[] themes, Action<T> themeButtonPressedAction) where T : TrainingContent
        {
            var buttonsContainer = themes[0] switch
            {
                TrainingTheme => mainThemeButtonsContainer,
                TrainingSubTheme => subThemeButtonsContainer
            };
            ClearButtonsContainer(buttonsContainer);
            for (var i = 1; i <= themes.Length; i++)
            {
                var themeNumber = i;
                var themeButton = Instantiate(themeButtonPrefab, buttonsContainer.transform);
                themeButton.SetInfo(themes[themeNumber - 1].Title.GetLocalizedString(), () => themeButtonPressedAction(themes[themeNumber - 1]));
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
