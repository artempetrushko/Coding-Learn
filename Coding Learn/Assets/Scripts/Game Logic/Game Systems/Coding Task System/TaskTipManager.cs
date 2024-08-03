using Cysharp.Threading.Tasks;
using System;

namespace Scripts
{
    public class TaskTipManager : IPadSecondaryFunction
    {
        public event Action NewTipShowed;

        private TipsSectionController _tipsSectionController;
        private TaskTipsConfig _taskTipsConfig;
        private string[] _currentTaskTips;
        private int _currentTipIndex;

        public TaskTipManager(TipsSectionController tipsSectionController, TaskTipsConfig taskTipsConfig)
        {
            _tipsSectionController = tipsSectionController;
            _taskTipsConfig = taskTipsConfig;
        }

        public void SetNewTips(string[] tips)
        {
            _currentTaskTips = tips;
            _currentTipIndex = 0;
            _tipsSectionController.ClearTipText();
            WaitUntilNextTip();
            WaitUntilTaskSkipping();
        }

        public void ShowModalSection() => _tipsSectionController.SetVisibility(true);

        public void HideModalSection() => _tipsSectionController.SetVisibility(false);

        public void ShowTip()
        {
            if (_currentTipIndex < _currentTaskTips.Length)
            {
                _tipsSectionController.AddNewTipText(_currentTaskTips[_currentTipIndex]);
                _currentTipIndex++;
                NewTipShowed.Invoke();
                WaitUntilNextTip();
            }
            else
            {
                _tipsSectionController.SetShowTipButtonInteractable(false);
                _tipsSectionController.SetTipStatusText(_taskTipsConfig.TipStatusLabelsData.ActionUnavailableText.GetLocalizedString());
            }
        }

        public void WaitUntilNextTip() => _ = WaitActionButtonUnlockingAsync(_taskTipsConfig.TimeToNextTipInSeconds, 
        () =>
        {
            _tipsSectionController.SetShowTipButtonInteractable(false);
        },
        ((int minutes, int seconds) countdownCurrentTime) =>
        {
            _tipsSectionController.SetTipStatusText(_taskTipsConfig.TipStatusLabelsData.ActionWaitingText.GetLocalizedString(string.Format("{0:d2}:{1:d2}", countdownCurrentTime.minutes, countdownCurrentTime.seconds)));
        },
        () =>
        {
            _tipsSectionController.SetTipStatusText(_taskTipsConfig.TipStatusLabelsData.ActionAvailableText.GetLocalizedString());
            _tipsSectionController.SetShowTipButtonInteractable(true);
        });

        public void WaitUntilTaskSkipping() => _ = WaitActionButtonUnlockingAsync(_taskTipsConfig.TimeToSkipTaskInSeconds,
        () =>
        {
            _tipsSectionController.SetSkipTaskButtonInteractable(false);
        },
        ((int minutes, int seconds) countdownCurrentTime) =>
        {
            _tipsSectionController.SetSkipTaskButtonLabelText(_taskTipsConfig.TaskSkippingStatusLabelsData.ActionWaitingText.GetLocalizedString(string.Format("{0:d2}:{1:d2}", countdownCurrentTime.minutes, countdownCurrentTime.seconds)));
        },
        () =>
        {
            _tipsSectionController.SetSkipTaskButtonInteractable(true);
            _tipsSectionController.SetSkipTaskButtonLabelText(_taskTipsConfig.TaskSkippingStatusLabelsData.ActionAvailableText.GetLocalizedString());
        });

        private async UniTask WaitActionButtonUnlockingAsync(int waitingTimeInSeconds, Action beforeCountdownAction, Action<(int minutes, int seconds)> duringCountdownAction, Action afterCountdownAction)
        {
            beforeCountdownAction();
            var timer = waitingTimeInSeconds;
            while (timer > 0)
            {
                var minutes = timer / 60;
                var seconds = timer - minutes * 60;
                duringCountdownAction((minutes, seconds));
                await UniTask.WaitForSeconds(1);
                timer--;
            }
            afterCountdownAction();
        }
    }
}
