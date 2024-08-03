using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using Zenject;

namespace Scripts
{
    public class ExitToMenuController : ITickable
    {
        public event Action BlackScreenShown;

        private ExitToMenuSectionView exitToMenuSectionView;
        private bool isMenuEnabled;
        private bool isMenuAnimationPlaying;

        public ExitToMenuController(ExitToMenuSectionView exitToMenuSectionView)
        {
            this.exitToMenuSectionView = exitToMenuSectionView;
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
            exitToMenuSectionView.gameObject.SetActive(true);
            await exitToMenuSectionView.ChangeVisibilityAsync(true);
        }

        private async UniTask HideExitToMenuViewAsync()
        {
            await exitToMenuSectionView.ChangeVisibilityAsync(false);
            exitToMenuSectionView.gameObject.SetActive(false);
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
            await exitToMenuSectionView.ShowBlackScreenAsync();
            BlackScreenShown?.Invoke();
        }
    }
}
