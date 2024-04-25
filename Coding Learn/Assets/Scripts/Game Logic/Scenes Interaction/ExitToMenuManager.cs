using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts
{
    public class ExitToMenuManager : MonoBehaviour
    {
        [SerializeField]
        private ExitToMenuSectionView exitToMenuSectionView;
        [SerializeField]
        private UnityEvent onBlackScreenShown;

        private bool isMenuEnabled;
        private bool isMenuAnimationPlaying;

        public void HideExitToMenuView() => _ = HideExitToMenuViewAsync();

        public void ExitToMenu() => _ = ExitToMenuAsync();

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !isMenuAnimationPlaying)
            {
                _ = PlayMenuAnimationAsync(isMenuEnabled ? HideExitToMenuViewAsync : ShowExitToMenuViewAsync);
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
            onBlackScreenShown.Invoke();
        }
    }
}
