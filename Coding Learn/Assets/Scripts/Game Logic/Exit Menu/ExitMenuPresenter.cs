using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using UnityEngine;
using Zenject;

namespace UI.Game
{
    public class ExitMenuPresenter : ITickable
    {
        public event Action ExitToMenuSelected;

        private ExitMenuView _exitMenuView;
        private bool isMenuEnabled;
        private bool isMenuAnimationPlaying;

        public ExitMenuPresenter(ExitMenuView exitMenuView)
        {
            _exitMenuView = exitMenuView;
        }

        public void HideExitToMenuView() => HideExitToMenuViewAsync().Forget();

        public void ExitToMenu() => ExitToMenuAsync().Forget();

        public void Tick()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !isMenuAnimationPlaying)
            {
                PlayMenuAnimationAsync(isMenuEnabled ? HideExitToMenuViewAsync : ShowExitToMenuViewAsync).Forget();
            }
        }

        private async UniTask ShowExitToMenuViewAsync()
        {
            Time.timeScale = 0f;
            _exitMenuView.gameObject.SetActive(true);
            //await exitToMenuSectionView.ChangeVisibilityAsync(true);
        }

        private async UniTask HideExitToMenuViewAsync()
        {
            //await exitToMenuSectionView.ChangeVisibilityAsync(false);
            _exitMenuView.gameObject.SetActive(false);
            Time.timeScale = 1f;
        }

        private async UniTask PlayMenuAnimationAsync(Func<UniTask> animation)
        {
            isMenuAnimationPlaying = true;
            await animation();
            isMenuAnimationPlaying = false;
        }

        private async UniTask ExitToMenuAsync()
        {
            //await exitToMenuSectionView.ShowBlackScreenAsync();
            ExitToMenuSelected?.Invoke();
        }




        public async UniTask ChangeContentVisibilityAsync(bool isVisible)
        {
            var backgroundEndOpacity = isVisible ? 0.9f : 0;
            var contentOpacity = isVisible ? 1 : 0;

            var tweenSequence = DOTween.Sequence();
            tweenSequence.Pause();
            tweenSequence
                .Append(_exitMenuView.Background.DOFade(backgroundEndOpacity, 1f))
                .Append(_exitMenuView.ContentCanvasGroup.DOFade(contentOpacity, 0.75f));
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
            await _exitMenuView.BlackScreen
                .DOFade(1f, 2f)
                .SetUpdate(true)
                .AsyncWaitForCompletion();
        }
    }
}
