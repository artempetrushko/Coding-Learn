using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace GameLogic
{
    public class ExitMenuPresenter : ITickable
    {
        public event Action ExitToMenuSelected;

        private const float VISIBILITY_CHANGING_DURATION = 1f;

        private ExitMenuView _exitMenuView;
        private bool _isMenuAnimationPlaying = false;

        public ExitMenuPresenter(ExitMenuView exitMenuView)
        {
            _exitMenuView = exitMenuView;

            _exitMenuView.ConfirmButton.onClick.AddListener(OnConfirmButtonPressed);
            _exitMenuView.CancelButton.onClick.AddListener(OnCancelButtonPressed);
        }

        public void Tick()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ToggleMenuVisibilityAsync().Forget();
            }
        }

        private async UniTask ToggleMenuVisibilityAsync()
        {
            if (!_isMenuAnimationPlaying)
            {
                _isMenuAnimationPlaying = true;

                if (_exitMenuView.isActiveAndEnabled)
                {
                    await HideMenuAsync();
                }
                else
                {
                    await ShowMenuAsync();
                }

                _isMenuAnimationPlaying = false;
            }
            
        }

        private async UniTask ShowMenuAsync()
        {
            Time.timeScale = 0f;

            _exitMenuView.SetActive(true);
            await SetMenuVisiblityAsync(true);
        }

        private async UniTask HideMenuAsync()
        {
            await SetMenuVisiblityAsync(false);
            _exitMenuView.SetActive(false);

            Time.timeScale = 1f;
        }

        private async UniTask SetMenuVisiblityAsync(bool isVisible)
        {
            await _exitMenuView.CanvasGroup
                .DOFade(isVisible ? 1f : 0f, VISIBILITY_CHANGING_DURATION)
                .SetUpdate(true)
                .AsyncWaitForCompletion();
        }

        private async UniTask ExitToMenuAsync()
        {
            await _exitMenuView.BlackScreen
                .DOFade(1f, VISIBILITY_CHANGING_DURATION)
                .SetUpdate(true)
                .AsyncWaitForCompletion();

            ExitToMenuSelected?.Invoke();
        }

        private void OnConfirmButtonPressed() => ExitToMenuAsync().Forget();

        private void OnCancelButtonPressed() => ToggleMenuVisibilityAsync().Forget();
    }
}
