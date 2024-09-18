using UnityEngine;
using UnityEngine.UI;

namespace GameLogic
{
    public class ExitMenuView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Button _confirmButton;
        [SerializeField] private Button _cancelButton;
        [SerializeField] private Image _blackScreen;

        public CanvasGroup CanvasGroup => _canvasGroup;
        public Button ConfirmButton => _confirmButton;
        public Button CancelButton => _cancelButton;
        public Image BlackScreen => _blackScreen;

        public void SetActive(bool isActive) => gameObject.SetActive(isActive);
    }
}
