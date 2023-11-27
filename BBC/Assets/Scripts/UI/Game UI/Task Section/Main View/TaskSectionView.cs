using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    public class TaskSectionView : MonoBehaviour
    {
        [SerializeField]
        private TaskSectionAnimator animator;

        public IEnumerator ChangeMainContentVisibility_COR(bool isVisible)
        {
            yield return StartCoroutine(animator.ChangeMainContentVisibility_COR(isVisible));
        }
    }
}
