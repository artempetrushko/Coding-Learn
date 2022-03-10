using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Video;
using UnityEngine.Events;
using System.Linq;

namespace Scripts
{
    public class TaskPanelBehaviour : MonoBehaviour
    {
        [Header("Панель задания")]
        [SerializeField] private Text taskTitle;
        [SerializeField] private Text taskDescription;
        [SerializeField] private Scrollbar taskDescriptionScrollbar;
        [Header("Панель обучения программированию")]
        [SerializeField] private GameObject codingTrainingPanel;
        [SerializeField] private Text trainingTheme;
        [SerializeField] private GameObject codingTrainingPages;
        [SerializeField] private Button previousPageButton;
        [SerializeField] private Button nextPageButton;
        [SerializeField] private GameObject textPagePrefab;
        [SerializeField] private GameObject textAndVideoPagePrefab;
        [Header("Панель награждения")]
        [SerializeField] private GameObject rewardingPanel;
        [SerializeField] private GameObject challengesContainer;
        [SerializeField] private Button closeRewardingPanelButton;
        [SerializeField] private GameObject challengePrefab;
        [Space]
        [SerializeField] private UnityEvent onTaskStarted;
        [SerializeField] private UnityEvent onLevelFinished;

        private GameManager gameManager;
        private UIManager uiManager;
        private PlayerBehaviour playerBehaviour;
        private int currentTrainingPageNumber;
        private GameManager.CodingTrainingInfo[] selectedCodingTrainingInfo;

        public void StartNewTask()
        {
            StartCoroutine(gameManager.StartTimer_COR());
            var taskText = gameManager.GetCurrentTask();
            taskTitle.text = taskText.Title;
            taskDescription.text = taskText.Description;
            CreateCodingTrainingPages(gameManager.SceneIndex, gameManager.CurrentTaskNumber - 1);         
            OpenCodingTrainingPanel_Special();
            onTaskStarted.Invoke();
        }

        public void FinishTask() => StartCoroutine(FinishTask_COR());

        public void GoToNextTask() => StartCoroutine(GoToNextTask_COR());

        public void GoToNextLevel() => StartCoroutine(GoToNextLevel_COR());

        public void OpenCodingTrainingPanel() => StartCoroutine(OpenCodingTrainingPanel_COR());

        public void OpenCodingTrainingPanel_Special() => StartCoroutine(ShowCodingTrainingPanel_COR());

        public void CloseCodingTrainingPanel() => StartCoroutine(CloseCodingTrainingPanel_COR());

        public void ChangeCodingTrainingPage(int coefficient)
        {
            codingTrainingPages.transform.GetChild(currentTrainingPageNumber).gameObject.SetActive(false);
            currentTrainingPageNumber += coefficient;
            codingTrainingPages.transform.GetChild(currentTrainingPageNumber).gameObject.SetActive(true);
            nextPageButton.gameObject.SetActive(currentTrainingPageNumber < codingTrainingPages.transform.childCount - 1);
            previousPageButton.gameObject.SetActive(currentTrainingPageNumber > 0);
            trainingTheme.text = selectedCodingTrainingInfo[currentTrainingPageNumber].Title;
        }

        public void ShowCodingTrainingOnSelectedTheme(int themeNumber, int subThemeNumber)
        {
            CreateCodingTrainingPages(themeNumber, subThemeNumber);
            OpenCodingTrainingPanel();
        }

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
            uiManager.PadMenuBehaviour.ShowIDEButton.interactable = false;
            uiManager.ActionButtonBehaviour.IsPressed = false;
            uiManager.PadMode = PadMode.Normal;
        }

        private IEnumerator FinishTask_COR()
        {
            yield return StartCoroutine(HideTaskPanel_COR());
            yield return StartCoroutine(CheckChallengesCompleting_COR());            
        }

        private IEnumerator GoToNextTask_COR()
        {
            closeRewardingPanelButton.gameObject.SetActive(false);
            for (var i = challengesContainer.transform.childCount - 1; i >= 0; i--)
                Destroy(challengesContainer.transform.GetChild(i).gameObject);
            yield return StartCoroutine(PlayTimeline_COR(rewardingPanel, "HideRewardingPanel"));
            rewardingPanel.SetActive(false);
            uiManager.BlackScreen.SetActive(true);
            yield return StartCoroutine(PlayAnimation_COR(uiManager.BlackScreen, "AppearBlackScreen"));
            gameManager.PlayNextCutscene();
        }

        private IEnumerator GoToNextLevel_COR()
        {
            onLevelFinished.Invoke();
            yield break;
        }

