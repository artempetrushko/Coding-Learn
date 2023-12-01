using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class LevelThumbnail : MonoBehaviour
    {
        [SerializeField]
        private Image image;

        public Image Image => image;

        public void SetContent(Sprite content)
        {
            image.sprite = content;
        }
    }
}
