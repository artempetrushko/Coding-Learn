using Cysharp.Threading.Tasks;
using DG.Tweening;

namespace Scripts
{
    public class ExitToMenuSectionController 
    {
        private ExitToMenuSectionView _exitToMenuSectionView;

        public ExitToMenuSectionController(ExitToMenuSectionView exitToMenuSectionView)
        {
            _exitToMenuSectionView = exitToMenuSectionView;
        }

        public async UniTask ChangeContentVisibilityAsync(bool isVisible)
        {
            var backgroundEndOpacity = isVisible ? 0.9f : 0;
            var contentOpacity = isVisible ? 1 : 0;

            var tweenSequence = DOTween.Sequence();
            tweenSequence.Pause();
            tweenSequence
                .Append(_exitToMenuSectionView.Background.DOFade(backgroundEndOpacity, 1f))
                .Append(_exitToMenuSectionView.ContentCanvasGroup.DOFade(contentOpacity, 0.75f));
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
            await _exitToMenuSectionView.BlackScreen
                .DOFade(1f, 2f)
                .SetUpdate(true)
                .AsyncWaitForCompletion();
        }
    }
}
