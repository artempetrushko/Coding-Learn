using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class ErrorsSectionView : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text errorsText;
        [SerializeField]
        private Scrollbar scrollbar;
        [Space, SerializeField]
        private ErrorsSectionAnimator animator;

        private bool isVisible = false;

        public IEnumerator ChangeVisibility_COR(bool isVisible)
        {
            if (this.isVisible != isVisible)
            {
                this.isVisible = isVisible;
                yield return StartCoroutine(animator.ChangeVisibility_COR(isVisible));
            }          
        }

        public void ToggleVisibility()
        {
            isVisible = !isVisible;
            StartCoroutine(animator.ChangeVisibility_COR(isVisible));
        }

        public void SetContent(string errorsMessage)
        {
            errorsText.text = errorsMessage;
            scrollbar.value = 1;
        }
    }
}
