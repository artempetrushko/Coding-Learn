using UnityEngine;
using UnityEngine.UI;

namespace UI.MainMenu
{
    public class LevelThumbnailView : MonoBehaviour
    {
        [SerializeField] private Image _thumbnail;
        [SerializeField] private CanvasGroup _canvasGroup;

        public Image Thumbnail => _thumbnail;
        public CanvasGroup CanvasGroup => _canvasGroup;

        public void SetActive(bool isActive) => gameObject.SetActive(isActive);
    }
}
