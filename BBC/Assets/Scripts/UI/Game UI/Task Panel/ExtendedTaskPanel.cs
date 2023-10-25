using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

namespace Scripts
{
    public class ExtendedTaskPanel : MonoBehaviour
    {
        [SerializeField] private Text taskTitle;
        [SerializeField] private GameObject taskDescriptionContent_DefaultView;
        [SerializeField] private GameObject taskDescriptionContent_AlternateView;
        [SerializeField] private GameObject taskImagePrefab;
        [Space]
        [SerializeField] private UnityEvent onPanelClosed;

        private GameObject extendedTaskPanelContainer;
        private GameObject currentTaskDescriptionContentView;
        private GameManager gameManager;
        private PlayableDirector playableDirector;
        private int sceneIndex;

        public void ShowExtendedTaskDescription() => StartCoroutine(ShowExtendedTaskDescription_COR());

        public void HideExtendedTaskDescription() => StartCoroutine(HideExtendedTaskDescription_COR());

        private IEnumerator ShowExtendedTaskDescription_COR()
        {
            ChangeSiblingIndex(1);
            var taskData = ResourcesData.TaskTexts[sceneIndex - 1][gameManager.GetCurrentTaskNumber() - 1];
            taskTitle.text = taskData.Title;
            var hasImages = !string.IsNullOrEmpty(taskData.ImagesTitles);
            currentTaskDescriptionContentView = hasImages ? taskDescriptionContent_AlternateView : taskDescriptionContent_DefaultView;
            currentTaskDescriptionContentView.SetActive(true);
            currentTaskDescriptionContentView.GetComponentInChildren<Text>().text = taskData.Description;
            if (hasImages)
            {
               FillImageGallery(taskData);
            }
            yield return StartCoroutine(PlayTimeline_COR("ShowExtendedTaskPanel"));
        }

        private IEnumerator HideExtendedTaskDescription_COR()
        {
            yield return StartCoroutine(PlayTimeline_COR("HideExtendedTaskPanel"));
            currentTaskDescriptionContentView.SetActive(false);
            ChangeSiblingIndex(-1);
            onPanelClosed.Invoke();
        }

        private void FillImageGallery(TaskText taskData)
        {
            var galleryContainer = taskDescriptionContent_AlternateView.GetComponentInChildren<HorizontalLayoutGroup>().transform;
            for (var i = galleryContainer.childCount - 1; i >= 0; i--)
            {
                Destroy(galleryContainer.GetChild(i).gameObject);
            }
            var imageTitles = taskData.ImagesTitles.Split();
            foreach (var title in imageTitles)
            {
                var taskImage = Instantiate(taskImagePrefab, galleryContainer);
                taskImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/Task Images/" + title);
            }
        }

        private void ChangeSiblingIndex(int indexOffset)
        {
            var currentSiblingIndex = extendedTaskPanelContainer.transform.GetSiblingIndex();
            extendedTaskPanelContainer.transform.SetSiblingIndex(currentSiblingIndex + indexOffset);
        }

        private IEnumerator PlayTimeline_COR(string playableAssetName = null)
        {
            if (playableAssetName != null)
            {
                playableDirector.playableAsset = Resources.Load<PlayableAsset>("Timelines/UI/Task Panel/" + playableAssetName);
            }
            playableDirector.Play();
            yield return new WaitForSeconds((float)playableDirector.playableAsset.duration);
        }

        private void Start()
        {
            extendedTaskPanelContainer = transform.parent.gameObject;
            gameManager = GameManager.Instance;
            playableDirector = GetComponent<PlayableDirector>();
            sceneIndex = SceneManager.GetActiveScene().buildIndex;
        }
    }
}
