using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Game
{
    public class DevEnvironmentView : MonoBehaviour
    {
        [SerializeField] private CodeFieldView _codeFieldView;
        [SerializeField] private Button _executeCodeButton;
        [SerializeField] private Button _errorsButton;

        [SerializeField] private Image _programExecutingProgressBar;
        [SerializeField] private Image _taskCompletingIndicator;

        public Button ExecuteCodeButton => _executeCodeButton;

        public void SetActive(bool isActive) => gameObject.SetActive(isActive);

        public void SetErrorsButtonInteractable(bool isInteractable) => _errorsButton.interactable = isInteractable;

        public void SetProgramExecutingProgressBarColor(Color color) => _programExecutingProgressBar.color = color;

        public void SetProgramExecutingProgressBarFillAmount(float fillAmount) => _programExecutingProgressBar.fillAmount = fillAmount;

        public async UniTask FillProgramExecutingProgressBarAsync(float duration)
        {
            await _programExecutingProgressBar
                .DOFillAmount(1f, duration)
                .AsyncWaitForCompletion();
        }

        public void SetTaskCompletingIndicatorActive(bool isActive) => _taskCompletingIndicator.gameObject.SetActive(isActive);

        public void SetTaskCompletingIndicatorColor(Color color) => _taskCompletingIndicator.color = color;

        public async UniTask SetTaskCompletingIndicatorAlphaAsync(float alpha, float duration)
        {
            await _taskCompletingIndicator
                .DOFade(alpha, duration)
                .AsyncWaitForCompletion();
        }

        public string CodeFieldContent => _codeFieldView.CodeFieldContent;

        
    }
}
