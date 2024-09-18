using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Video;
using Object = UnityEngine.Object;

namespace GameLogic
{
    public class TrainingPresenter
    {
        public event Action TrainingDisabled;

        private TrainingView _trainingView;
        private TrainingData[] _currentTrainingDatas;    
        private TrainingTextPageView _trainingTextPageViewPrefab;
        private TrainingTextVideoPageView _trainingTextVideoPageViewPrefab;

        private Sequence _visibilityChangeTween;   
        private int _currentTrainingPageNumber;

        private int CurrentTrainingPageNumber
        {
            get => _currentTrainingPageNumber;
            set
            {
                _trainingView.TrainingPagesContainer.transform.GetChild(_currentTrainingPageNumber - 1).gameObject.SetActive(false);

                _currentTrainingPageNumber = value;
                if (_currentTrainingDatas != null)
                {
                    _trainingView.TrainingPagesContainer.transform.GetChild(_currentTrainingPageNumber - 1).gameObject.SetActive(true);

                    _trainingView.SetTrainingThemeLabelText(_currentTrainingDatas[_currentTrainingPageNumber - 1].Title.GetLocalizedString());
                    _trainingView.PreviousPageButton.gameObject.SetActive(_currentTrainingPageNumber > 1);
                    _trainingView.NextPageButton.gameObject.SetActive(_currentTrainingPageNumber < _currentTrainingDatas.Length);
                }
            }
        }

        public TrainingPresenter(TrainingView trainingView)
        {
            _trainingView = trainingView;

            _trainingView.NextPageButton.onClick.AddListener(OnNextPageButtonClicked);
            _trainingView.PreviousPageButton.onClick.AddListener(OnPreviousPageButtonClicked);
            _trainingView.CloseTrainingButton.onClick.AddListener(OnCloseTrainingButtonClicked);
        }

        public void SetCurrentTrainingContent(TrainingData[] trainingDatas) => _currentTrainingDatas = trainingDatas;

        public async UniTask ShowAsync()
        {
            CreateTrainingPages(_currentTrainingDatas);
            CurrentTrainingPageNumber = 1;

            await SetVisibilityAsync(true);
        }

        public async UniTask HideAsync()
        {
            await SetVisibilityAsync(false);
            _trainingView.TrainingPagesContainer.transform.DeleteAllChildren();
        }

        private async UniTask SetVisibilityAsync(bool isVisible)
        {
            _visibilityChangeTween ??= CreateVisibilityChangeTween();
            if (isVisible)
            {
                _visibilityChangeTween.PlayForward();
            }
            else
            {
                _visibilityChangeTween.PlayBackwards();
            }
            await _visibilityChangeTween.AsyncWaitForRewind();
        }

        private Sequence CreateVisibilityChangeTween()
        {
            var fillingDuration = 1f;
            var everyPartFillingDuration = fillingDuration / _trainingView.BackgroundParts.Length;

            var tweenSequence = DOTween.Sequence();
            tweenSequence.Pause();

            foreach (var backgroundPart in _trainingView.BackgroundParts)
            {
                tweenSequence.Append(backgroundPart.DOFillAmount(1f, everyPartFillingDuration));
            }
            tweenSequence.Append(_trainingView.Content.transform.DOLocalMoveY(0f, 0.5f));

            tweenSequence.SetAutoKill(false);
            return tweenSequence;
        }

        private void CreateTrainingPages(TrainingData[] trainingDatas)
        {
            foreach (var trainingData in trainingDatas)
            {
                if (trainingData.VideoGuideReference != null)
                {
                    CreateTrainingTextPage(trainingData);
                }
                else
                {
                    CreateTrainingTextVideoPage(trainingData);
                }
            }
        }

        private void CreateTrainingTextPage(TrainingData trainingData) => CreateTrainingPage(_trainingTextPageViewPrefab, trainingData);

        private async void CreateTrainingTextVideoPage(TrainingData trainingData)
        {
            var trainingPage = CreateTrainingPage(_trainingTextVideoPageViewPrefab, trainingData);

            var videoGuideLoading = trainingData.VideoGuideReference.LoadAssetAsync<VideoClip>();
            await videoGuideLoading.Task;
            if (videoGuideLoading.Status == AsyncOperationStatus.Succeeded)
            {
                trainingPage.SetVideoClip(videoGuideLoading.Result);
            }
            Addressables.Release(videoGuideLoading);
        }

        private T CreateTrainingPage<T>(T trainingPageViewPrefab, TrainingData trainingData) where T : TrainingTextPageView
        {
            var trainingPage = Object.Instantiate(trainingPageViewPrefab, _trainingView.TrainingPagesContainer.transform);
            trainingPage.SetTrainingText(trainingData.TrainingText.GetLocalizedString());
            trainingPage.SetTrainingTextScrollbarValue(1f);

            return trainingPage;
        }

        private void OnNextPageButtonClicked() => CurrentTrainingPageNumber++;

        private void OnPreviousPageButtonClicked() => CurrentTrainingPageNumber--;

        private void OnCloseTrainingButtonClicked()
        {
            UniTask.Void(async () =>
            {
                await HideAsync();
                TrainingDisabled?.Invoke();
            });
        }
    }
}
