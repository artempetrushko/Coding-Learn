using UnityEngine;
using UnityEngine.UI;

namespace GameLogic
{
    public class HandbookView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private CanvasGroup _mainThemeButtonsContainer;
        [SerializeField] private CanvasGroup _subThemeButtonsContainer;
        [SerializeField] private Button _returnToMainThemesButton;
        [SerializeField] private Button _closeViewButton;

        public CanvasGroup CanvasGroup => _canvasGroup;
        public CanvasGroup MainThemeButtonsContainer => _mainThemeButtonsContainer;
        public CanvasGroup SubThemeButtonsContainer => _subThemeButtonsContainer;
        public Button ReturnToMainThemesButton => _returnToMainThemesButton;
        public Button CloseViewButton => _closeViewButton;

        public void SetActive(bool isActive) => gameObject.SetActive(isActive);

        public void SetMainThemeButtonsContainerScrollbarValue(float value) => SetContainerScrollbarValue(_mainThemeButtonsContainer.gameObject, value);

        public void SetSubThemeButtonsContainerScrollbarValue(float value) => SetContainerScrollbarValue(_subThemeButtonsContainer.gameObject, value);

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
