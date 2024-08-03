using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Scripts
{
    public class PadModalWindow : MonoBehaviour
    {
        public virtual void SetVisibility(bool isVisible) => ChangeVisibilityAsync(isVisible);



        [SerializeField]
        protected CanvasGroup windowCanvasGroup;

        protected Sequence windowVisibilityChangeTween;

        public virtual async UniTask ChangeVisibilityAsync(bool isVisible)
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
            await windowVisibilityChangeTween.AsyncWaitForRewind();
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
