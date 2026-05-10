using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;

namespace GameLogic
{
    public class TaskTipsPresenter : IPadModalSection
    {
        public event Action TaskSkippingSelected;
        public event Action NewTipShown;

        private const float VISIBILITY_CHANGING_DURATION = 1.5f;

        private TaskTipsConfig _taskTipsConfig;
        private TaskTipsView _taskTipsView;
        
        private string[] _currentTaskTips;
        private int _currentTipIndex;

        public TaskTipsPresenter(TaskTipsView taskTipsView, TaskTipsConfig taskTipsConfig)
        {
            _taskTipsView = taskTipsView;
            _taskTipsConfig = taskTipsConfig;

            _taskTipsView.ShowTipButton.onClick.AddListener(OnShowTipButtonPressed);
            _taskTipsView.SkipTaskButton.onClick.AddListener(() => OnSkipTaskButtonPressedAsync().Forget());
        }

        public async UniTask ShowModalSectionAsync()
        {
            _taskTipsView.SetActive(true);
            await _taskTipsView.CanvasGroup.DOFade(1f, VISIBILITY_CHANGING_DURATION).AsyncWaitForCompletion();
        }

        public async UniTask HideModalSectionAsync()
        {
            await _taskTipsView.CanvasGroup.DOFade(0f, VISIBILITY_CHANGING_DURATION).AsyncWaitForCompletion();
            _taskTipsView.SetActive(false);
        }

        public void SetNewTips(string[] tips)
        {
            _currentTaskTips = tips;
            _currentTipIndex = 0;
            _taskTipsView.SetTipText("");
            _taskTipsView.SetTipFillerActive(true);

            WaitUntilNextTipAsync().Forget();
            WaitUntilTaskSkippingAsync().Forget();
        }

        private async UniTask WaitUntilNextTipAsync()
        {
            _taskTipsView.ShowTipButton.interactable = false;

            var timer = TimeSpan.FromMinutes(_taskTipsConfig.NextTipTimerConfig.TimeInMinutes);
            while (timer.TotalSeconds > 0)
            {
                timer -= TimeSpan.FromSeconds(1);
                _taskTipsView.SetTipStatusText(_taskTipsConfig.NextTipTimerConfig.ActionTimerTextsConfig.ActionWaitingText.GetLocalizedString(timer.ToString(_taskTipsConfig.TimersFormat)));
                await UniTask.WaitForSeconds(1);
            }

            _taskTipsView.SetTipStatusText(_taskTipsConfig.NextTipTimerConfig.ActionTimerTextsConfig.ActionAvailableText.GetLocalizedString());
            _taskTipsView.ShowTipButton.interactable = true;
        }

        private async UniTask WaitUntilTaskSkippingAsync()
        {
            _taskTipsView.SkipTaskButton.interactable = false;

            var timer = TimeSpan.FromMinutes(_taskTipsConfig.SkipTaskTimerConfig.TimeInMinutes);
            while (timer.TotalSeconds > 0)
            {
                timer -= TimeSpan.FromSeconds(1);
                _taskTipsView.SetSkipTaskButtonLabelText(_taskTipsConfig.SkipTaskTimerConfig.ActionTimerTextsConfig.ActionWaitingText.GetLocalizedString(timer.ToString(_taskTipsConfig.TimersFormat)));
                await UniTask.WaitForSeconds(1);
            }
            
            _taskTipsView.SetSkipTaskButtonLabelText(_taskTipsConfig.SkipTaskTimerConfig.ActionTimerTextsConfig.ActionAvailableText.GetLocalizedString());
            _taskTipsView.SkipTaskButton.interactable = true;
        }

        private void OnShowTipButtonPressed()
        {
            _taskTipsView.SetTipFillerActive(false);
            _taskTipsView.AddTipText(_currentTaskTips[_currentTipIndex]);
            _currentTipIndex++;

            if (_currentTipIndex < _currentTaskTips.Length)
            {
                WaitUntilNextTipAsync().Forget();
            }
            else
            {
                _taskTipsView.ShowTipButton.interactable = false;
                _taskTipsView.SetTipStatusText(_taskTipsConfig.NextTipTimerConfig.ActionTimerTextsConfig.ActionUnavailableText.GetLocalizedString());
            }

            NewTipShown.Invoke();
        }

        private async UniTask OnSkipTaskButtonPressedAsync()
        {
            await HideModalSectionAsync();
            TaskSkippingSelected?.Invoke();
        }
    }
}
