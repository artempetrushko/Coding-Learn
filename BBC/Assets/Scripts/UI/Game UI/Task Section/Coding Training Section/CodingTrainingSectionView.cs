using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace Scripts
{
    public enum TrainingShowingMode
    {
        Normal,
        FirstPart,
        LastPart
    }

    public class CodingTrainingSectionView : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text trainingThemeLabel;
        [SerializeField]
        private GameObject trainingPagesContainer;
        [SerializeField]
        private Button previousPageButton;
        [SerializeField]
        private Button nextPageButton;
        [Space, SerializeField]
        private CodingTrainingTextPageView trainingTextPageViewPrefab;
        [SerializeField]
        private CodingTrainingTextVideoPageView trainingTextVideoPageViewPrefab;
        [Space, SerializeField]
        private CodingTrainingSectionAnimator animator;

        public void Show() => StartCoroutine(animator.Show_COR());

        public IEnumerator Hide_COR()
        {
            yield return StartCoroutine(animator.Hide_COR());
        }

        public void CreateTrainingTextPage(string trainingTheme, string trainingContent, TrainingShowingMode trainingShowingMode)
        {
            DeletePreviousTrainingPage();
            SetHeaderContent(trainingTheme, trainingShowingMode);

            var trainingTextPage = Instantiate(trainingTextPageViewPrefab, trainingPagesContainer.transform);
            trainingTextPage.SetContent(trainingContent);
        }

        public void CreateTrainingTextVideoPage(string trainingTheme, string trainingContent, VideoClip trainingVideo, TrainingShowingMode trainingShowingMode)
        {
            DeletePreviousTrainingPage();
            SetHeaderContent(trainingTheme, trainingShowingMode);

            var trainingTextPage = Instantiate(trainingTextVideoPageViewPrefab, trainingPagesContainer.transform);
            trainingTextPage.SetContent(trainingContent, trainingVideo);
        }

        private void SetHeaderContent(string trainingTheme, TrainingShowingMode trainingShowingMode)
        {
            trainingThemeLabel.text = trainingTheme;
            previousPageButton.gameObject.SetActive(trainingShowingMode == TrainingShowingMode.Normal || trainingShowingMode == TrainingShowingMode.LastPart);
            nextPageButton.gameObject.SetActive(trainingShowingMode == TrainingShowingMode.Normal || trainingShowingMode == TrainingShowingMode.FirstPart);
        }

        private void DeletePreviousTrainingPage()
        {
            if (trainingPagesContainer.transform.childCount > 0)
            {
                Destroy(trainingPagesContainer.transform.GetChild(0).gameObject);
            }
            trainingPagesContainer.transform.DetachChildren();
        }
    }
}
