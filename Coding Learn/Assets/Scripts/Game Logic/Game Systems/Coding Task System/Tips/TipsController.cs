using Cysharp.Threading.Tasks;
using System;

namespace Scripts
{
    public class TipsController : IPadSecondaryFunction
    {
        public event Action NewTipShowed;

        private PadTipsScreenView padTipsScreenView;
        private TipSectionLabelsData tipStatusLabelsData;
        private TipSectionLabelsData taskSkippingStatusLabelsData;
        private int timeToNextTipInSeconds;
        private int timeToSkipTaskInSeconds;
        private string[] currentTaskTips;
        private int currentTipIndex;

        public TipsController(PadTipsScreenView padTipsScreenView, TipSectionLabelsData tipStatusLabelsData, TipSectionLabelsData taskSkippingStatusLabelsData, int timeToNextTipInSeconds, int timeToSkipTaskInSeconds)
        {
            this.padTipsScreenView = padTipsScreenView;
            this.tipStatusLabelsData = tipStatusLabelsData;
            this.taskSkippingStatusLabelsData = taskSkippingStatusLabelsData;
            this.timeToNextTipInSeconds = timeToNextTipInSeconds;
            this.timeToSkipTaskInSeconds = timeToSkipTaskInSeconds;
        }

        public void SetNewTips(string[] tips)
        {
            currentTaskTips = tips;
            currentTipIndex = 0;
            padTipsScreenView.ClearTipText();
            WaitUntilNextTip();
            WaitUntilTaskSkipping();
        }

        public void ShowModalWindow() => padTipsScreenView.SetVisibility(true);

        public void HideModalWindow() => padTipsScreenView.SetVisibility(false);

        public void ShowTip()
        {
            if (currentTipIndex < currentTaskTips.Length)
            {
                padTipsScreenView.AddNewTipText(currentTaskTips[currentTipIndex]);
                currentTipIndex++;
                NewTipShowed.Invoke();
                WaitUntilNextTip();
            }
            else
            {
                padTipsScreenView.SetShowTipButtonState(false);
                padTipsScreenView.SetTipStatusText(tipStatusLabelsData.ActionUnavailableLabel.GetLocalizedString());
            }
        }

        public void WaitUntilNextTip() => _ = WaitActionButtonUnlockingAsync(timeToNextTipInSeconds, 
        () =>
        {
            padTipsScreenView.SetShowTipButtonState(false);
        },
        ((int minutes, int seconds) countdownCurrentTime) =>
        {
            padTipsScreenView.SetTipStatusText(tipStatusLabelsData.ActionWaitingLabel.GetLocalizedString(string.Format("{0:d2}:{1:d2}", countdownCurrentTime.minutes, countdownCurrentTime.seconds)));
        },
        () =>
        {
            padTipsScreenView.SetTipStatusText(tipStatusLabelsData.ActionAvailableLabel.GetLocalizedString());
            padTipsScreenView.SetShowTipButtonState(true);
        });

        public void WaitUntilTaskSkipping() => _ = WaitActionButtonUnlockingAsync(timeToSkipTaskInSeconds,
        () =>
        {
            padTipsScreenView.SetSkipTaskButtonState(false);
        },
        ((int minutes, int seconds) countdownCurrentTime) =>
        {
            padTipsScreenView.SetSkipTaskButtonLabelText(taskSkippingStatusLabelsData.ActionWaitingLabel.GetLocalizedString(string.Format("{0:d2}:{1:d2}", countdownCurrentTime.minutes, countdownCurrentTime.seconds)));
        },
        () =>
        {
            padTipsScreenView.SetSkipTaskButtonState(true);
            padTipsScreenView.SetSkipTaskButtonLabelText(taskSkippingStatusLabelsData.ActionAvailableLabel.GetLocalizedString());
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
                await UniTask.Delay(TimeSpan.FromSeconds(1));
                timer--;
            }
            afterCountdownAction();
        }
    }
}
