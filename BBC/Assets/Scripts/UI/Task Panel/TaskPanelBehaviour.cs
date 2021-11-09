using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Video;
using UnityEngine.Events;

namespace Scripts
{
    public class TaskPanelBehaviour : MonoBehaviour
    {
        [Header("Панель задания")]
        public GameObject TaskPanel;
        public Text TaskTitle;
        public Text TaskDescription;
        public Scrollbar TaskDescriptionScrollbar;
        [Tooltip("Кнопка для получения полной доп. информации о задании")]
        public Button TaskInfoButton;

        [Header("Панель обучения программированию")]
        [SerializeField] private GameObject codingTrainingPanel;
        [SerializeField] private Text trainingTheme;
        [SerializeField] private GameObject codingTrainingPages;
        [SerializeField] private Button previousPageButton;
        [SerializeField] private Button nextPageButton;
        [SerializeField] private GameObject textPagePrefab;
        [SerializeField] private GameObject textAndVideoPagePrefab;
        [Space]
        [SerializeField] private UnityEvent onTaskStarted;

        private GameManager gameManager;
        private UIManager uiManager;
        private PlayerBehaviour playerBehaviour;
        private int currentOpenedTrainingPage;

        public void StartNewTask()
        {
            var taskText = gameManager.TaskTexts[gameManager.CurrentTaskNumber - 1];
            TaskTitle.text = taskText.Title;
            TaskDescription.text = taskText.Description;
            CreateCodingTrainingPages();         
            OpenCodingTrainingPanel_Special();
            onTaskStarted.Invoke();
        }

        public void FinishTask() => StartCoroutine(FinishTask_COR());

        public void OpenCodingTrainingPanel() => StartCoroutine(OpenCodingTrainingPanel_COR());

        public void OpenCodingTrainingPanel_Special() => StartCoroutine(ShowCodingTrainingPanel_COR());

        public void CloseCodingTrainingPanel() => StartCoroutine(CloseCodingTrainingPanel_COR());

        public void ChangeCodingTrainingPage(bool shouldIncreasePageNumber)
        {
            codingTrainingPages.transform.GetChild(currentOpenedTrainingPage).gameObject.SetActive(false);
            codingTrainingPages.transform.GetChild(shouldIncreasePageNumber ? ++currentOpenedTrainingPage : --currentOpenedTrainingPage).gameObject.SetActive(true);
            nextPageButton.gameObject.SetActive(currentOpenedTrainingPage < codingTrainingPages.transform.childCount - 1);
            previousPageButton.gameObject.SetActive(currentOpenedTrainingPage > 0);
            trainingTheme.text = gameManager.CodingTrainingInfos[gameManager.CurrentTaskNumber][currentOpenedTrainingPage].Title;
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
            gameManager.CurrentScriptTrigger.MakeTransitionToNextTask();
        }

        private void CreateCodingTrainingPages()
        {
            for (var i = codingTrainingPages.transform.childCount; i > 0; i--)
                Destroy(codingTrainingPages.transform.GetChild(i - 1).gameObject);
            var codingTrainingInfo = gameManager.CodingTrainingInfos[gameManager.CurrentTaskNumber];
            for (var i = 0; i < codingTrainingInfo.Length; i++)
            {
                var prefab = codingTrainingInfo[i].VideoTitles == "" ? textPagePrefab : textAndVideoPagePrefab;
                var page = Instantiate(prefab, codingTrainingPages.transform);
                page.GetComponentInChildren<TMP_Text>().text = codingTrainingInfo[i].Info;
                page.GetComponentInChildren<Scrollbar>().value = 1;
                var videoPlayer = page.GetComponentInChildren<VideoPlayer>();
                if (videoPlayer != null)
                    videoPlayer.clip = Resources.Load<VideoClip>("Video/" + codingTrainingInfo[i].VideoTitles);
                page.SetActive(i == 0);
            }
            currentOpenedTrainingPage = 0;
            trainingTheme.text = codingTrainingInfo[0].Title;
            previousPageButton.gameObject.SetActive(false);
        }

        private IEnumerator ShowTaskPanel_COR()
        {
            TaskDescriptionScrollbar.value = 1;
            yield return StartCoroutine(PlayTimeline_COR(TaskPanel, "ShowTaskPanel"));
        }

        public IEnumerator HideTaskPanel_COR()
        {
            yield return StartCoroutine(PlayTimeline_COR(TaskPanel, "HideTaskPanel"));
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
            yield return StartCoroutine(HideTaskPanel_COR());
            yield return StartCoroutine(ShowCodingTrainingPanel_COR());
        }

        private IEnumerator CloseCodingTrainingPanel_COR()
        {
            yield return StartCoroutine(HideCodingTrainingPanel_COR());
            yield return StartCoroutine(ShowTaskPanel_COR());
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
        }

        private void Start()
        {
            gameManager = GameManager.Instance;
            uiManager = UIManager.Instance;
        }  
    }
}
