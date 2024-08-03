using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Video;
using Zenject;

namespace Scripts
{
    public class CodingTrainingSectionController
    {
        private CodingTrainingSectionView _codingTrainingSectionView;
        private CodingTrainingTextPageView trainingTextPageViewPrefab;
        private CodingTrainingTextVideoPageView trainingTextVideoPageViewPrefab;
        private DiContainer _diContainer;

        private Sequence visibilityChangeTween;

        public CodingTrainingSectionController(DiContainer diContainer, CodingTrainingSectionView codingTrainingSectionView)
        {
            _codingTrainingSectionView = codingTrainingSectionView;
            _diContainer = diContainer;
        }

        public void Show() => ChangeVisibilityAsync(true).Forget();

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
            trainingPage.SetTrainingText(codingTrainingData.TrainingText.GetLocalizedString());
            trainingPage.SetSliderValue(1f);
        }

        private async void CreateTrainingTextVideoPage(CodingTrainingData codingTrainingData, TrainingShowingMode trainingShowingMode)
        {
            var trainingPage = CreateTrainingPage(codingTrainingData.Title.GetLocalizedString(), trainingShowingMode, trainingTextVideoPageViewPrefab);
            var videoGuideLoading = codingTrainingData.VideoGuideReference.LoadAssetAsync<VideoClip>();
            await videoGuideLoading.Task;
            if (videoGuideLoading.Status == AsyncOperationStatus.Succeeded)
            {
                trainingPage.SetTrainingText(codingTrainingData.TrainingText.GetLocalizedString());
                trainingPage.SetSliderValue(1f);
                trainingPage.SetVideoClip(videoGuideLoading.Result);
            }
            Addressables.Release(videoGuideLoading);
        }

        private T CreateTrainingPage<T>(string trainingTheme, TrainingShowingMode trainingShowingMode, T trainingPageViewPrefab) where T : CodingTrainingTextPageView
        {
            DeletePreviousTrainingPage();

            _codingTrainingSectionView.SetTrainingThemeLabelText(trainingTheme);
            _codingTrainingSectionView.SetPreviousPageButtonActive(trainingShowingMode != TrainingShowingMode.FirstPart);
            _codingTrainingSectionView.SetNextPageButtonActive(trainingShowingMode != TrainingShowingMode.LastPart);

            return _diContainer.InstantiatePrefab(trainingPageViewPrefab, _codingTrainingSectionView.TrainingPagesContainer.transform).GetComponent<T>();
        }

        private void DeletePreviousTrainingPage()
        {
            if (_codingTrainingSectionView.TrainingPagesContainer.transform.childCount > 0)
            {
                Object.Destroy(_codingTrainingSectionView.TrainingPagesContainer.transform.GetChild(0).gameObject);
            }
            _codingTrainingSectionView.TrainingPagesContainer.transform.DetachChildren();
        }




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
