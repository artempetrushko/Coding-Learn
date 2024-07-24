using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
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

        public void Show() => _ = ChangeVisibilityAsync(true);

        public async UniTask HideAsync() => await ChangeVisibilityAsync(false);

        public void CreateTrainingPage(CodingTrainingData codingTrainingData, TrainingShowingMode trainingShowingMode)
        {
            if (codingTrainingData.VideoGuideReference != null)
            {
                CreateTrainingTextPage(codingTrainingData, trainingShowingMode);
            }
            else
            {
                CreateTrainingTextVideoPage(codingTrainingData, trainingShowingMode);
            }
        }

        private void CreateTrainingTextPage(CodingTrainingData codingTrainingData, TrainingShowingMode trainingShowingMode)
        {
            var trainingPage = CreateTrainingPage(codingTrainingData.Title.GetLocalizedString(), trainingShowingMode, trainingTextPageViewPrefab);
            trainingPage.SetContent(codingTrainingData.TrainingText.GetLocalizedString());
        }

        private async void CreateTrainingTextVideoPage(CodingTrainingData codingTrainingData, TrainingShowingMode trainingShowingMode)
        {
            var trainingPage = CreateTrainingPage(codingTrainingData.Title.GetLocalizedString(), trainingShowingMode, trainingTextVideoPageViewPrefab);
            var videoGuideLoading = codingTrainingData.VideoGuideReference.LoadAssetAsync<VideoClip>();
            await videoGuideLoading.Task;
            if (videoGuideLoading.Status == AsyncOperationStatus.Succeeded)
            {
                trainingPage.SetContent(codingTrainingData.TrainingText.GetLocalizedString(), videoGuideLoading.Result);
            }
            Addressables.Release(videoGuideLoading);
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


        [SerializeField]
        private GameObject content;
        [SerializeField]
        private List<Image> backgroundParts;

        private Sequence visibilityChangeTween;

        public async UniTask ChangeVisibilityAsync(bool isVisible)
        {
            visibilityChangeTween ??= CreateVisibilityChangeTween();
            if (isVisible)
            {
                visibilityChangeTween.PlayForward();
            }
            else
            {
                visibilityChangeTween.PlayBackwards();
            }
            await visibilityChangeTween.AsyncWaitForRewind();
        }

        private Sequence CreateVisibilityChangeTween()
        {
            var fillingDuration = 1f;
            var everyPartFillingDuration = fillingDuration / backgroundParts.Count;
            var endFillAmount = 1f;
            var contentEndPositionY = 0f;

            var tweenSequence = DOTween.Sequence();
            tweenSequence.Pause();
            backgroundParts.ForEach(part => tweenSequence.Append(part.DOFillAmount(endFillAmount, everyPartFillingDuration)));
            tweenSequence.Append(content.transform.DOLocalMoveY(contentEndPositionY, 0.5f));
            tweenSequence.SetAutoKill(false);
            return tweenSequence;
        }
    }
}
