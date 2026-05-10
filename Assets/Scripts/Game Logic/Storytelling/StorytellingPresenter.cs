using System;
using System.Linq;
using System.Text;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace GameLogic
{
    public class StorytellingPresenter : IDisposable
    {
        public event Action CutsceneFinished;

        private const int CAMERAS_CUTSCENE_TRACK_INDEX = 1;

        private StorytellingView _storytellingView;    
        private StoryContent _currentStoryContent;
        private PlayableDirector _playableDirector;
        private CutsceneSignalsHandler _cutsceneSignalsHandler;
        private int _currentStoryPartArticleNumber;
        private bool _wasStoryTextSkipped = false;

        public StorytellingPresenter(StorytellingView storytellingSectionView, CutsceneSignalsHandler cutsceneSignalsHandler)
        {
            _storytellingView = storytellingSectionView;
            _cutsceneSignalsHandler = cutsceneSignalsHandler;

            _playableDirector.stopped += OnPlayableDirectorStopped;

            _cutsceneSignalsHandler.ShowStorySignalReceived += OnShowStorySignalReceived;
            _cutsceneSignalsHandler.StopCurrentFrameSignalReceived += OnStopCurrentFrameSignalReceived;

            _storytellingView.SkipStoryPartButton.onClick.AddListener(OnSkipStoryPartButtonPressed);
            _storytellingView.NextStoryPartButton.onClick.AddListener(OnNextStoryPartButtonPressed);
        }

        public void Dispose()
        {
            _playableDirector.stopped -= OnPlayableDirectorStopped;

            _cutsceneSignalsHandler.ShowStorySignalReceived -= OnShowStorySignalReceived;
            _cutsceneSignalsHandler.StopCurrentFrameSignalReceived -= OnStopCurrentFrameSignalReceived;
        }

        public void ShowNewStoryContent(StoryContent storyContent)
        {
            _currentStoryContent = storyContent;
            _currentStoryPartArticleNumber = 1;
            StartCutscene(_currentStoryContent.Cutscene);
        } 

        private void StartCutscene(TimelineAsset cutscene)
        {
            _playableDirector.time = 0;
            _playableDirector.Play(cutscene);
            HideBlackScreenAsync().Forget();
        }

        private async UniTask ShowStoryPartTextAsync(int storyPartArticleNumber)
        {
            var storyText = _currentStoryContent.CutsceneScenarioParts[storyPartArticleNumber - 1].GetLocalizedString();
            var totalTextShowingTime = (float)(GetCurrentFrameStopTime() - _playableDirector.time);

            _storytellingView.SetStoryTextSectionActive(true);
            _storytellingView.SkipStoryPartButton.gameObject.SetActive(true);

            var displayedText = new StringBuilder();
            var latency = totalTextShowingTime / storyText.Length;
            for (var i = 0; i < storyText.Length; i++)
            {
                if (_wasStoryTextSkipped)
                {
                    _wasStoryTextSkipped = false;
                    _storytellingView.SetStoryText(storyText);
                    break;
                }
                displayedText.Append(storyText[i]);
                _storytellingView.SetStoryText(displayedText.ToString());

                await UniTask.WaitForSeconds(latency);
            }

            _storytellingView.SkipStoryPartButton.gameObject.SetActive(false);
        }

        private double GetCurrentFrameStopTime()
        {
            var currentCameraClip = _currentStoryContent.Cutscene.GetOutputTrack(CAMERAS_CUTSCENE_TRACK_INDEX).GetClips().ToArray()[_currentStoryPartArticleNumber - 1];
            return currentCameraClip.end - currentCameraClip.blendOutDuration;
        }

        private async UniTask PlayNextCutsceneFrameTransitionAsync()
        {
            await ShowBlackScreenAsync();
            await HideBlackScreenAsync();
        }

        private async UniTask ShowBlackScreenAsync()
        {
            _storytellingView.BlackScreen.gameObject.SetActive(true);
            await _storytellingView.BlackScreen.DOFade(1f, 1f).AsyncWaitForCompletion();
        }

        private async UniTask HideBlackScreenAsync()
        {
            await _storytellingView.BlackScreen.DOFade(0f, 1f).AsyncWaitForCompletion();
            _storytellingView.BlackScreen.gameObject.SetActive(false);
        }

        private void OnShowStorySignalReceived() => ShowStoryPartTextAsync(_currentStoryPartArticleNumber).Forget();

        private void OnStopCurrentFrameSignalReceived()
        {
            _playableDirector.Pause();
            _storytellingView.NextStoryPartButton.gameObject.SetActive(true);
        }

        private void OnSkipStoryPartButtonPressed()
        {
            _playableDirector.time = GetCurrentFrameStopTime();
            _wasStoryTextSkipped = true;
        }

        private void OnNextStoryPartButtonPressed()
        {
            _storytellingView.NextStoryPartButton.gameObject.SetActive(false);
            _storytellingView.SetStoryText("");
            _storytellingView.SetStoryTextSectionActive(false);    

            _playableDirector.Resume();
            PlayNextCutsceneFrameTransitionAsync().Forget();
        }

        private void OnPlayableDirectorStopped(PlayableDirector playableDirector)
        {
            if (playableDirector.time >= _playableDirector.playableAsset.duration)
            {
                CutsceneFinished?.Invoke();
            }
        }
    }
}
