using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace GameLogic
{
    public class ExitMenuComponent : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Button _confirmButton;
        [SerializeField] private Button _cancelButton;
        [SerializeField] private Image _blackScreen;

        public event Action ExitToMenuSelected;

        private const float VISIBILITY_CHANGING_DURATION = 1f;

        private ExitMenuComponent _exitMenuView;
        private bool _isMenuAnimationPlaying = false;

        private void Start()
        {
            _confirmButton.onClick.AddListener(OnConfirmButtonPressed);
            _cancelButton.onClick.AddListener(OnCancelButtonPressed);
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

            gameObject.SetActive(true);
            await SetMenuVisiblityAsync(true);
        }

        private async UniTask HideMenuAsync()
        {
            await SetMenuVisiblityAsync(false);
            gameObject.SetActive(false);

            Time.timeScale = 1f;
        }

        private async UniTask SetMenuVisiblityAsync(bool isVisible)
        {
            await _canvasGroup
                .DOFade(isVisible ? 1f : 0f, VISIBILITY_CHANGING_DURATION)
                .SetUpdate(true)
                .AsyncWaitForCompletion();
        }

        private async UniTask ExitToMenuAsync()
        {
            await _blackScreen
                .DOFade(1f, VISIBILITY_CHANGING_DURATION)
                .SetUpdate(true)
                .AsyncWaitForCompletion();

            ExitToMenuSelected?.Invoke();
        }

        private void OnConfirmButtonPressed() => ExitToMenuAsync().Forget();

        private void OnCancelButtonPressed() => ToggleMenuVisibilityAsync().Forget();
    }
}
