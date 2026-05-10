using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;

namespace GameLogic
{
    public class CodingTaskPresenter : IDisposable
    {
        public event Action CodingTaskCompleted;
        public event Action<TrainingData[]> TrainingSelected;

        private const float VISIBILITY_CHANGING_DURATION = 1f;

        private CodingTaskView _codingTaskView;
        private CodingTaskModel _codingTaskModel;
        private DevEnvironmentPresenter _devEnvironmentPresenter;
        private HandbookPresenter _handbookPresenter;
        private TaskTipsPresenter _taskTipsPresenter;
        private ChallengesPresenter _challengePresenter;
        private bool _isTaskStarted;

        public CodingTaskPresenter(CodingTaskView codingTaskView, DevEnvironmentPresenter devEnvironmentPresenter, HandbookPresenter handbookPresenter, ChallengesPresenter challengesPresenter, TaskTipsPresenter taskTipsPresenter)
        {
            _codingTaskView = codingTaskView;
            _devEnvironmentPresenter = devEnvironmentPresenter;
            _handbookPresenter = handbookPresenter;
            _challengePresenter = challengesPresenter;
            _taskTipsPresenter = taskTipsPresenter;

            _devEnvironmentPresenter.TaskCompleted += OnTaskCompleted;
            _devEnvironmentPresenter.HandbookButtonPressed += OnHandbookButtonPressed;
            _devEnvironmentPresenter.TipsButtonPressed += OnTipsButtonPressed;
            _devEnvironmentPresenter.ChallengesButtonPressed += OnChallengesButtonPressed;
            _handbookPresenter.SubThemeButtonPressed += OnSubThemeButtonPressed;
            _taskTipsPresenter.NewTipShown += OnNewTipShown;
            _taskTipsPresenter.TaskSkippingSelected += OnTaskSkippingSelected;
            _challengePresenter.ChallengesCompletingChecked += OnChallengesCompletingChecked;
        }

        public void Dispose()
        {
            _devEnvironmentPresenter.TaskCompleted -= OnTaskCompleted;
            _devEnvironmentPresenter.HandbookButtonPressed -= OnHandbookButtonPressed;
            _devEnvironmentPresenter.TipsButtonPressed -= OnTipsButtonPressed;
            _devEnvironmentPresenter.ChallengesButtonPressed -= OnChallengesButtonPressed;
            _taskTipsPresenter.NewTipShown -= OnNewTipShown;
            _taskTipsPresenter.TaskSkippingSelected -= OnTaskSkippingSelected;
            _challengePresenter.ChallengesCompletingChecked -= OnChallengesCompletingChecked;
        }

        public void StartCodingTask(CodingTaskConfig codingTask)
        {
            _codingTaskModel = new CodingTaskModel(codingTask);
            StartTaskCompletingTimerAsync().Forget();

            _codingTaskView.SetTaskTitleText(_codingTaskModel.Config.Title.GetLocalizedString());
            _codingTaskView.SetTaskDescriptionText(_codingTaskModel.Config.Description.GetLocalizedString());
            _codingTaskView.SetScrollbarValue(1);

            SetVisibilityAsync(true).Forget();
        }

        public void ShowCurrentTask() => SetVisibilityAsync(true).Forget();

        private async UniTask SetVisibilityAsync(bool isVisible)
        {
            var movementOffsetXSign = isVisible ? 1 : -1;
            _codingTaskView.transform.DOLocalMoveX(_codingTaskView.LocalPosition.x + _codingTaskView.SizeDelta.x * movementOffsetXSign, VISIBILITY_CHANGING_DURATION).ToUniTask().Forget();
            _devEnvironmentPresenter.ShowAsync().Forget();

            await UniTask.WaitForSeconds(VISIBILITY_CHANGING_DURATION);
        }

        private async UniTask StartTaskCompletingTimerAsync()
        {
            _isTaskStarted = true;
            while (_isTaskStarted)
            {
                await UniTask.WaitForSeconds(1);
                _codingTaskModel.TaskCompletingTimeInSeconds++;
            }
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
