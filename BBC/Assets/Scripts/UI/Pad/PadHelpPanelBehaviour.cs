using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PadHelpPanelBehaviour : MonoBehaviour
{
    [Header("������� (������ ���������)")]
    [Tooltip("������ ���������")]
    public GameObject HelpPanel;
    [Tooltip("������ ������ ���������")]
    public Button ShowTipButton;
    [Tooltip("������ ������� ���������")]
    public Button BuyTipButton;
    [Tooltip("������ ������� ���������� ���������")]
    public Button BuyManyTipsButton;
    [Tooltip("���� � ������� ���������")]
    public Text Tip;
    [Tooltip("������� ���������")]
    public Text TipsCounter;
    [Tooltip("������� �����")]
    public Text CoinsCounter;
    [Tooltip("�����-������ (��������� ����� ���������, ���� ��� ����������)")]
    public Text TipFiller;

    [Space] public UnityEvent OnTipsCountChanged;

    private GameManager gameManager;

    public void UpdateHelpPanelData()
    {
        var taskNumber = gameManager.CurrentTaskNumber;
        CoinsCounter.text = gameManager.CoinsCount.ToString();
        TipsCounter.text = gameManager.TipsCount.ToString();
        BuyTipButton.interactable = gameManager.CoinsCount >= gameManager.TipPrice;
        //BuyManyTipsButton.interactable = GameManager.Instance.CoinsCount >= ManyTipsPrice;
        if (gameManager.SceneIndex > 0 && taskNumber > 0 && taskNumber < gameManager.AvailableTipsCounts.Count)
            ShowTipButton.GetComponentInChildren<Text>().text = "�������� ��������� (��������: " + gameManager.AvailableTipsCounts[taskNumber - 1] + ")";
        ShowTipButton.interactable = gameManager.TipsCount > 0 && gameManager.AvailableTipsCounts[taskNumber - 1] > 0;
    }

    public void ShowTip()
    {       
        if (TipFiller.IsActive())
            TipFiller.gameObject.SetActive(false);
        Tip.text = gameManager.GetNewTipText();
        OnTipsCountChanged.Invoke();
    }

    public void BuyTips(int tipsAmount)
    {
        gameManager.BuyTips(tipsAmount);
        OnTipsCountChanged.Invoke();
    }

    public void ShowHelpPanel()
    {
        OnTipsCountChanged.Invoke();
        var taskNumber = gameManager.CurrentTaskNumber;   
        if (gameManager.AvailableTipsCounts[taskNumber - 1] == gameManager.Tips[taskNumber - 1].Length)
            Tip.text = "";
        else
        {
            var tipNumber = gameManager.Tips[taskNumber - 1].Length - gameManager.AvailableTipsCounts[taskNumber - 1] - 1;
            Tip.text = gameManager.Tips[taskNumber - 1][tipNumber].Tip;
        }
        HelpPanel.GetComponent<Animator>().Play("ScaleUp");
    }

    public void CloseHelpPanel() => HelpPanel.GetComponent<Animator>().Play("ScaleDown");

    private void Start()
    {
        gameManager = GameManager.Instance;
    }
}
