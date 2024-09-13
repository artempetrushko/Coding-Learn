using UnityEngine;
using UnityEngine.UI;

namespace UI.Game
{
    public class HandbookView : MonoBehaviour
    {
        [SerializeField] private Button _previousHandbookPageButton;
        [SerializeField] private GameObject _mainThemeButtonsContainer;
        [SerializeField] private GameObject _subThemeButtonsContainer;
        [SerializeField] private CanvasGroup _canvasGroup;

        public GameObject MainThemeButtonsContainer => _mainThemeButtonsContainer;
        public GameObject SubThemeButtonsContainer => _subThemeButtonsContainer;
        public CanvasGroup CanvasGroup => _canvasGroup;

        public void SetPreviousHandbookPageButtonActive(bool isActive) => _previousHandbookPageButton.gameObject.SetActive(isActive);

        public void SetMainThemeButtonsContainerScrollbarValue(float value) => SetContainerScrollbarValue(_mainThemeButtonsContainer, value);

        public void SetSubThemeButtonsContainerScrollbarValue(float value) => SetContainerScrollbarValue(_subThemeButtonsContainer, value);

        private void SetContainerScrollbarValue(GameObject buttonsContainer, float value)
        {
            var scrollbar = buttonsContainer.GetComponentInChildren<Scrollbar>();
            if (scrollbar != null)
            {
                scrollbar.value = value;
            }
        }
    }
}
