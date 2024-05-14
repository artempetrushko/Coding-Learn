using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class LevelThumbnail : MonoBehaviour
    {
        [field: SerializeField]
        public Image Image { get; private set; }

        public void SetContent(Sprite content)
        {
            Image.sprite = content;
        }
    }
}