        private void CreateCodingTrainingPages(int levelNumber, int taskNumber)
        {
            for (var i = codingTrainingPages.transform.childCount - 1; i >= 0; i--)
                Destroy(codingTrainingPages.transform.GetChild(i).gameObject);
            selectedCodingTrainingInfo = gameManager.GetCodingTrainingInfo(levelNumber, taskNumber);
            for (var i = 0; i < selectedCodingTrainingInfo.Length; i++)
            {
                var prefab = selectedCodingTrainingInfo[i].VideoTitles == "" ? textPagePrefab : textAndVideoPagePrefab;
                var page = Instantiate(prefab, codingTrainingPages.transform);
                page.GetComponentInChildren<TMP_Text>().text = selectedCodingTrainingInfo[i].Info;
                page.GetComponentInChildren<Scrollbar>().value = 1;
                var videoPlayer = page.GetComponentInChildren<VideoPlayer>();
                if (videoPlayer != null)
                    videoPlayer.clip = Resources.Load<VideoClip>("Video/" + selectedCodingTrainingInfo[i].VideoTitles);
                page.SetActive(i == 0);
            }
            currentTrainingPageNumber = 0;
            trainingTheme.text = selectedCodingTrainingInfo[0].Title;
            previousPageButton.gameObject.SetActive(false);
            nextPageButton.gameObject.SetActive(selectedCodingTrainingInfo.Length > 1);
        }

        private IEnumerator CheckChallengesCompleting_COR()
        {
            rewardingPanel.SetActive(true);
            yield return StartCoroutine(PlayTimeline_COR(rewardingPanel, "ShowRewardingPanel"));
            var challenges = gameManager.TaskChallenges[gameManager.CurrentTaskNumber - 1];
            for (var i = 0; i < challenges.Length; i++)
            {
                var challenge = Instantiate(challengePrefab, challengesContainer.transform);
                challenge.GetComponentInChildren<TMP_Text>().text = challenges[i].Challenge;
                if (IsChallengeCompleting(challenges[i].CheckValue))
                {
                    challenge.GetComponentInChildren<TMP_Text>().color = Color.green;
                    yield return new WaitForSeconds(0.5f);
                    yield return StartCoroutine(PlayAnimation_COR(challenge.GetComponentInChildren<Animator>().gameObject, "AppearStar"));
                }
            }
            closeRewardingPanelButton.gameObject.SetActive(true);
        }

        private bool IsChallengeCompleting(double checkValue)
        {
            switch (checkValue)
            {
                case 1:
                    return true;
                case 0:
                    return !gameManager.AvailableTipsData[gameManager.CurrentTaskNumber - 1].IsShown;
                case 120:
                    return gameManager.SpentTime <= checkValue;
                default:
                    return false;
            }
        }

        private IEnumerator ShowTaskPanel_COR()
        {
            taskDescriptionScrollbar.value = 1;
            yield return StartCoroutine(PlayTimeline_COR(gameObject, "ShowTaskPanel"));
        }

        public IEnumerator HideTaskPanel_COR()
        {
            yield return StartCoroutine(PlayTimeline_COR(gameObject, "HideTaskPanel"));
        }

        private IEnumerator ShowCodingTrainingPanel_COR()
        {
            yield return StartCoroutine(PlayTimeline_COR(codingTrainingPanel, "ShowCodingTrainingPanel"));
        }

        private IEnumerator HideCodingTrainingPanel_COR()
        {
            yield return StartCoroutine(PlayTimeline_COR(codingTrainingPanel, "HideCodingTrainingPanel"));
        }

        private IEnumerator OpenCodingTrainingPanel_COR()
        {
            if (currentTrainingPageNumber != 0)
                ChangeCodingTrainingPage(-currentTrainingPageNumber);
            yield return StartCoroutine(HideTaskPanel_COR());
            yield return StartCoroutine(ShowCodingTrainingPanel_COR());
        }

        private IEnumerator CloseCodingTrainingPanel_COR()
        {
            if (uiManager.PadMode == PadMode.HandbookProgrammingInfo)
                uiManager.PadMode = PadMode.HandbookSubThemes;
            yield return StartCoroutine(HideCodingTrainingPanel_COR());
            yield return StartCoroutine(ShowTaskPanel_COR());
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
                playableDirector.playableAsset = Resources.Load<PlayableAsset>("Timelines/UI/Task Panel/" + playableAssetName);
            playableDirector.Play();
            yield return new WaitForSeconds((float)playableDirector.playableAsset.duration);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.RightControl))
                StartNewTask();
            else if (Input.GetKeyDown(KeyCode.RightAlt))
                StartCoroutine(FinishTask_COR());
        }

        private void Start()
        {
            gameManager = GameManager.Instance;
            uiManager = UIManager.Instance;
        }  
    }
}
