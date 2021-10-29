using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExtendedTaskPanelBehaviour : MonoBehaviour
{
    [Header("Панель задания (расширенная)")]
    [Tooltip("Панель задания (расширенная)")]
    public GameObject ExtendedTaskPanel;
    [Tooltip("Название задания (расширенное)")]
    public Text ExtendedTaskTitle;
    [Tooltip("Описание задания (расширенное)")]
    public Text ExtendedTaskDescription;
    [Tooltip("Скроллбар для прокрутки описания")]
    public Scrollbar ExtendedTaskDescriptionScrollbar;
    [Tooltip("Кнопка перехода на следующий уровень")]
    public Button NextLevelButton;
    [Tooltip("Кнопка для закрытия расширенного описания задания")]
    public Button CloseExtendedTaskButton;
    public GameObject ExtendedTaskPanelBackground;

    [HideInInspector] public bool IsTaskMessage;

    private UIManager uiManager;
    private GameObject blackScreen;
    private GameManager gameManager;
    private LoadLevel levelLoader;

    public void OpenTaskExtendedDescription() => StartCoroutine(OpenTaskExtendedDescription_COR());

    public void OpenTaskExtendedDescription_Special() => StartCoroutine(ShowExtendedTaskPanel_COR());

    public void CloseTaskExtendedDescription() => StartCoroutine(CloseTaskExtendedDescription_COR());

    public void GoToNextLevel() => StartCoroutine(GoToNextLevel_COR());   

    public void FinishLevel()
    {
        var finishMessage = gameManager.FinishMessages[gameManager.SceneIndex];
        CloseExtendedTaskButton.gameObject.SetActive(false);
        NextLevelButton.gameObject.SetActive(true);
        ExtendedTaskTitle.text = finishMessage.Title;
        ExtendedTaskDescription.text = finishMessage.Description;
        OpenTaskExtendedDescription_Special();     
    }

    public void ShowNewTaskExtendedInfo()
    {
        var taskText = gameManager.TaskTexts[gameManager.CurrentTaskNumber - 1];
        ExtendedTaskTitle.text = taskText.Title;
        ExtendedTaskDescription.text = taskText.ExtendedDescription;
        OpenTaskExtendedDescription_Special();
    }

    private IEnumerator OpenTaskExtendedDescription_COR()
    {  
        yield return StartCoroutine(uiManager.TaskPanelBehaviour.HideTaskPanel_COR());
        StartCoroutine(ShowExtendedTaskPanel_COR());
    }

    private IEnumerator CloseTaskExtendedDescription_COR()
    {
        yield return StartCoroutine(HideExtendedTaskPanel_COR());
        if (IsTaskMessage)
            yield return StartCoroutine(uiManager.TaskPanelBehaviour.ShowTaskPanel_COR());
        else
        {
            gameManager.Player.GetComponent<PlayerBehaviour>().UnfreezePlayer();
            IsTaskMessage = true;
        }
    }

    private IEnumerator GoToNextLevel_COR()
    {
        SaveManager.Save_NextLevel();
        yield return StartCoroutine(HideExtendedTaskPanel_COR());
        uiManager.BlackScreen.transform.localScale = new Vector3(1, 1, 1);
        blackScreen.GetComponent<Animator>().Play("AppearBlackScreen");
        yield return new WaitForSeconds(1.4f);
        if (gameManager.SceneIndex == 0)
            StartCoroutine(levelLoader.LoadLevelAsync_COR(2));
        StartCoroutine(levelLoader.LoadLevelAsync_COR(gameManager.SceneIndex + 1));
    }

    public IEnumerator ShowExtendedTaskPanel_COR()
    {
        ExtendedTaskDescriptionScrollbar.value = 1;
        ExtendedTaskPanelBackground.transform.GetChild(0).GetComponent<Animator>().Play("DrawBackground");
        yield return new WaitForSeconds(0.15f);
        ExtendedTaskPanelBackground.transform.GetChild(1).GetComponent<Animator>().Play("DrawBackground");
        yield return new WaitForSeconds(0.15f);
        ExtendedTaskPanel.GetComponent<Animator>().Play("MoveUp_TaskPanel_Extended");
        yield return new WaitForSeconds(0.7f);
    }

    public IEnumerator HideExtendedTaskPanel_COR()
    {
        ExtendedTaskPanel.GetComponent<Animator>().Play("MoveDown_TaskPanel_Extended");
        yield return new WaitForSeconds(0.7f);
        ExtendedTaskPanelBackground.transform.GetChild(1).GetComponent<Animator>().Play("EraseBackground");
        yield return new WaitForSeconds(0.15f);
        ExtendedTaskPanelBackground.transform.GetChild(0).GetComponent<Animator>().Play("EraseBackground");
        yield return new WaitForSeconds(0.15f);
    }

    private void Start()
    {
        uiManager = UIManager.Instance;
        gameManager = GameManager.Instance;
        levelLoader = uiManager.LoadScreen.GetComponent<LoadLevel>();
        blackScreen = uiManager.BlackScreen.transform.GetChild(0).gameObject;
        if (!IsTaskMessage && gameManager.SceneIndex != 0)
        {
            var startMessage = gameManager.StartMessages[gameManager.SceneIndex - 1];
            ExtendedTaskTitle.text = startMessage.Title;
            ExtendedTaskDescription.text = startMessage.Description;
            gameManager.Player.GetComponent<PlayerBehaviour>().FreezePlayer();
            OpenTaskExtendedDescription_Special();
        }      
    }
}
