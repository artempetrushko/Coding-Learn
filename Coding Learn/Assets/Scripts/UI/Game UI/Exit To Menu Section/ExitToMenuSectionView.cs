using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scripts
{
    public class ExitToMenuSectionView : MonoBehaviour
    {
        [SerializeField]
        private ExitToMenuSectionAnimator animator;
        
        public IEnumerator ChangeVisibility_COR(bool isVisible)
        {
            yield return StartCoroutine(animator.ChangeContentVisibility_COR(isVisible));
        }

        public IEnumerator ShowBlackScreen_COR()
        {
            yield return StartCoroutine(animator.ShowBlackScreen_COR());
        }
    }
}
