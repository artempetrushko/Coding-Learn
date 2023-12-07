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
        [Space, SerializeField]
        private LevelDescriptionAnimator animator;

        public void SetInfo(string levelDescription)
        {
            descriptionText.text = levelDescription;
        }

        private void OnEnable()
        {
            StartCoroutine(animator.ChangeVisibility_COR(true));
        }
    }
}
