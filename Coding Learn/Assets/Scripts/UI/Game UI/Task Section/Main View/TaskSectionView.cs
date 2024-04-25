using Cysharp.Threading.Tasks;
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

        public async UniTask ChangeMainContentVisibilityAsync(bool isVisible)
        {
            if (isVisible)
            {
                onMainContentEnablingStarted.Invoke(isVisible);
            }
            await animator.ChangeMainContentVisibilityAsync(isVisible);
            if (!isVisible)
            {
                onMainContentDisablingFinished.Invoke(isVisible);
            }
        }
    }
}
