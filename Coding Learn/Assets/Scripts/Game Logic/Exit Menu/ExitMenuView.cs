using UnityEngine;
using UnityEngine.UI;

namespace UI.Game
{
    public class ExitMenuView : MonoBehaviour
    {
        [SerializeField] private Image _background;
        [SerializeField] private CanvasGroup _contentCanvasGroup;
        [SerializeField] private Image _blackScreen;

        public Image Background => _background;
        public CanvasGroup ContentCanvasGroup => _contentCanvasGroup;
        public Image BlackScreen => _blackScreen;

        public void SetActive(bool isActive) => gameObject.SetActive(isActive);
    }
}
