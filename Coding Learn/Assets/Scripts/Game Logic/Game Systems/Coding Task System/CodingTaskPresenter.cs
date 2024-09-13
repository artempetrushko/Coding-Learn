using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UI.Game;

namespace GameLogic
{
    public class CodingTaskPresenter : IDisposable
    {
        public event Action CodingTaskCompleted;
        public event Action TrainingSelected;

        private const float VISIBILITY_CHANGING_DURATION = 1f;

        private CodingTaskView _taskDescriptionSectionView;
        private CodingTaskModel _currentTask;
        private DevEnvironmentPresenter _devEnvironmentPresenter;
        private TaskTipsPresenter _taskTipsPresenter;
        private ChallengesPresenter _challengePresenter;
        private bool _isTaskStarted;

        public CodingTaskPresenter(CodingTaskView taskDescriptionSectionView, DevEnvironmentPresenter devEnvironmentPresenter, ChallengesPresenter challengesPresenter, TaskTipsPresenter taskTipsPresenter)
        {
            _taskDescriptionSectionView = taskDescriptionSectionView;
            _devEnvironmentPresenter = devEnvironmentPresenter;
            _challengePresenter = challengesPresenter;
            _taskTipsPresenter = taskTipsPresenter;

            _devEnvironmentPresenter.TaskCompleted += OnTaskCompleted;
            _taskTipsPresenter.NewTipShown += OnNewTipShown;
            _taskTipsPresenter.TaskSkippingSelected += OnTaskSkippingSelected;
            _challengePresenter.ChallengesCompletingChecked += OnChallengesCompletingChecked;
        }

        public void Dispose()
        {
            _devEnvironmentPresenter.TaskCompleted -= OnTaskCompleted;
            _taskTipsPresenter.TaskSkippingSelected -= OnTaskSkippingSelected;
            _challengePresenter.ChallengesCompletingChecked -= OnChallengesCompletingChecked;
        }

        public void StartCodingTask(CodingTaskConfig codingTask)
        {
            _currentTask = new CodingTaskModel(codingTask);
            StartTaskCompletingTimerAsync().Forget();

            _taskDescriptionSectionView.SetTaskTitleText(_currentTask.Config.Title.GetLocalizedString());
            _taskDescriptionSectionView.SetTaskDescriptionText(_currentTask.Config.Description.GetLocalizedString());
            _taskDescriptionSectionView.SetScrollbarValue(1);

            SetVisibilityAsync(true).Forget();
        }

        public async UniTask ShowTaskContent() { } //await _taskSectionController.ChangeMainContentVisibilityAsync(true);

        public async UniTask HideTaskContent() { } //await _taskSectionController.ChangeMainContentVisibilityAsync(false);

        private async UniTask ShowTaskResultsAsync(bool isTaskSkipped = false)
        {
            await HideTaskContent();
            _challengePresenter.CheckChallengesCompleting(_currentTask, isTaskSkipped);
        }




        private async UniTask StartTaskCompletingTimerAsync()
        {
            _isTaskStarted = true;
            while (_isTaskStarted)
            {
                await UniTask.WaitForSeconds(1);
                _currentTask.TaskCompletingTimeInSeconds++;
            }
        }








        public async UniTask ChangeMainContentVisibilityAsync(bool isVisible)
        {
            var movementOffsetXSign = isVisible ? 1f : -1f;
            var padViewRightMargin = 20;

            await SetVisibilityAsync(isVisible);

            //padSectionView.transform.DOLocalMoveX(padSectionView.transform.localPosition.x - (padSectionView.GetComponent<RectTransform>().sizeDelta.x + padViewRightMargin) * movementOffsetXSign, 1f);
            await UniTask.WaitForSeconds(1);
        }



        private async UniTask SetVisibilityAsync(bool isVisible)
        {
            var movementOffsetXSign = isVisible ? 1 : -1;
            await _taskDescriptionSectionView.transform
                .DOLocalMoveX(_taskDescriptionSectionView.LocalPosition.x + _taskDescriptionSectionView.SizeDelta.x * movementOffsetXSign, VISIBILITY_CHANGING_DURATION)
                .AsyncWaitForCompletion();
        }

        private void OnTaskCompleted()
        {
            _isTaskStarted = false;
            ShowTaskResultsAsync().Forget();
        }

        private void OnNewTipShown() => _currentTask.UsedTipsCount++;

        private void OnTaskSkippingSelected() => ShowTaskResultsAsync(true).Forget();

        private void OnChallengesCompletingChecked() => CodingTaskCompleted?.Invoke();
    }
}
