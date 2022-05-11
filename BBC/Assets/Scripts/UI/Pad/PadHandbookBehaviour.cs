using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.UI;

namespace Scripts
{
    public class PadHandbookBehaviour : MonoBehaviour
    {
        [Header("Планшет (режим справочника)")]
        [Tooltip("Контейнер для кнопок справочника")]
        [SerializeField] private GameObject handbookButtons;
        [Tooltip("Кнопки разделов по программированию")]
        [SerializeField] private GameObject themeButtonsContainer;
        [Tooltip("Кнопки подразделов для каждого раздела")]
        [SerializeField] private GameObject subThemeButtons;
        [Tooltip("Кнопка для перехода на предыдущую страницу")]
        [SerializeField] private Button previousHandbookPageButton;
        [Header("Префабы кнопок")]
        [Tooltip("Префаб кнопки темы")]
        [SerializeField] private GameObject themeButtonPrefab;
        [Tooltip("Префаб контейнера с кнопками подтем")]
        [SerializeField] private GameObject subThemeButtonsContainerPrefab;
        
        [Space]
        [SerializeField] private UnityEvent<int, int> onSubThemeButtonPressed;

        private int currentThemeNumber;
        private GameManager gameManager;
        private UIManager uiManager;

        public void OpenHandbook() => StartCoroutine(OpenHandbook_COR());

        public void CloseHandbook() => StartCoroutine(CloseHandbook_COR());

        public void OpenSubThemesList(int mainThemeNumber) => StartCoroutine(OpenSubThemesList_COR(mainThemeNumber));

        public void OpenSubTheme(int subThemeNumber)
        {
            uiManager.PadMode = PadMode.HandbookProgrammingInfo;
            onSubThemeButtonPressed.Invoke(currentThemeNumber, subThemeNumber);
        }

        public void ReturnToPreviousPage() => StartCoroutine(ReturnToPreviousPage_COR());

        public void UnlockProgrammingInfo()
        {
            for (var i = 1; i <= gameManager.GetCurrentTaskNumber(); i++)
            {
                var buttonToUnlock = subThemeButtons.transform.GetChild(gameManager.GetAvailableThemesCount() - 1).GetComponentInChildren<VerticalLayoutGroup>().transform.GetChild(i - 1).GetComponent<Button>();
                buttonToUnlock.interactable = true;
                buttonToUnlock.GetComponentInChildren<Text>().text = ResourcesData.GetCodingTrainingInfo(gameManager.GetAvailableThemesCount() - 1, i - 1)[0].Title;
            }
        }

        private IEnumerator OpenHandbook_COR()
        {
            uiManager.PadMode = PadMode.HandbookMainThemes;
            previousHandbookPageButton.transform.parent.gameObject.SetActive(false);           
            if (themeButtonsContainer.GetComponentInChildren<Scrollbar>() != null)
                themeButtonsContainer.GetComponentInChildren<Scrollbar>().value = 1;    
            yield return StartCoroutine(PlayAnimation_COR(gameObject, "ScaleUp"));
            themeButtonsContainer.GetComponent<Animator>().Play("MoveButtons_LeftToMiddle");
        }

        private IEnumerator CloseHandbook_COR()
        {
            uiManager.PadMode = PadMode.Normal;
            yield return StartCoroutine(PlayAnimation_COR(gameObject, "ScaleDown"));
            handbookButtons.SetActive(true);
            for (var i = 0; i < subThemeButtons.transform.childCount; i++)
                subThemeButtons.transform.GetChild(i).gameObject.SetActive(false);
        }

        private IEnumerator OpenSubThemesList_COR(int themeNumber)
        {
            currentThemeNumber = themeNumber;
            yield return StartCoroutine(PlayAnimation_COR(themeButtonsContainer, "MoveButtons_MiddleToLeft"));
            previousHandbookPageButton.transform.parent.gameObject.SetActive(true);
            yield return StartCoroutine(MoveSubThemeButtons_COR(true, "MoveButtons_RightToMiddle"));
            uiManager.PadMode = PadMode.HandbookSubThemes;
        }

        private void FillHandbook()
        {
            var themeButtons = themeButtonsContainer.transform.GetChild(0).GetChild(0);
            for (var i = 0; i < gameManager.GetAvailableThemesCount(); i++)
            {
                var themeButton = Instantiate(themeButtonPrefab, themeButtons);
                var themeNumber = i;
                themeButton.GetComponentInChildren<Text>().text = ResourcesData.ThemeTitles[i].Title;
                themeButton.GetComponent<Button>().onClick.AddListener(() => OpenSubThemesList(themeNumber));

                var subThemeContainer = Instantiate(subThemeButtonsContainerPrefab, subThemeButtons.transform);
                for (var j = 0; j < ResourcesData.CodingTrainingInfos[i].Count; j++)
                {
                    var subThemeButton = Instantiate(themeButtonPrefab, subThemeContainer.transform.GetChild(0).GetChild(0));
                    var subThemeNumber = j;
                    if (i == gameManager.GetAvailableThemesCount() - 1)
                    {
                        subThemeButton.GetComponentInChildren<Text>().text = "???";
                        subThemeButton.GetComponent<Button>().interactable = false;
                    }
                    else subThemeButton.GetComponentInChildren<Text>().text = ResourcesData.GetCodingTrainingInfo(themeNumber, subThemeNumber)[0].Title;
                    subThemeButton.GetComponent<Button>().onClick.AddListener(() => OpenSubTheme(subThemeNumber));
                }
                subThemeContainer.SetActive(false);
            }
        }

        private IEnumerator ReturnToPreviousPage_COR()
        {
            yield return StartCoroutine(MoveSubThemeButtons_COR(false, "MoveButtons_MiddleToRight"));
            themeButtonsContainer.GetComponent<Animator>().Play("MoveButtons_LeftToMiddle");
            previousHandbookPageButton.transform.parent.gameObject.SetActive(false);
            uiManager.PadMode = PadMode.HandbookMainThemes;
        }

        private IEnumerator MoveSubThemeButtons_COR(bool willBeShown, string animation)
        {
            var subThemesList = subThemeButtons.transform.GetChild(currentThemeNumber).gameObject;
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
