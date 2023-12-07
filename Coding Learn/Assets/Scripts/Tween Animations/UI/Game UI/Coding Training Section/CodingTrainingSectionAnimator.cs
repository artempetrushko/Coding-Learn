using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class CodingTrainingSectionAnimator : MonoBehaviour
    {
        [SerializeField]
        private GameObject content;
        [SerializeField]
        private List<Image> backgroundParts;

        private Sequence visibilityChangeTween;

        public IEnumerator ChangeVisibility_COR(bool isVisible)
        {
            visibilityChangeTween ??= CreateVisibilityChangeTween();
            if (isVisible)
            {
                visibilityChangeTween.PlayForward();
            }
            else
            {
                visibilityChangeTween.PlayBackwards();
            }
            yield return visibilityChangeTween.WaitForRewind();
        }

        private Sequence CreateVisibilityChangeTween()
        {
            var fillingDuration = 1f;
            var everyPartFillingDuration = fillingDuration / backgroundParts.Count;
            var endFillAmount = 1f;
            var contentEndPositionY = 0f;

            var tweenSequence = DOTween.Sequence();
            tweenSequence.Pause();
            backgroundParts.ForEach(part => tweenSequence.Append(part.DOFillAmount(endFillAmount, everyPartFillingDuration)));
            tweenSequence.Append(content.transform.DOLocalMoveY(contentEndPositionY, 0.5f));
            tweenSequence.SetAutoKill(false);
            return tweenSequence;
        }
    }
}
