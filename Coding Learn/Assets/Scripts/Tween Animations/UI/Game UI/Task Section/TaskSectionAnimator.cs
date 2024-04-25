using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using UnityEngine;

namespace Scripts
{
    public class TaskSectionAnimator : MonoBehaviour
    {
        [SerializeField]
        private GameObject taskDescriptionSectionView;
        [SerializeField]
        private GameObject padSectionView;

        public async UniTask ChangeMainContentVisibilityAsync(bool isVisible)
        {
            var movementOffsetXSign = isVisible ? 1f : -1f;
            var padViewRightMargin = 20;

            taskDescriptionSectionView.transform.DOLocalMoveX(taskDescriptionSectionView.transform.localPosition.x + taskDescriptionSectionView.GetComponent<RectTransform>().sizeDelta.x * movementOffsetXSign, 1f);
            padSectionView.transform.DOLocalMoveX(padSectionView.transform.localPosition.x - (padSectionView.GetComponent<RectTransform>().sizeDelta.x + padViewRightMargin) * movementOffsetXSign, 1f);
            await UniTask.Delay(TimeSpan.FromSeconds(1));
        }
    }
}
