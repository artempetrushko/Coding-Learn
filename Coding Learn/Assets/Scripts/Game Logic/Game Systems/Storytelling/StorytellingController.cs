using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Scripts
{
    public class StorytellingController : MonoBehaviour
    {
        public event Action CutsceneFinished;

        private const float correctiveTimeOffset = 0.2f;

        [SerializeField]
        private StorytellingSectionView storytellingSectionView;
        [SerializeField]
        private PlayableDirector playableDirector;

        private StoryContent currentStoryContent;
        private int currentStoryPartArticleNumber;     

        public void ShowNewStoryContent(StoryContent storyContent)
        {
            currentStoryContent = storyContent;
            currentStoryPartArticleNumber = 0;
            playableDirector.time = 0;
            playableDirector.Play(currentStoryContent.Cutscene);
        }

        public void ResumeCutscene()
        {
            playableDirector.Resume();
            storytellingSectionView.ClearStoryTextArea();
            storytellingSectionView.SetNextStoryPartButtonActive(false);
        }

        public void FinishCutscene() => CutsceneFinished?.Invoke();

        public void ShowNextStoryPartArticle() => ShowNewStoryPartArticle(++currentStoryPartArticleNumber);     

        public void SkipCurrentStoryPart()
        {
            storytellingSectionView.SkipStoryTextShowing();
            playableDirector.time = GetCurrentClipStopTime();
        }

        private void ShowNewStoryPartArticle(int storyPartArticleNumber)
        {
            currentStoryPartArticleNumber = storyPartArticleNumber;
            var totalTextAppearingTime = (float)(GetCurrentClipStopTime() - playableDirector.time);
            _ = storytellingSectionView.ShowStoryTextAsync(currentStoryContent.CutsceneScenarioParts[currentStoryPartArticleNumber - 1].GetLocalizedString(), totalTextAppearingTime);
        }

        private double GetCurrentClipStopTime()
        {
            var currentCameraClip = GetCurrentCutsceneTrackClips(1)[currentStoryPartArticleNumber - 1];
            var blackScreenClips = GetCurrentCutsceneTrackClips(2);
            if (currentStoryPartArticleNumber * 2 < blackScreenClips.Count)
            {
                var blackScreenShowDuration = blackScreenClips[currentStoryPartArticleNumber * 2 - 1].duration;
                return currentCameraClip.end - blackScreenShowDuration - correctiveTimeOffset;
            }
            return currentCameraClip.end - currentCameraClip.blendOutDuration - correctiveTimeOffset;
        }

        private List<TimelineClip> GetCurrentCutsceneTrackClips(int trackIndex) => currentStoryContent.Cutscene.GetOutputTrack(trackIndex).GetClips().ToList();
    }
}
