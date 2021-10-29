using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TrainingPanelBehaviour : MonoBehaviour
{
    [Header("Панель обучения")]
    public GameObject TrainingPanel;
    public GameObject TrainingPanelBackground;
    [Space]
    public UnityEvent OnLevelStarted;

    [Header("Элементы UI для переключения состояния")]
    [SerializeField] private PadDevelopmentBehaviour Pad_Dev;

    private GameManager gameManager;
    private UIManager uiManager;

    public void ShowFirstTip(GameObject tip)
    {
        gameManager.Player.GetComponent<PlayerBehaviour>().FreezePlayer();
        TrainingPanel.SetActive(true);
        TrainingPanelBackground.SetActive(true);
        tip.SetActive(true);
        tip.GetComponent<TipBehaviour>().ShowTip();
    }

    public void CloseTraining()
    {
        TrainingPanel.SetActive(false);
        TrainingPanelBackground.SetActive(false);
        gameManager.Player.GetComponent<PlayerBehaviour>().UnfreezePlayer();
    }

    #region Методы для события OnTipEnable
    public void EnableTip_Level_Training_Tip_1()
    {
        uiManager.TaskPanelBehaviour.TaskInfoButton.interactable = false;
        uiManager.PadMenuBehaviour.ShowIDEButton.interactable = false;
        uiManager.PadMenuBehaviour.ShowHandbookButton.interactable = false;
    }

    public void EnableTip_Level_Training_Tip_5()
    {
        Pad_Dev.StartButton.interactable = false;
        Pad_Dev.ResetButton.interactable = false;
        Pad_Dev.TipButton.gameObject.SetActive(true);
        Pad_Dev.TipButton.interactable = false;
        Pad_Dev.ExitDevModeButton.interactable = false;
    }

    public void EnableTip_Level_1_Tip_11()
    {
        gameManager.Player.GetComponent<PlayerBehaviour>().FreezePlayer();
        
    }

    public void EnableTip_Level_1_Tip_14()
    {
        
    }
    #endregion

    #region Методы для события OnTipDisable
    public void DisableTip_Level_Training_Tip_4()
    {
        uiManager.TaskPanelBehaviour.TaskInfoButton.interactable = true;
        uiManager.PadMenuBehaviour.ShowIDEButton.interactable = true;
        uiManager.PadMenuBehaviour.ShowHandbookButton.interactable = true;
    }

    public void DisableTip_Level_1_Tip_10()
    {
        Pad_Dev.StartButton.interactable = true;
        Pad_Dev.ResetButton.interactable = true;
        Pad_Dev.TipButton.gameObject.SetActive(false);
        Pad_Dev.TipButton.interactable = true;
        Pad_Dev.ExitDevModeButton.interactable = true;
    }
    #endregion

    private void Start()
    {
        gameManager = GameManager.Instance;
        uiManager = UIManager.Instance;
        OnLevelStarted.Invoke();
    }
}
