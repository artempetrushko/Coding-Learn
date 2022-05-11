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
        [Header("������� (������ ���������)")]
        [Tooltip("������ ���������")]
        public GameObject HelpPanel;
        [Tooltip("������ ������ ���������")]
        public Button ShowTipButton;
        [Tooltip("���� � ������� ���������")]
        public TMP_Text Tip;
        [Tooltip("����� � ��������")]
        public Text TimerText;
        [Tooltip("�����-������ (��������� ����� ���������, ���� ��� ����������)")]
        public Text TipFiller;

        private GameManager gameManager;
        private int timeToNextTip;

        public void WaitUntilNextTip() => StartCoroutine(WaitUntilNextTip_COR());

        public void ShowTip()
        {
            if (TipFiller.IsActive())
                TipFiller.gameObject.SetActive(false);
            Tip.text = gameManager.GetNewTipText();
            if (gameManager.AvailableTipsData[gameManager.GetCurrentTaskNumber() - 1].Amount > 0)
                WaitUntilNextTip();
        }

        public void OpenHelpPanel()
        {
            var taskNumber = gameManager.GetCurrentTaskNumber();
            if (gameManager.AvailableTipsData[taskNumber - 1].Amount != ResourcesData.Tips[taskNumber - 1].Length)
            {
                var tipNumber = ResourcesData.Tips[taskNumber - 1].Length - gameManager.AvailableTipsData[taskNumber - 1].Amount - 1;
                Tip.text = ResourcesData.Tips[taskNumber - 1][tipNumber].Tip;
            }
            else Tip.text = "";
            HelpPanel.GetComponent<Animator>().Play("ScaleUp");
        }

        public void CloseHelpPanel() => HelpPanel.GetComponent<Animator>().Play("ScaleDown");

        private IEnumerator WaitUntilNextTip_COR()
        {
            ShowTipButton.interactable = false;
            while (timeToNextTip > 0)
            {
                var minutes = timeToNextTip / 60;
                var seconds = timeToNextTip - minutes * 60;
                TimerText.text = string.Format("��������� ��������� ����� {0:d2}:{1:d2}", minutes, seconds);
                yield return new WaitForSeconds(1f);
                timeToNextTip--;
            }
            TimerText.text = "�������� ���������";
            ShowTipButton.interactable = true;
        }

        private void Start()
        {
            gameManager = GameManager.Instance;
            timeToNextTip = gameManager.GetTimeToNextTip();
        }
    }
}
