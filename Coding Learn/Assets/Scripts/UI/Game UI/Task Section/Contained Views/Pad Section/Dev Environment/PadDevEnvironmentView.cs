using System.Collections;
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
            StartCoroutine(ShowErrors_COR(errorsMessage));
        }

        public IEnumerator ShowExecutingProcess_COR(bool isTaskCompleted)
        {
            buttonsClickBlocker.SetActive(true);
            errorsButton.interactable = false;
            yield return StartCoroutine(errorsSectionView.ChangeVisibility_COR(false));
            yield return StartCoroutine(animator.ShowTaskSolutionChecking_COR(isTaskCompleted));
            if (!isTaskCompleted)
            {
                yield return StartCoroutine(ShowErrors_COR("Some of tests were failed! Try again!"));
            }
            buttonsClickBlocker.SetActive(true);
        }

        private IEnumerator ShowErrors_COR(string errorsMessage)
        {
            yield return StartCoroutine(animator.ShowTaskCompletingIndicator_COR(false));
            errorsButton.interactable = true;
            errorsSectionView.SetContent(errorsMessage);
            StartCoroutine(errorsSectionView.ChangeVisibility_COR(true));
        }
    }
}
