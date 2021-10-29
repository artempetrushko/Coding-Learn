using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PadHandbookBehaviour : MonoBehaviour
{
    [Header("Планшет (режим справочника)")]
    [Tooltip("Планшет")]
    public GameObject Pad;
    [Tooltip("Контейнер для кнопок справочника")]
    public GameObject HandbookButtons;
    [Tooltip("Кнопки разделов по программированию")]
    public GameObject ThemeButtonsContainer;
    [Tooltip("Кнопки подразделов для каждого раздела")]
    public GameObject SubThemeButtons;
    [Tooltip("Панель с информацией о программировании")]
    public GameObject InfoPanel;
    [Tooltip("Поле для информации по программрованию")]
    public Text ProgrammingInfo;
    [Tooltip("Заголовок для раздела по программрованию")]
    public Text ProgrammingInfoTitle;
    [Tooltip("Скроллбар раздела по программрованию")]
    public Scrollbar ProgrammingInfoScrollBar;
    [Tooltip("Кнопка для перехода на предыдущую страницу")]
    public Button PreviousHandbookPageButton;
    [Tooltip("Черный экран для анимированного перехода")]
    public GameObject InfoPanel_BlackScreen;
    [Header("Префабы кнопок")]
    [Tooltip("Префаб кнопки темы")]
    public GameObject ThemeButtonPrefab;
    [Tooltip("Префаб контейнера с кнопками подтем")]
    public GameObject SubThemeButtonsContainerPrefab;

    private int themeNumber;
    private GameManager gameManager;
    private UIManager uiManager;

    public void SwitchToHandbookMode() => StartCoroutine(SwitchToHandbookMode_COR());

    public void ReturnToMenuFromHandbookMode() => StartCoroutine(ReturnToMenuFromHandbookMode_COR());

    public void OpenSubThemesList(int mainThemeNumber) => StartCoroutine(OpenSubThemesList_COR(mainThemeNumber));

    public void OpenSubTheme(int subThemeNumber) => StartCoroutine(OpenSubTheme_COR(subThemeNumber));

    public void ReturnToPreviousPage() => StartCoroutine(ReturnToPreviousPage_COR());

    public void UnlockProgrammingInfo(int chapterNumber)
    {
        var buttonToUnlock = SubThemeButtons.transform.GetChild(gameManager.AvailableThemesCount - 1).GetChild(chapterNumber - 1).gameObject.GetComponent<Button>();
        buttonToUnlock.interactable = true;
        buttonToUnlock.GetComponentInChildren<Text>().text = gameManager.HandbookLetters[gameManager.AvailableThemesCount - 1][chapterNumber - 1].Title;
    }

    private IEnumerator SwitchToHandbookMode_COR()
    {
        uiManager.TaskPanelBehaviour.CloseTaskButton.transform.localScale = new Vector3(0, 0, 0);
        PreviousHandbookPageButton.transform.parent.gameObject.SetActive(false);
        if (gameManager.IsTaskStarted)
        {
            Pad.GetComponent<Animator>().Play("SwitchToHandbookMode");
            uiManager.TaskPanelBehaviour.TaskPanel.GetComponent<Animator>().Play("MoveLeft_TaskPanel");
            yield return new WaitForSeconds(0.7f);
            yield return StartCoroutine(uiManager.TaskPanelBehaviour.EraseTaskPanelBackground_COR());
        }
        else
        {
            Pad.GetComponent<Animator>().Play("SwitchToHandbookMode_NoLatency");
            yield return new WaitForSeconds(1.5f);
        }
        uiManager.PadMode = PadMode.HandbookMainThemes;
        if (ThemeButtonsContainer.GetComponentInChildren<Scrollbar>() != null)
            ThemeButtonsContainer.GetComponentInChildren<Scrollbar>().value = 1;
        ThemeButtonsContainer.GetComponent<Animator>().Play("MoveButtons_LeftToMiddle");
    }

    private IEnumerator ReturnToMenuFromHandbookMode_COR()
    {
        if (gameManager.IsTaskStarted)
            Pad.GetComponentInParent<Animator>().Play("ReturnToMenuFromHandbookMode");
        else Pad.GetComponentInParent<Animator>().Play("ReturnToMenuFromHandbookMode_NoLatency");
        yield return new WaitForSeconds(0.83f);
        HandbookButtons.SetActive(true);
        for (var i = 0; i < SubThemeButtons.transform.childCount; i++)
            SubThemeButtons.transform.GetChild(i).gameObject.SetActive(false);
        InfoPanel_BlackScreen.SetActive(true);
        InfoPanel_BlackScreen.GetComponent<Animator>().Play("HideProgrammingInfo");
        if (gameManager.IsTaskStarted)
        {
            yield return StartCoroutine(uiManager.TaskPanelBehaviour.DrawTaskPanelBackground_COR());
            uiManager.TaskPanelBehaviour.TaskPanel.GetComponent<Animator>().Play("MoveRight_TaskPanel");
            yield return new WaitForSeconds(0.7f);
            uiManager.TaskPanelBehaviour.CloseTaskButton.transform.localScale = new Vector3(1, 1, 1);
        }
        uiManager.PadMode = PadMode.Normal;
    }

    private IEnumerator OpenSubThemesList_COR(int themeNumber)
    {
        this.themeNumber = themeNumber;
        ThemeButtonsContainer.GetComponent<Animator>().Play("MoveButtons_MiddleToLeft");
        yield return new WaitForSeconds(0.583f);
        PreviousHandbookPageButton.transform.parent.gameObject.SetActive(true);
        yield return StartCoroutine(MoveSubThemeButtons_COR(true, "MoveButtons_RightToMiddle"));
        uiManager.PadMode = PadMode.HandbookSubThemes;
    }

    private IEnumerator OpenSubTheme_COR(int subThemeNumber)
    {
         var handbookLetter = gameManager.HandbookLetters[themeNumber - 1][subThemeNumber - 1];
         ProgrammingInfo.text = handbookLetter.Description;
         ProgrammingInfoTitle.text = handbookLetter.Title;
         yield return StartCoroutine(MoveSubThemeButtons_COR(false, "MoveButtons_MiddleToLeft"));
         HandbookButtons.SetActive(false);
         ProgrammingInfoScrollBar.value = 1;
         InfoPanel_BlackScreen.GetComponent<Animator>().Play("ShowProgrammingInfo");
         yield return new WaitForSeconds(1f);
         InfoPanel_BlackScreen.SetActive(false);
         uiManager.PadMode = PadMode.HandbookProgrammingInfo;
    }

    private void FillHandbook()
    {
        var themeButtons = ThemeButtonsContainer.transform.GetChild(0).GetChild(0);
        for (var i = 0; i < gameManager.AvailableThemesCount; i++)
        {
            var themeButton = Instantiate(ThemeButtonPrefab, themeButtons);
            var themeNumber = i + 1;
            themeButton.GetComponentInChildren<Text>().text = gameManager.ThemeTitles[i].Title;
            themeButton.GetComponent<Button>().onClick.AddListener(() => OpenSubThemesList(themeNumber));

            var subThemeContainer = Instantiate(SubThemeButtonsContainerPrefab, SubThemeButtons.transform);
            var parentPosition = SubThemeButtons.transform.position;
            for (var j = 0; j < gameManager.HandbookLetters[i].Length; j++)
            {
                var subThemeButton = Instantiate(ThemeButtonPrefab, subThemeContainer.transform.GetChild(0).GetChild(0));
                var subThemeNumber = j + 1;
                if (i == gameManager.AvailableThemesCount - 1)
                {
                    subThemeButton.GetComponentInChildren<Text>().text = "???";
                    subThemeButton.GetComponent<Button>().interactable = false;
                }
                else subThemeButton.GetComponentInChildren<Text>().text = gameManager.HandbookLetters[i][j].Title;
                subThemeButton.GetComponent<Button>().onClick.AddListener(() => OpenSubTheme(subThemeNumber));
            }
            subThemeContainer.SetActive(false);
        }
    }

    private IEnumerator ReturnToPreviousPage_COR()
    {
        switch (uiManager.PadMode)
        {
            case PadMode.HandbookSubThemes:
                yield return StartCoroutine(MoveSubThemeButtons_COR(false, "MoveButtons_MiddleToRight"));
                ThemeButtonsContainer.GetComponent<Animator>().Play("MoveButtons_LeftToMiddle");
                PreviousHandbookPageButton.transform.parent.gameObject.SetActive(false);
                uiManager.PadMode = PadMode.HandbookMainThemes;
                break;
            case PadMode.HandbookProgrammingInfo:
                InfoPanel_BlackScreen.SetActive(true);
                InfoPanel_BlackScreen.GetComponent<Animator>().Play("HideProgrammingInfo");
                yield return new WaitForSeconds(1f);
                HandbookButtons.SetActive(true);
                yield return StartCoroutine(MoveSubThemeButtons_COR(true, "MoveButtons_LeftToMiddle"));
                uiManager.PadMode = PadMode.HandbookSubThemes;
                break;
        }
    }

    private IEnumerator MoveSubThemeButtons_COR(bool willBeShown, string animation)
    {
        var subThemesList = SubThemeButtons.transform.GetChild(themeNumber - 1).gameObject;
        if (willBeShown)
        {
            subThemesList.SetActive(true);
            subThemesList.GetComponent<Animator>().Play(animation);
            yield return new WaitForSeconds(0.583f);
        }
        else
        {
            subThemesList.GetComponent<Animator>().Play(animation);
            yield return new WaitForSeconds(0.583f);
            subThemesList.SetActive(false);
        }
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
        uiManager = UIManager.Instance;
        FillHandbook();
        Debug.Log("Словарь обновлён!");
    }
}
