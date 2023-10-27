using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    [RequireComponent(typeof(Animator))]
    public class LevelThumbnail : MonoBehaviour
    {
        private Animator animator;
        private Image image;

        public void Enable(Sprite thumbnail)
        {
            image.sprite = thumbnail;
            animator.Play("Show Level Thumbnail");
        }

        private void OnEnable()
        {
            animator = GetComponent<Animator>();
            image = GetComponent<Image>();
        }

        private void OnDestroy()
        {
            animator.Play("Hide Level Thumbnail");
        }
    }
}
