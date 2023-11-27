using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        public void ShowTip()
        {
            /*padTipsScreenView.AddNewTipText(gameManager.GetNewTipText());
            if (gameManager.GetCurrentTaskTipsData().Amount > 0)
            {
                WaitUntilNextTip();
            }
            else
            {
                padTipsScreenView.SetShowTipButtonState(false);
                padTipsScreenView.SetTipStatusText(uiLocalization.NoTipsText);
            }*/
        }

        public void WaitUntilNextTip() => StartCoroutine(MakeCountdownToActionButtonUnlocking_COR(timeToNextTipInSeconds, 
        () =>
        {
            padTipsScreenView.SetShowTipButtonState(false);
        },
        ((int minutes, int seconds) countdownCurrentTime) =>
        {
            //padTipsScreenView.SetTipStatusText(uiLocalization.TipWaitingText + string.Format(" {0:d2}:{1:d2}", countdownCurrentTime.minutes, countdownCurrentTime.seconds));
        },
        () =>
        {
            //padTipsScreenView.SetTipStatusText(uiLocalization.TipReadyText);
            padTipsScreenView.SetShowTipButtonState(true);
        }));

        public void WaitUntilTaskSkipping() => StartCoroutine(MakeCountdownToActionButtonUnlocking_COR(timeToNextTipInSeconds,
        () =>
        {
            padTipsScreenView.SetSkipTaskButtonState(false);
        },
        ((int minutes, int seconds) countdownCurrentTime) =>
        {
            //padTipsScreenView.SetSkipTaskButtonLabelText(uiLocalization.SkipTaskText + string.Format(" ({0:d2}:{1:d2})", countdownCurrentTime.minutes, countdownCurrentTime.seconds));
        },
        () =>
        {
            padTipsScreenView.SetSkipTaskButtonState(true);
            //padTipsScreenView.SetSkipTaskButtonLabelText(uiLocalization.SkipTaskText);
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
