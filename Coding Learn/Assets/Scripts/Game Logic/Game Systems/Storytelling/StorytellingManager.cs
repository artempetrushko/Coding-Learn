using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Scripts
{
    public class StorytellingManager : MonoBehaviour
    {
        [SerializeField]
        private StorytellingSectionView storytellingSectionView;
        [SerializeField]
        private PlayableDirector playableDirector;

        private (TimelineAsset cutscene, string[] storyArticles) currentStoryPart;
        private int currentStoryPartNumber;
        private int currentStoryPartArticleNumber;
        private double correctiveTimeOffset = 0.2;

        public void PlayFirstCutscene() => PlayCutscene(1);

        public void PlayNextCutscene() => PlayCutscene(++currentStoryPartNumber);    

        public void ResumeCutscene()
        {
            playableDirector.Resume();
            storytellingSectionView.ClearStoryTextArea();
            storytellingSectionView.SetNextStoryPartButtonActive(false);
        }

        public void ShowFirstStoryPartArticle() => ShowNewStoryPartArticle(1);

        public void ShowNextStoryPartArticle() => ShowNewStoryPartArticle(++currentStoryPartArticleNumber);     

        public void SkipCurrentStoryPart()
        {
            storytellingSectionView.SkipStoryTextShowing();
            playableDirector.time = GetCurrentClipStopTime();
        }

        private void PlayCutscene(int storyPartNumber)
        {
            currentStoryPartNumber = storyPartNumber;
            currentStoryPart = GameContentManager.GetStoryPart(GameManager.CurrentLevelNumber, currentStoryPartNumber);
            playableDirector.time = 0;
            playableDirector.Play(currentStoryPart.cutscene);
        }

        private void ShowNewStoryPartArticle(int storyPartArticleNumber)
        {
            currentStoryPartArticleNumber = storyPartArticleNumber;
            var totalTextAppearingTime = (float)(GetCurrentClipStopTime() - playableDirector.time);
            _ = storytellingSectionView.ShowStoryTextAsync(currentStoryPart.storyArticles[currentStoryPartArticleNumber - 1], totalTextAppearingTime);
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

        private List<TimelineClip> GetCurrentCutsceneTrackClips(int trackIndex) => currentStoryPart.cutscene.GetOutputTrack(trackIndex).GetClips().ToList();
    }
}
