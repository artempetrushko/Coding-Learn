using Core.Features.Game.CodingTraining;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using GameLogic;
using Scripts.Assets.Scripts.Core.Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace Scripts.Assets.Scripts.UI.Game.Training
{
    public class TrainingPageComponent : MonoBehaviour
    {
        [SerializeField] private GameObject _content;
        [SerializeField] private TMP_Text _trainingThemeLabel;
        [SerializeField] private GameObject _trainingPagesContainer;
        [SerializeField] private Button _previousPageButton;
        [SerializeField] private Button _nextPageButton;
        [SerializeField] private Button _closeTrainingButton;
        [SerializeField] private Image[] _backgroundParts;
        [SerializeField] private TMP_Text _trainingText;
        [SerializeField] private Scrollbar _trainingTextScrollbar;
        [SerializeField] private VideoPlayer _videoPlayer;

        private readonly CodingTrainingService _codingTrainingService;
        private TrainingData[] _currentTrainingDatas;

        private Sequence _visibilityChangeTween;
        private int _currentTrainingPageNumber;

        private int CurrentTrainingPageNumber
        {
            get => _currentTrainingPageNumber;
            set
            {
                _trainingPagesContainer.transform.GetChild(_currentTrainingPageNumber - 1).gameObject.SetActive(false);

                _currentTrainingPageNumber = value;
                if (_currentTrainingDatas != null)
                {
                    _trainingPagesContainer.transform.GetChild(_currentTrainingPageNumber - 1).gameObject.SetActive(true);

                    _trainingThemeLabel.text = _currentTrainingDatas[_currentTrainingPageNumber - 1].Title.GetLocalizedString();
                    _previousPageButton.gameObject.SetActive(_currentTrainingPageNumber > 1);
                    _nextPageButton.gameObject.SetActive(_currentTrainingPageNumber < _currentTrainingDatas.Length);
                }
            }
        }

        public TrainingPageComponent(CodingTrainingService codingTrainingService)
        {
            _nextPageButton.onClick.AddListener(OnNextPageButtonClicked);
            _previousPageButton.onClick.AddListener(OnPreviousPageButtonClicked);
            _closeTrainingButton.onClick.AddListener(OnCloseTrainingButtonClicked);
        }

        public void SetCurrentTrainingContent(TrainingData[] trainingDatas) => _currentTrainingDatas = trainingDatas;

        public async UniTask ShowAsync()
        {
            CurrentTrainingPageNumber = 1;

            await SetVisibilityAsync(true);
        }

        public async UniTask HideAsync()
        {
            await SetVisibilityAsync(false);
            _trainingPagesContainer.transform.DeleteAllChildren();
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
            var everyPartFillingDuration = fillingDuration / _backgroundParts.Length;

            var tweenSequence = DOTween.Sequence();
            tweenSequence.Pause();

            foreach (var backgroundPart in _backgroundParts)
            {
                tweenSequence.Append(backgroundPart.DOFillAmount(1f, everyPartFillingDuration));
            }
            tweenSequence.Append(_content.transform.DOLocalMoveY(0f, 0.5f));

            tweenSequence.SetAutoKill(false);
            return tweenSequence;
        }

        private void OnNextPageButtonClicked() => _codingTrainingService.ShowNextTrainingPage();

        private void OnPreviousPageButtonClicked() => _codingTrainingService.ShowPreviousTrainingPage();

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
