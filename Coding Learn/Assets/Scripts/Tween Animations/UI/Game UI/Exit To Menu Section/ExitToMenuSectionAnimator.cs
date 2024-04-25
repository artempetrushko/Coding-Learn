using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class ExitToMenuSectionAnimator : MonoBehaviour
    {
        [SerializeField]
        private Image background;
        [SerializeField]
        private GameObject contentContainer;
        [SerializeField]
        private Image blackScreen;

        public async UniTask ChangeContentVisibilityAsync(bool isVisible)
        {
            var backgroundEndOpacity = isVisible ? 0.9f : 0;
            var contentOpacity = isVisible ? 1 : 0;

            var tweenSequence = DOTween.Sequence();
            tweenSequence.Pause();
            tweenSequence.Append(background.DOFade(backgroundEndOpacity, 1f));
            tweenSequence.Append(contentContainer.GetComponent<CanvasGroup>().DOFade(contentOpacity, 0.75f));
            tweenSequence.SetUpdate(true);
            if (isVisible)
            {
                tweenSequence.Play();
            }
            else
            {
                tweenSequence.PlayBackwards();
            }
            await tweenSequence.AsyncWaitForCompletion();
        }

        public async UniTask ShowBlackScreenAsync()
        {
            await blackScreen
                .DOFade(1f, 2f)
                .SetUpdate(true)
                .AsyncWaitForCompletion();
        }
    }
}
