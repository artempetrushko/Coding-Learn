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
        private GameObject padView;

        public IEnumerator Show_COR()
        {
            taskDescriptionSectionView.transform.DOLocalMoveX(taskDescriptionSectionView.transform.localPosition.x + taskDescriptionSectionView.GetComponent<RectTransform>().sizeDelta.x, 1f);
            padView.transform.DOLocalMoveX(padView.transform.localPosition.x - padView.GetComponent<RectTransform>().sizeDelta.x - 20, 1f);
            yield return new WaitForSeconds(1f);
        }

        public IEnumerator Hide_COR()
        {
            taskDescriptionSectionView.transform.DOLocalMoveX(-835, 1f);
            padView.transform.DOLocalMoveX(1250, 1f);
            yield return new WaitForSeconds(1f);
        }
    }
}
