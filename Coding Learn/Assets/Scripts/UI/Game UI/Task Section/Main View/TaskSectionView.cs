using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts
{
    public class TaskSectionView : MonoBehaviour
    {
        [SerializeField]
        private TaskSectionAnimator animator;
        [Space, SerializeField]
        private UnityEvent<bool> onMainContentEnablingStarted;
        [SerializeField]
        private UnityEvent<bool> onMainContentDisablingFinished;

        public IEnumerator ChangeMainContentVisibility_COR(bool isVisible)
        {
            if (isVisible)
            {
                onMainContentEnablingStarted.Invoke(isVisible);
            }
            yield return StartCoroutine(animator.ChangeMainContentVisibility_COR(isVisible));
            if (!isVisible)
            {
                onMainContentDisablingFinished.Invoke(isVisible);
            }
        }
    }
}
