using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Scripts
{
    public class StorytellingManager
    {
        public event Action CutsceneFinished;

        private const float CORRECTIVE_TIME_OFFSET = 0.2f;

        private StorytellingSectionController _storytellingSectionController;
        private PlayableDirector _playableDirector;
        private StoryContent _currentStoryContent;
        private int _currentStoryPartArticleNumber;     

        public void ShowNewStoryContent(StoryContent storyContent)
        {
            _currentStoryContent = storyContent;
            _currentStoryPartArticleNumber = 0;
            _playableDirector.time = 0;
            _playableDirector.Play(_currentStoryContent.Cutscene);
        }

        public void ResumeCutscene()
        {
            _playableDirector.Resume();
            _storytellingSectionController.ClearSection();
        }

        public void FinishCutscene() => CutsceneFinished?.Invoke();

        public void ShowNextStoryPartArticle() => ShowNewStoryPartArticle(++_currentStoryPartArticleNumber);     

        public void SkipCurrentStoryPart()
        {
            _storytellingSectionController.SkipStoryTextShowing();
            _playableDirector.time = GetCurrentClipStopTime();
        }

        private void ShowNewStoryPartArticle(int storyPartArticleNumber)
        {
            _currentStoryPartArticleNumber = storyPartArticleNumber;
            var totalTextAppearingTime = (float)(GetCurrentClipStopTime() - _playableDirector.time);
            _storytellingSectionController.ShowStoryTextAsync(_currentStoryContent.CutsceneScenarioParts[_currentStoryPartArticleNumber - 1].GetLocalizedString(), totalTextAppearingTime).Forget();
        }

        private double GetCurrentClipStopTime()
        {
            var currentCameraClip = GetCurrentCutsceneTrackClips(1)[_currentStoryPartArticleNumber - 1];
            var blackScreenClips = GetCurrentCutsceneTrackClips(2);
            if (_currentStoryPartArticleNumber * 2 < blackScreenClips.Count)
            {
                var blackScreenShowDuration = blackScreenClips[_currentStoryPartArticleNumber * 2 - 1].duration;
                return currentCameraClip.end - blackScreenShowDuration - CORRECTIVE_TIME_OFFSET;
            }
            return currentCameraClip.end - currentCameraClip.blendOutDuration - CORRECTIVE_TIME_OFFSET;
        }

        private List<TimelineClip> GetCurrentCutsceneTrackClips(int trackIndex) => _currentStoryContent.Cutscene.GetOutputTrack(trackIndex).GetClips().ToList();
    }
}
