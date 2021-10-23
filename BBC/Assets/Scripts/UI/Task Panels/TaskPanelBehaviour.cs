using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class TaskPanelBehaviour : MonoBehaviour
{
    [Header("Панель задания")]
    [Tooltip("Панель задания")]
    public GameObject TaskPanel;
    [Tooltip("Название задания")]
    public Text TaskTitle;
    [Tooltip("Описание задания")]
    public Text TaskDescription;
    [Tooltip("Скроллбар для прокрутки задания")]
    public Scrollbar TaskDescriptionScrollbar;
    [Tooltip("Кнопка для получения полной доп. информации о задании")]
    public Button TaskInfoButton;
    [Tooltip("Кнопка для закрытия задания")]
    public Button CloseTaskButton;
    [Header("Задний фон UI-элементов")]
    public GameObject TaskPanelBackground;
    public GameObject PadBackground;

    private GameManager gameManager;
    private UIManager uiManager;
    private PlayerBehaviour playerBehaviour;

    public void ShowNewTaskGeneralInfo()
    {
        var taskText = gameManager.TaskTexts[gameManager.CurrentTaskNumber - 1];
        TaskTitle.text = taskText.Title;
        TaskDescription.text = taskText.Description;
    }

    public void CloseTask() => StartCoroutine(CloseTask_COR());

    public void HideCloseTaskButton() => CloseTaskButton.transform.localScale = new Vector3(0, 0, 0);

    public IEnumerator ReturnToScene_COR()
    {
        gameManager.IsTaskStarted = false;
        gameManager.CurrentSceneCamera.GetComponent<PlayableDirector>().playableAsset = Resources.Load<PlayableAsset>("Timelines/Tasks/Level " + gameManager.SceneIndex + "/ReturnToScene_Task_" + gameManager.CurrentTaskNumber);
        gameManager.CurrentSceneCamera.GetComponent<PlayableDirector>().Play();
        yield return new WaitForSeconds(2f);
        var isTaskCompleted = gameManager.HasTasksCompleted[gameManager.CurrentTaskNumber - 1];
        if (!isTaskCompleted)
        {
            var activatedTrigger = uiManager.ActionButtonBehaviour.ActivatedTrigger.gameObject;
            gameManager.Player.GetComponentInChildren<TriggersBehaviour>().ActivateTrigger_Any(activatedTrigger);
            StartCoroutine(uiManager.ActionButtonBehaviour.ShowActionButton_COR());
        }
        playerBehaviour.UnfreezePlayer();
        uiManager.Minimap.SetActive(true);
        uiManager.PadMenuBehaviour.ShowIDEButton.interactable = false;
        uiManager.ChangeCallAvailability(true);
        uiManager.ActionButtonBehaviour.IsPressed = false;
        uiManager.PadMode = PadMode.Normal;
    }

    public IEnumerator ShowTaskPanel_COR()
    {
        TaskDescriptionScrollbar.value = 1;
        yield return StartCoroutine(DrawTaskPanelBackground_COR());
        TaskPanel.GetComponent<Animator>().Play("MoveRight_TaskPanel");
        uiManager.PadMenuBehaviour.PlayPadMoveAnimation("ShowPad", "ShowPad_DevMode");
        yield return new WaitForSeconds(0.7f);
        CloseTaskButton.transform.localScale = new Vector3(1, 1, 1);
    }

    public IEnumerator HideTaskPanel_COR()
    {
        CloseTaskButton.transform.localScale = new Vector3(0, 0, 0);
        TaskPanel.GetComponent<Animator>().Play("MoveLeft_TaskPanel");
        uiManager.PadMenuBehaviour.PlayPadMoveAnimation("HidePad", "HidePad_DevMode");
        yield return new WaitForSeconds(0.7f);
        //HelpPanel.GetComponent<Animator>().Play("ScaleDown_Quick");
        yield return StartCoroutine(EraseTaskPanelBackground_COR());
    }

    public IEnumerator DrawTaskPanelBackground_COR()
    {
        TaskPanelBackground.transform.GetChild(0).GetComponent<Animator>().Play("DrawBackground");
        PadBackground.transform.GetChild(0).GetComponent<Animator>().Play("DrawBackground");
        yield return new WaitForSeconds(0.15f);
        TaskPanelBackground.transform.GetChild(1).GetComponent<Animator>().Play("DrawBackground");
        PadBackground.transform.GetChild(1).GetComponent<Animator>().Play("DrawBackground");
        TaskPanelBackground.transform.GetChild(2).GetComponent<Animator>().Play("DrawBackground");
        yield return new WaitForSeconds(0.15f);
    }

    public IEnumerator EraseTaskPanelBackground_COR()
    {
        PadBackground.transform.GetChild(1).GetComponent<Animator>().Play("EraseBackground");
        TaskPanelBackground.transform.GetChild(1).GetComponent<Animator>().Play("EraseBackground");
        TaskPanelBackground.transform.GetChild(2).GetComponent<Animator>().Play("EraseBackground");
        yield return new WaitForSeconds(0.15f);
        TaskPanelBackground.transform.GetChild(0).GetComponent<Animator>().Play("EraseBackground");
        PadBackground.transform.GetChild(0).GetComponent<Animator>().Play("EraseBackground");
        yield return new WaitForSeconds(0.15f);
    }

    private IEnumerator CloseTask_COR()
    {     
        yield return StartCoroutine(HideTaskPanel_COR());
        if (gameManager.CurrentInteractivePuzzle == null)
            yield return StartCoroutine(ReturnToScene_COR());
        else gameManager.CurrentInteractivePuzzle.FinishPuzzle();
    }

    private void Awake()
    {
        gameManager = GameManager.Instance;
        uiManager = UIManager.Instance;
        playerBehaviour = gameManager.Player.GetComponent<PlayerBehaviour>();
        uiManager.ExtendedTaskPanelBehaviour.NextLevelButton.gameObject.SetActive(false);
        HideCloseTaskButton();
    }
}
