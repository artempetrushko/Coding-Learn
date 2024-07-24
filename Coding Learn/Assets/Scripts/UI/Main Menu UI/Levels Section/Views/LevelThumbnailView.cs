using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class LevelThumbnailView : MonoBehaviour
    {
        [SerializeField] private Image _thumbnail;

        public void SetThumbnail(Sprite thumbnail)
        {
            _thumbnail.sprite = thumbnail;
        }
    }
}
