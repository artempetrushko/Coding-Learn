using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    public class TaskSectionAnimator : MonoBehaviour
    {
        [SerializeField]
        private GameObject taskDescriptionSectionView;
        [SerializeField]
        private GameObject padSectionView;

        public IEnumerator ChangeMainContentVisibility_COR(bool isVisible)
        {
            var movementOffsetXSign = isVisible ? 1f : -1f;
            var padViewRightMargin = 20;

            taskDescriptionSectionView.transform.DOLocalMoveX(taskDescriptionSectionView.transform.localPosition.x + taskDescriptionSectionView.GetComponent<RectTransform>().sizeDelta.x * movementOffsetXSign, 1f);
            padSectionView.transform.DOLocalMoveX(padSectionView.transform.localPosition.x - (padSectionView.GetComponent<RectTransform>().sizeDelta.x + padViewRightMargin) * movementOffsetXSign, 1f);
            yield return new WaitForSeconds(1f);
        }
    }
}
