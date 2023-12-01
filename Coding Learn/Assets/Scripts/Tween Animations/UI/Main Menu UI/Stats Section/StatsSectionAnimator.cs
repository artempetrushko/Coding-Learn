using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class StatsSectionAnimator : MonoBehaviour
    {
        [SerializeField]
        private GameObject levelCardsContainer;
        [SerializeField]
        private GameObject detailedLevelStatsContainer;

        public IEnumerator ChangeVisibility_COR(bool isVisible)
        {
            if (isVisible)
            {
                levelCardsContainer.transform.localPosition = Vector3.zero;
                detailedLevelStatsContainer.transform.localPosition = new Vector3(detailedLevelStatsContainer.GetComponent<RectTransform>().rect.width, 0, 0);
            }
            var visibilityChangeTween = transform.DOLocalMoveY(isVisible ? 0 : GetComponent<RectTransform>().rect.height, 0.75f);
            yield return visibilityChangeTween.WaitForCompletion();
        }

        public IEnumerator ShowDetailedLevelStats_COR()
        {
            var tweenSequence = DOTween.Sequence();
            tweenSequence
                .Append(levelCardsContainer.transform.DOLocalMoveX(-levelCardsContainer.GetComponent<RectTransform>().rect.width, 0.75f))
                .Append(detailedLevelStatsContainer.transform.DOLocalMoveX(0, 0.75f));
            tweenSequence.Play();
            yield return tweenSequence.WaitForCompletion();
        }

        public IEnumerator ReturnToLevelCards_COR()
        {
            var tweenSequence = DOTween.Sequence();
            tweenSequence
                .Append(detailedLevelStatsContainer.transform.DOLocalMoveX(detailedLevelStatsContainer.GetComponent<RectTransform>().rect.width, 0.75f))
                .Append(levelCardsContainer.transform.DOLocalMoveX(0, 0.75f));
            tweenSequence.Play();
            yield return tweenSequence.WaitForCompletion();
        }
    }
}
