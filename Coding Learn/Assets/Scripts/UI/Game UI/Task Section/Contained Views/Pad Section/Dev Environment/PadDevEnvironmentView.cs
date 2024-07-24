using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class PadDevEnvironmentView : MonoBehaviour
    {
        [SerializeField]
        private CodeFieldView codeFieldView;
        [SerializeField]
        private Button errorsButton;
        [SerializeField]
        private ErrorsSectionView errorsSectionView;
        [SerializeField]
        private GameObject buttonsClickBlocker;

        public string CodeFieldContent => codeFieldView.CodeFieldContent;

        public void SetDefaultCode(string defaultCode) => codeFieldView.SetDefaultCode(defaultCode);

        public void SetAndShowCompilationErrorsInfo(List<(int line, int column, string message)> errors)
        {
            var errorsMessage = string.Join("\n", errors.Select(error => $"<color=red>Error</color> ({error.line}, {error.column}): {error.message}"));
            ShowErrorsAsync(errorsMessage);
        }

        public async UniTask ShowExecutingProcessAsync(bool isTaskCompleted)
        {
            buttonsClickBlocker.SetActive(true);
            errorsButton.interactable = false;
            await errorsSectionView.ChangeVisibilityAsync(false);
            await ShowTaskSolutionCheckingAsync(isTaskCompleted);
            if (!isTaskCompleted)
            {
                await ShowErrorsAsync("Some of tests were failed! Try again!");
            }
            buttonsClickBlocker.SetActive(true);
        }

        private async UniTask ShowErrorsAsync(string errorsMessage)
        {
            await ShowTaskCompletingIndicatorAsync(false);
            errorsButton.interactable = true;
            errorsSectionView.SetContent(errorsMessage);
            errorsSectionView.ChangeVisibilityAsync(true);
        }





        [SerializeField]
        private Image programExecutingProgressBar;
        [SerializeField]
        private Image taskCompletingIndicator;

        private Color progressBarNormalColor = Color.blue;
        private Color progressBarSuccessColor = Color.green;
        private Color progressBarFailureColor = Color.red;

        public async UniTask ShowTaskSolutionCheckingAsync(bool isTaskCompleted)
        {
            programExecutingProgressBar.color = progressBarNormalColor;
            var progressBarFillingTween = programExecutingProgressBar.DOFillAmount(1f, 3f);
            await progressBarFillingTween.AsyncWaitForCompletion();
            if (isTaskCompleted)
            {
                programExecutingProgressBar.color = progressBarSuccessColor;
                await ShowTaskCompletingIndicatorAsync(isTaskCompleted);
            }
            programExecutingProgressBar.fillAmount = 0f;
        }
        public async UniTask ShowTaskCompletingIndicatorAsync(bool isTaskCompleted)
        {
            var indicatorEndColor = isTaskCompleted ? progressBarSuccessColor : progressBarFailureColor;
            var indicatorStartColor = indicatorEndColor;
            indicatorStartColor.a = 0f;
            indicatorEndColor.a = 0.5f;

            taskCompletingIndicator.gameObject.SetActive(true);
            taskCompletingIndicator.color = indicatorStartColor;

            var tweenSequence = DOTween.Sequence();
            tweenSequence
                .Append(taskCompletingIndicator.DOColor(indicatorEndColor, 0.7f))
                .Append(taskCompletingIndicator.DOColor(indicatorStartColor, 0.7f));
            await tweenSequence.Play().AsyncWaitForCompletion();
            taskCompletingIndicator.gameObject.SetActive(false);
        }
    }
}
