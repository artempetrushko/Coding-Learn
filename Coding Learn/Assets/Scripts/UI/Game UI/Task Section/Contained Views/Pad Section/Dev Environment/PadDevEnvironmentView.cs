using Cysharp.Threading.Tasks;
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
        [Space, SerializeField]
        private PadDevEnvironmentAnimator animator;

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
            await animator.ShowTaskSolutionCheckingAsync(isTaskCompleted);
            if (!isTaskCompleted)
            {
                await ShowErrorsAsync("Some of tests were failed! Try again!");
            }
            buttonsClickBlocker.SetActive(true);
        }

        private async UniTask ShowErrorsAsync(string errorsMessage)
        {
            await animator.ShowTaskCompletingIndicatorAsync(false);
            errorsButton.interactable = true;
            errorsSectionView.SetContent(errorsMessage);
            errorsSectionView.ChangeVisibilityAsync(true);
        }
    }
}
