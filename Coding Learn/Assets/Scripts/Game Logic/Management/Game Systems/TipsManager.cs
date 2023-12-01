using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts
{
    public class TipsManager : MonoBehaviour
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

        public void SetDefaultState(int currentTaskNumber)
        {
            currentTaskTips = GameContentManager.GetTaskInfo(currentTaskNumber).Tips;
            nextTipIndex = 0;
            padTipsScreenView.ClearTipText();
            WaitUntilNextTip();
            WaitUntilTaskSkipping();
        }

        public void ShowTipsScreen() => padTipsScreenView.ChangeVisibility(true);

        public void HideTipsScreen() => padTipsScreenView.ChangeVisibility(false);

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

        public void WaitUntilNextTip() => StartCoroutine(MakeCountdownToActionButtonUnlocking_COR(timeToNextTipInSeconds, 
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
        }));

        public void WaitUntilTaskSkipping() => StartCoroutine(MakeCountdownToActionButtonUnlocking_COR(timeToNextTipInSeconds,
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
        }));

        private IEnumerator MakeCountdownToActionButtonUnlocking_COR(int countdownTimeInSeconds, Action beforeCountdownAction, Action<(int minutes, int seconds)> duringCountdownAction, Action afterCountdownAction)
        {
            beforeCountdownAction();
            var timer = countdownTimeInSeconds;
            while (timer > 0)
            {
                var minutes = timer / 60;
                var seconds = timer - minutes * 60;
                duringCountdownAction((minutes, seconds));
                yield return new WaitForSeconds(1f);
                timer--;
            }
            afterCountdownAction();
        }
    }
}
