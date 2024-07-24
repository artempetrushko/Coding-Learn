using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts
{
    public class TaskSectionView : MonoBehaviour
    {
        [Space, SerializeField]
        private UnityEvent<bool> onMainContentEnablingStarted;
        [SerializeField]
        private UnityEvent<bool> onMainContentDisablingFinished;

        [SerializeField]
        private GameObject taskDescriptionSectionView;
        [SerializeField]
        private GameObject padSectionView;

        public async UniTask ChangeMainContentVisibilityAsync(bool isVisible)
        {
            if (isVisible)
            {
                onMainContentEnablingStarted.Invoke(isVisible);
            }

            var movementOffsetXSign = isVisible ? 1f : -1f;
            var padViewRightMargin = 20;

            taskDescriptionSectionView.transform.DOLocalMoveX(taskDescriptionSectionView.transform.localPosition.x + taskDescriptionSectionView.GetComponent<RectTransform>().sizeDelta.x * movementOffsetXSign, 1f);
            padSectionView.transform.DOLocalMoveX(padSectionView.transform.localPosition.x - (padSectionView.GetComponent<RectTransform>().sizeDelta.x + padViewRightMargin) * movementOffsetXSign, 1f);
            await UniTask.WaitForSeconds(1);

            if (!isVisible)
            {
                onMainContentDisablingFinished.Invoke(isVisible);
            }
        }
    }
}
