using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Scripts
{
    public class LevelDescriptionView : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text descriptionText;

        private Animator animator;

        public void SetInfo(string levelDescription)
        {
            descriptionText.text = levelDescription;
        }

        private void OnEnable()
        {
            animator = GetComponent<Animator>();
            animator.Play("Show Level Description View");
        }

        private void OnDestroy()
        {
            animator.Play("Hide Level Description View");
        }
    }
}
