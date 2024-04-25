using Cysharp.Threading.Tasks;
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

        public void Show() => animator.ChangeVisibilityAsync(true);

        public async UniTask HideAsync() => await animator.ChangeVisibilityAsync(false);

        public void CreateTrainingTextPage(string trainingTheme, string trainingContent, TrainingShowingMode trainingShowingMode)
        {
            var trainingPage = CreateTrainingPage(trainingTheme, trainingShowingMode, trainingTextPageViewPrefab);
            trainingPage.SetContent(trainingContent);
        }

        public void CreateTrainingTextVideoPage(string trainingTheme, string trainingContent, VideoClip trainingVideo, TrainingShowingMode trainingShowingMode)
        {
            var trainingPage = CreateTrainingPage(trainingTheme, trainingShowingMode, trainingTextVideoPageViewPrefab);
            trainingPage.SetContent(trainingContent, trainingVideo);
        }

        private T CreateTrainingPage<T>(string trainingTheme, TrainingShowingMode trainingShowingMode, T trainingPageViewPrefab) where T : CodingTrainingTextPageView
        {
            DeletePreviousTrainingPage();

            trainingThemeLabel.text = trainingTheme;
            previousPageButton.gameObject.SetActive(trainingShowingMode != TrainingShowingMode.FirstPart);
            nextPageButton.gameObject.SetActive(trainingShowingMode != TrainingShowingMode.LastPart);
            return Instantiate(trainingPageViewPrefab, trainingPagesContainer.transform);
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
