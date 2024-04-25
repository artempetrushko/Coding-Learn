using Cysharp.Threading.Tasks;
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
