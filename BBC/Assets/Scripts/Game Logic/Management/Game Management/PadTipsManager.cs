using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    public class PadTipsManager : MonoBehaviour
    {
        [SerializeField]
        private PadTipsScreenView padTipsScreenView;

        private GameManager gameManager;
        private UiLocalizationScript uiLocalization;
        private int timeToNextTip;
        private int timeToSkipTask;

        public void WaitUntilNextTip() => StartCoroutine(WaitUntilNextTip_COR());

        public void WaitUntilTaskSkipping() => StartCoroutine(WaitUntilTaskSkipping_COR());

        public void ShowTip()
        {
            padTipsScreenView.AddNewTipText(gameManager.GetNewTipText());
            if (gameManager.GetCurrentTaskTipsData().Amount > 0)
            {
                WaitUntilNextTip();
            }
            else
            {
                padTipsScreenView.SetShowTipButtonState(false);
                padTipsScreenView.SetTipStatusText(uiLocalization.NoTipsText);
            }
        }

        private IEnumerator WaitUntilNextTip_COR()
        {
            padTipsScreenView.SetShowTipButtonState(false);
            var timer = timeToNextTip;
            while (timer > 0)
            {
                var minutes = timer / 60;
                var seconds = timer - minutes * 60;
                padTipsScreenView.SetTipStatusText(uiLocalization.TipWaitingText + string.Format(" {0:d2}:{1:d2}", minutes, seconds));
                yield return new WaitForSeconds(1f);
                timer--;
            }
            padTipsScreenView.SetTipStatusText(uiLocalization.TipReadyText);
            padTipsScreenView.SetShowTipButtonState(true);
        }

        private IEnumerator WaitUntilTaskSkipping_COR()
        {
            padTipsScreenView.SetSkipTaskButtonState(false);
            var timer = timeToSkipTask;
            while (timer > 0)
            {
                var minutes = timer / 60;
                var seconds = timer - minutes * 60;
                padTipsScreenView.SetSkipTaskButtonLabelText(uiLocalization.SkipTaskText + string.Format(" ({0:d2}:{1:d2})", minutes, seconds));
                yield return new WaitForSeconds(1f);
                timer--;
            }
            padTipsScreenView.SetSkipTaskButtonState(true);
            padTipsScreenView.SetSkipTaskButtonLabelText(uiLocalization.SkipTaskText);
        }

        private void Start()
        {
            gameManager = GameManager.Instance;
            uiLocalization = gameManager.GetComponent<UiLocalizationScript>();
            timeToNextTip = gameManager.GetTimeToNextTip();
            timeToSkipTask = gameManager.GetTimeToSkipTask();
        }
    }
}
