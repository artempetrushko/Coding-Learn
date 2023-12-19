using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    public class PadModalWindowAnimator : MonoBehaviour
    {
        [SerializeField]
        protected CanvasGroup windowCanvasGroup;

        protected Sequence windowVisibilityChangeTween;

        public virtual IEnumerator ChangeVisibility_COR(bool isVisible)
        {
            windowVisibilityChangeTween ??= CreateWindowVisibilityChangeTween();
            if (isVisible)
            {
                windowVisibilityChangeTween.PlayForward();
            }
            else
            {
                windowVisibilityChangeTween.PlayBackwards();
            }
            yield return windowVisibilityChangeTween.WaitForRewind();
        }

        protected Sequence CreateWindowVisibilityChangeTween()
        {
            var tweenSequence = DOTween.Sequence();
            tweenSequence.Pause();
            tweenSequence.Append(transform.DOScale(1f, 0.01f));
            tweenSequence.Append(windowCanvasGroup.DOFade(1f, 0.5f));
            tweenSequence.SetAutoKill(false);
            return tweenSequence;
        }
    }
}
