using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

namespace Scripts
{
    public class PadHelpPanelBehaviour : MonoBehaviour
    {
        [Header("Планшет (панель подсказок)")]
        [SerializeField] private GameObject helpPanel;
        [SerializeField] private Button showTipButton;
        [SerializeField] private Button skipTaskButton;
        [SerializeField] private TMP_Text tipText;
        [SerializeField] private Text tipStatusText;
        [SerializeField] private Text tipFiller;

        private GameManager gameManager;
        private UiLocalizationScript uiLocalization;
        private int timeToNextTip;
        private int timeToSkipTask;

        public void WaitUntilNextTip() => StartCoroutine(WaitUntilNextTip_COR());

        public void WaitUntilTaskSkipping() => StartCoroutine(WaitUntilTaskSkipping_COR()); 

        public void ShowTip()
        {
            if (tipFiller.IsActive())
            {
                tipFiller.gameObject.SetActive(false);
            }            
            tipText.text += " - " + gameManager.GetNewTipText() + "\n";
            if (gameManager.GetCurrentTaskTipsData().Amount > 0)
            {
                WaitUntilNextTip();
            }
            else
            {
                showTipButton.interactable = false;
                tipStatusText.text = uiLocalization.NoTipsText;
            }
        }

        public void ClearTipText()
        {
            tipText.text = "";
            tipFiller.gameObject.SetActive(true);
        }

        public void OpenHelpPanel() => helpPanel.GetComponent<Animator>().Play("ScaleUp");

        public void CloseHelpPanel() => helpPanel.GetComponent<Animator>().Play("ScaleDown");

        private IEnumerator WaitUntilNextTip_COR()
        {
            showTipButton.interactable = false;
            var timer = timeToNextTip;
            while (timer > 0)
            {
                var minutes = timer / 60;
                var seconds = timer - minutes * 60;
                tipStatusText.text = uiLocalization.TipWaitingText + string.Format(" {0:d2}:{1:d2}", minutes, seconds);
                yield return new WaitForSeconds(1f);
                timer--;
            }
            tipStatusText.text = uiLocalization.TipReadyText;
            showTipButton.interactable = true;
        }

        private IEnumerator WaitUntilTaskSkipping_COR()
        {
            /*skipTaskButton.interactable = false;
            var timer = timeToSkipTask;
            while (timer > 0)
            {
                var minutes = timer / 60;
                var seconds = timer - minutes * 60;
                skipTaskButton.GetComponentInChildren<Text>().text = uiLocalization.SkipTaskText + string.Format(" ({0:d2}:{1:d2})", minutes, seconds);
                yield return new WaitForSeconds(1f);
                timer--;
            }
            skipTaskButton.GetComponentInChildren<Text>().text = uiLocalization.SkipTaskText;
            skipTaskButton.interactable = true;*/
            skipTaskButton.GetComponentInChildren<Text>().text = uiLocalization.SkipTaskText;
            yield break;
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
