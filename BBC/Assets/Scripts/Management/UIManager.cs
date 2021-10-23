using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance = null;

    [Header("Интерфейс")]
    public Canvas Canvas;

    [Header ("Кнопки")]
    [Tooltip ("Кнопка выполнения действия (активация задания, смена сцены и т.д.)")]
    public Button ActionButton;

    [Header("Мини-карта")]
    public GameObject Minimap;
    [Header("Чёрный экран (контейнер)")]
    public GameObject BlackScreen;
    [Header("Загрузочный экран")]
    public GameObject LoadScreen;
    [Header("Описание осматриваемых объектов")]
    public Text LookAtDescription;

    [Header("Скрипты UI-элементов")]
    [Tooltip("Скрипты UI-элементов для взаимодействия между собой")]
    public PadMenuBehaviour PadMenuBehaviour;
    public ExtendedTaskPanelBehaviour ExtendedTaskPanelBehaviour;
    public TargetPanelBehaviour TargetPanelBehaviour;
    public TaskPanelBehaviour TaskPanelBehaviour;
    public TrainingPanelBehaviour TrainingPanelBehaviour;
    public NoteReadingPanelBehaviour NoteReadingPanelBehaviour;
    public InventoryBehaviour InventoryBehaviour;

    [HideInInspector] public ActionButtonBehaviour ActionButtonBehaviour;
    [HideInInspector] public PadMode PadMode;
    [HideInInspector] public bool isExitToMenuAvailable = true;

    public void ChangeCallAvailability(bool isCallAvailable)
    {
        TargetPanelBehaviour.IsCallAvailable = isCallAvailable;
        PadMenuBehaviour.IsCallAvailable = isCallAvailable;
    }

    public void HideUI()
    {
        if (TargetPanelBehaviour.IsShown)
            TargetPanelBehaviour.HideTarget();
        TargetPanelBehaviour.IsCallAvailable = false;
        Minimap.SetActive(false);
    }

    public void ShowUI()
    {
        TargetPanelBehaviour.IsCallAvailable = true;
        Minimap.SetActive(true);
    }

    public IEnumerator MakeExitToMenuAvailable_COR()
    {
        yield return new WaitForSeconds(1.5f);
        isExitToMenuAvailable = true;
    }

    private void InitializeUiManager()
    {
        if (Instance == null)
            Instance = this;
        ActionButtonBehaviour = ActionButton.GetComponent<ActionButtonBehaviour>();
    }

    private void Awake()
    {
        InitializeUiManager();
        ChangeCallAvailability(false);
    }
}
