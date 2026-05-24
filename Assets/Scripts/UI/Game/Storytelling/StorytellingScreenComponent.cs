using System.Linq;
using System.Text;
using Core.Features.Game.Storytelling;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;

namespace Scripts.Assets.Scripts.UI.Game.Storytelling
{
    public class StorytellingScreenComponent : MonoBehaviour
    {
        [SerializeField] private GameObject _storyTextSection;
        [SerializeField] private TMP_Text _storyText;
        [SerializeField] private Button _nextStoryPartButton;
        [SerializeField] private Button _skipStoryPartButton;
        [SerializeField] private Image _blackScreen;

        private readonly StorytellingService _storytellingService;
        
        private bool _wasStoryTextSkipped = false;

        public StorytellingScreenComponent(StorytellingService storytellingService)
        {
            _storytellingService = storytellingService;

            _skipStoryPartButton.onClick.AddListener(OnSkipStoryPartButtonPressed);
            _nextStoryPartButton.onClick.AddListener(OnNextStoryPartButtonPressed);
        }

        public void Dispose()
        {

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

            _storyTextSection.SetActive(true);
            _skipStoryPartButton.gameObject.SetActive(true);

            var displayedText = new StringBuilder();
            var latency = totalTextShowingTime / storyText.Length;
            for (var i = 0; i < storyText.Length; i++)
            {
                if (_wasStoryTextSkipped)
                {
                    _wasStoryTextSkipped = false;
                    _storyText.text = storyText;
                    break;
                }
                displayedText.Append(storyText[i]);
                _storyText.text = displayedText.ToString();

                await UniTask.WaitForSeconds(latency);
            }

            _skipStoryPartButton.gameObject.SetActive(false);
        }

        private double GetCurrentFrameStopTime()
        {
            var currentCameraClip = _currentStoryContent.Cutscene.GetOutputTrack(1).GetClips().ToArray()[_currentStoryPartArticleNumber - 1];
            return currentCameraClip.end - currentCameraClip.blendOutDuration;
        }

        private async UniTask PlayNextCutsceneFrameTransitionAsync()
        {
            await ShowBlackScreenAsync();
            await HideBlackScreenAsync();
        }

        private async UniTask ShowBlackScreenAsync()
        {
            _blackScreen.gameObject.SetActive(true);
            await _blackScreen.DOFade(1f, 1f).AsyncWaitForCompletion();
        }

        private async UniTask HideBlackScreenAsync()
        {
            await _blackScreen.DOFade(0f, 1f).AsyncWaitForCompletion();
            _blackScreen.gameObject.SetActive(false);
        }

        private void OnShowStorySignalReceived() => ShowStoryPartTextAsync(_currentStoryPartArticleNumber).Forget();

        private void OnStopCurrentFrameSignalReceived()
        {
            _playableDirector.Pause();
            _nextStoryPartButton.gameObject.SetActive(true);
        }

        private void OnSkipStoryPartButtonPressed()
        {
            _playableDirector.time = GetCurrentFrameStopTime();
            _wasStoryTextSkipped = true;
        }

        private void OnNextStoryPartButtonPressed()
        {
            _nextStoryPartButton.gameObject.SetActive(false);
            _storyText.text = "";
            _storyTextSection.SetActive(false);

            _playableDirector.Resume();
            PlayNextCutsceneFrameTransitionAsync().Forget();
        }
    }
}
