using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using R3;
using Scripts.Core.Features.Game.CodingTasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameLogic
{
    public class CodingTaskComponent : MonoBehaviour
    {
        [SerializeField] private TMP_Text _taskTitleText;
        [SerializeField] private TMP_Text _taskDescriptionText;
        [SerializeField] private Scrollbar _scrollbar;

        public event Action CodingTaskCompleted;
        public event Action<TrainingData[]> TrainingSelected;

        private const float VISIBILITY_CHANGING_DURATION = 1f;

        private readonly CodingTaskService _codingTaskService;
        private ChallengesPresenter _challengePresenter;
        private bool _isTaskStarted;

        public CodingTaskComponent(CodingTaskService codingTaskService)
        {
            _codingTaskService = codingTaskService;

            _codingTaskService.CurrentTask.Subscribe(task =>
            {
                _taskTitleText.text = task.Title.GetLocalizedString();
                _taskDescriptionText.text = task.Description.GetLocalizedString();
                _scrollbar.value = 1;

                SetVisibilityAsync(true).Forget();
            });

            _challengePresenter.ChallengesCompletingChecked += OnChallengesCompletingChecked;
        }

        public void Dispose()
        {
            _challengePresenter.ChallengesCompletingChecked -= OnChallengesCompletingChecked;
        }

        public void ShowCurrentTask() => SetVisibilityAsync(true).Forget();

        private async UniTask SetVisibilityAsync(bool isVisible)
        {
            var movementOffsetXSign = isVisible ? 1 : -1;
            transform.DOLocalMoveX(transform.localPosition.x + transform.sizeDelta.x * movementOffsetXSign, VISIBILITY_CHANGING_DURATION).ToUniTask().Forget();
            _devEnvironmentPresenter.ShowAsync().Forget();

            await UniTask.WaitForSeconds(VISIBILITY_CHANGING_DURATION);
        }

        private async UniTask ShowTaskResultsAsync(bool isTaskSkipped = false)
        {
            await SetVisibilityAsync(false);
            _challengePresenter.CheckChallengesCompleting(_codingTaskModel, isTaskSkipped);
        }

        private async UniTask ReturnToCodingTrainingAsync(TrainingData[] trainingDatas)
        {
            await SetVisibilityAsync(false);
            TrainingSelected?.Invoke(trainingDatas);
        }

        private void OnTaskCompleted()
        {
            _isTaskStarted = false;
            ShowTaskResultsAsync().Forget();
        }

        private void OnHandbookButtonPressed() => _handbookPresenter.ShowModalSectionAsync().Forget();

        private void OnTipsButtonPressed() => _taskTipsPresenter.ShowModalSectionAsync().Forget();

        private void OnChallengesButtonPressed() => _challengePresenter.ShowModalSectionAsync().Forget();

        private void OnSubThemeButtonPressed(TrainingData[] trainingDatas) => ReturnToCodingTrainingAsync(trainingDatas).Forget();

        private void OnNewTipShown() => _codingTaskModel.UsedTipsCount++;

        private void OnTaskSkippingSelected() => ShowTaskResultsAsync(true).Forget();

        private void OnChallengesCompletingChecked() => CodingTaskCompleted?.Invoke();
    }
}
