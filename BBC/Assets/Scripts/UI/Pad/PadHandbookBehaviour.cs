using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

namespace Scripts
{
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

        private int currentThemeNumber;
        private GameManager gameManager;
        private UIManager uiManager;

        public void OpenHandbook() => StartCoroutine(OpenHandbook_COR());

        public void CloseHandbook() => StartCoroutine(CloseHandbook_COR());

        public void OpenSubThemesList(int mainThemeNumber) => StartCoroutine(OpenSubThemesList_COR(mainThemeNumber));

        public void OpenSubTheme(int subThemeNumber) => StartCoroutine(OpenSubTheme_COR(subThemeNumber));

        public void ReturnToPreviousPage() => StartCoroutine(ReturnToPreviousPage_COR());

        public void UnlockProgrammingInfo(int chapterNumber)
        {
            var buttonToUnlock = SubThemeButtons.transform.GetChild(gameManager.AvailableThemesCount - 1).GetChild(chapterNumber - 1).gameObject.GetComponent<Button>();
            buttonToUnlock.interactable = true;
            buttonToUnlock.GetComponentInChildren<Text>().text = gameManager.HandbookLetters[gameManager.AvailableThemesCount - 1][chapterNumber - 1].Title;
        }

        private IEnumerator OpenHandbook_COR()
        {
            uiManager.PadMode = PadMode.HandbookMainThemes;
            PreviousHandbookPageButton.transform.parent.gameObject.SetActive(false);           
            if (ThemeButtonsContainer.GetComponentInChildren<Scrollbar>() != null)
                ThemeButtonsContainer.GetComponentInChildren<Scrollbar>().value = 1;
            yield return StartCoroutine(PlayTimeline_COR(Pad, "OpenHandbook"));       
            ThemeButtonsContainer.GetComponent<Animator>().Play("MoveButtons_LeftToMiddle");
        }

        private IEnumerator CloseHandbook_COR()
        {
            uiManager.PadMode = PadMode.Normal;
            yield return StartCoroutine(PlayTimeline_COR(Pad, "CloseHandbook"));           
            HandbookButtons.SetActive(true);
            for (var i = 0; i < SubThemeButtons.transform.childCount; i++)
                SubThemeButtons.transform.GetChild(i).gameObject.SetActive(false);
            InfoPanel_BlackScreen.SetActive(true);
            InfoPanel_BlackScreen.GetComponent<Animator>().Play("HideProgrammingInfo");   
        }

        private IEnumerator OpenSubThemesList_COR(int themeNumber)
        {
            currentThemeNumber = themeNumber;
            yield return StartCoroutine(PlayAnimation_COR(ThemeButtonsContainer, "MoveButtons_MiddleToLeft"));
            PreviousHandbookPageButton.transform.parent.gameObject.SetActive(true);
            yield return StartCoroutine(MoveSubThemeButtons_COR(true, "MoveButtons_RightToMiddle"));
            uiManager.PadMode = PadMode.HandbookSubThemes;
        }

        private IEnumerator OpenSubTheme_COR(int subThemeNumber)
        {
            uiManager.PadMode = PadMode.HandbookProgrammingInfo;
            var handbookLetter = gameManager.HandbookLetters[currentThemeNumber - 1][subThemeNumber - 1];
            ProgrammingInfo.text = handbookLetter.Description;
            ProgrammingInfoTitle.text = handbookLetter.Title;
            yield return StartCoroutine(MoveSubThemeButtons_COR(false, "MoveButtons_MiddleToLeft"));
            HandbookButtons.SetActive(false);
            ProgrammingInfoScrollBar.value = 1;
            yield return StartCoroutine(PlayAnimation_COR(InfoPanel_BlackScreen, "ShowProgrammingInfo"));
            InfoPanel_BlackScreen.SetActive(false);           
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
                    yield return StartCoroutine(PlayAnimation_COR(InfoPanel_BlackScreen, "HideProgrammingInfo"));
                    HandbookButtons.SetActive(true);
                    yield return StartCoroutine(MoveSubThemeButtons_COR(true, "MoveButtons_LeftToMiddle"));
                    uiManager.PadMode = PadMode.HandbookSubThemes;
                    break;
            }
        }

        private IEnumerator MoveSubThemeButtons_COR(bool willBeShown, string animation)
        {
            var subThemesList = SubThemeButtons.transform.GetChild(currentThemeNumber - 1).gameObject;
            if (willBeShown)
            {
                subThemesList.SetActive(true);
                yield return StartCoroutine(PlayAnimation_COR(subThemesList, animation));
            }
            else
            {
                yield return StartCoroutine(PlayAnimation_COR(subThemesList, animation));
                subThemesList.SetActive(false);
            }
        }

        private IEnumerator PlayAnimation_COR(GameObject animatorHolder, string animationName)
        {
            var animator = animatorHolder.GetComponent<Animator>();
            var clip = animator.runtimeAnimatorController.animationClips.Where(x => x.name == animationName).First();
            animator.Play(clip.name);
            yield return new WaitForSeconds(clip.length);
        }

        private IEnumerator PlayTimeline_COR(GameObject director, string playableAssetName = null)
        {
            var playableDirector = director.GetComponent<PlayableDirector>();
            if (playableAssetName != null)
                playableDirector.playableAsset = Resources.Load<PlayableAsset>("Timelines/UI/Pad/" + playableAssetName);
            playableDirector.Play();
            yield return new WaitForSeconds((float)playableDirector.playableAsset.duration);
        }

        private void Start()
        {
            gameManager = GameManager.Instance;
            uiManager = UIManager.Instance;
            FillHandbook();
            Debug.Log("Словарь обновлён!");
        }
    }
}
