using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class ExitToMenuSectionView : MonoBehaviour
    {
        [SerializeField] private Image _background;
        [SerializeField] private CanvasGroup _contentCanvasGroup;
        [SerializeField] private Image _blackScreen;

        public Image Background => _background;
        public CanvasGroup ContentCanvasGroup => _contentCanvasGroup;
        public Image BlackScreen => _blackScreen;
    }
}
