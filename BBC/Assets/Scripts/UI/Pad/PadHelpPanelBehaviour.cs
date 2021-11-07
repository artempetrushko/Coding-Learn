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
        [Tooltip("Панель подсказок")]
        public GameObject HelpPanel;
        [Tooltip("Кнопка показа подсказки")]
        public Button ShowTipButton;
        [Tooltip("Поле с текстом подсказки")]
        public TMP_Text Tip;
        [Tooltip("Текст с таймером")]
        public Text TimerText;
        [Tooltip("Текст-филлер (заполняет место подсказки, пока она недоступна)")]
        public Text TipFiller;

        private GameManager gameManager;

        public void WaitUntilNextTip() => StartCoroutine(WaitUntilNextTip_COR());

        public void ShowTip()
        {
            if (TipFiller.IsActive())
                TipFiller.gameObject.SetActive(false);
            Tip.text = gameManager.GetNewTipText();
            if (gameManager.AvailableTipsCounts[gameManager.CurrentTaskNumber - 1] > 0)
                WaitUntilNextTip();
        }

        public void OpenHelpPanel()
        {
            var taskNumber = gameManager.CurrentTaskNumber;
            if (gameManager.AvailableTipsCounts[taskNumber - 1] != gameManager.Tips[taskNumber - 1].Length)
            {
                var tipNumber = gameManager.Tips[taskNumber - 1].Length - gameManager.AvailableTipsCounts[taskNumber - 1] - 1;
                Tip.text = gameManager.Tips[taskNumber - 1][tipNumber].Tip;
            }
            else Tip.text = "";
            HelpPanel.GetComponent<Animator>().Play("ScaleUp");
        }

        public void CloseHelpPanel() => HelpPanel.GetComponent<Animator>().Play("ScaleDown");

        private IEnumerator WaitUntilNextTip_COR()
        {
            ShowTipButton.interactable = false;
            var timeToNextTip = gameManager.TimeToNextTip;
            while (timeToNextTip > 0)
            {
                var minutes = timeToNextTip / 60;
                var seconds = timeToNextTip - minutes * 60;
                TimerText.text = string.Format("Следующая подсказка через {0:d2}:{1:d2}", minutes, seconds);
                yield return new WaitForSeconds(1f);
                timeToNextTip--;
            }
            TimerText.text = "Получить подсказку";
            ShowTipButton.interactable = true;
        }

        private void Start()
        {
            gameManager = GameManager.Instance;
        }
    }
}
