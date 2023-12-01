using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class PadDevEnvironmentAnimator : MonoBehaviour
    {
        [SerializeField]
        private Image programExecutingProgressBar;
        [SerializeField]
        private Image taskCompletingIndicator;

        private Color progressBarNormalColor = Color.blue;
        private Color progressBarSuccessColor = Color.green;
        private Color progressBarFailureColor = Color.red;

        public IEnumerator ShowTaskSolutionChecking_COR(bool isTaskCompleted)
        {
            programExecutingProgressBar.color = progressBarNormalColor;
            var progressBarFillingTween = programExecutingProgressBar.DOFillAmount(1f, 3f);
            yield return progressBarFillingTween.WaitForCompletion();
            if (isTaskCompleted)
            {
                programExecutingProgressBar.color = progressBarSuccessColor;
                yield return StartCoroutine(ShowTaskCompletingIndicator_COR(isTaskCompleted));
            }
            programExecutingProgressBar.fillAmount = 0f;
        }
        public IEnumerator ShowTaskCompletingIndicator_COR(bool isTaskCompleted)
        {
            var indicatorEndColor = isTaskCompleted ? progressBarSuccessColor : progressBarFailureColor;
            var indicatorStartColor = indicatorEndColor;
            indicatorStartColor.a = 0f;
            indicatorEndColor.a = 0.5f;

            taskCompletingIndicator.gameObject.SetActive(true);
            taskCompletingIndicator.color = indicatorStartColor;

            var tweenSequence = DOTween.Sequence();
            tweenSequence
                .Append(taskCompletingIndicator.DOColor(indicatorEndColor, 1.5f))
                .Append(taskCompletingIndicator.DOColor(indicatorStartColor, 1.5f));
            tweenSequence.Play();
            yield return tweenSequence.WaitForCompletion();
            taskCompletingIndicator.gameObject.SetActive(false);
        }
    }
}
