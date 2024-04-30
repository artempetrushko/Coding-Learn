using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts
{
    public class TipsManager : PadFunctionManager
    {
        [SerializeField]
        private int timeToNextTipInSeconds;
        [SerializeField]
        private int timeToSkipTaskInSeconds;
        [Space, SerializeField]
        private PadTipsScreenView padTipsScreenView;
        [Space, SerializeField]
        private UnityEvent onNewTipShowed;

        private string[] currentTaskTips;
        private int nextTipIndex;

        public override void Initialize(int currentTaskNumber)
        {
            //currentTaskTips = GameContentManager.GetTaskInfo(currentTaskNumber).Tips;
            nextTipIndex = 0;
            padTipsScreenView.ClearTipText();
            WaitUntilNextTip();
            WaitUntilTaskSkipping();
        }

        public override void ShowModalWindow() => padTipsScreenView.SetVisibility(true);

        public override void HideModalWindow() => padTipsScreenView.SetVisibility(false);

        public void ShowTip()
        {
            if (nextTipIndex < currentTaskTips.Length)
            {
                padTipsScreenView.AddNewTipText(currentTaskTips[nextTipIndex]);
                nextTipIndex++;
                onNewTipShowed.Invoke();
                WaitUntilNextTip();
            }
            else
            {
                padTipsScreenView.SetShowTipButtonState(false);
                padTipsScreenView.SetTipStatusText("Game UI (Pad)", "No Tips Label");
            }
        }

        public void WaitUntilNextTip() => _ = MakeCountdownToActionButtonUnlockingAsync(timeToNextTipInSeconds, 
        () =>
        {
            padTipsScreenView.SetShowTipButtonState(false);
        },
        ((int minutes, int seconds) countdownCurrentTime) =>
        {
            padTipsScreenView.SetTipStatusTextWithTimer("Game UI (Pad)", "Tip Waiting Label", string.Format("{0:d2}:{1:d2}", countdownCurrentTime.minutes, countdownCurrentTime.seconds));
        },
        () =>
        {
            padTipsScreenView.SetTipStatusText("Game UI (Pad)", "Tip Available Label");
            padTipsScreenView.SetShowTipButtonState(true);
        });

        public void WaitUntilTaskSkipping() => _ = MakeCountdownToActionButtonUnlockingAsync(timeToSkipTaskInSeconds,
        () =>
        {
            padTipsScreenView.SetSkipTaskButtonState(false);
        },
        ((int minutes, int seconds) countdownCurrentTime) =>
        {
            padTipsScreenView.SetSkipTaskButtonLabelTextWithTimer("Game UI (Pad)", "Skip Task Waiting Label", string.Format("{0:d2}:{1:d2}", countdownCurrentTime.minutes, countdownCurrentTime.seconds));
        },
        () =>
        {
            padTipsScreenView.SetSkipTaskButtonState(true);
            padTipsScreenView.SetSkipTaskButtonLabelText("Game UI (Pad)", "Skip Task Label");
        });

        private async UniTask MakeCountdownToActionButtonUnlockingAsync(int countdownTimeInSeconds, Action beforeCountdownAction, Action<(int minutes, int seconds)> duringCountdownAction, Action afterCountdownAction)
        {
            beforeCountdownAction();
            var timer = countdownTimeInSeconds;
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
