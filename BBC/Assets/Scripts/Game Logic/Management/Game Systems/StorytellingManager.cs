using System.Collections;
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
        [SerializeField]
        private List<TimelineAsset> cutscenes = new List<TimelineAsset>();

        private StoryInfo storyPart;
        private int currentCutsceneNumber;
        private int currentStoryPartNumber;
        private double correctiveTimeOffset = 0.2;

        public void PlayFirstCutscene() => PlayCutscene(1);

        public void PlayNextCutscene() => PlayCutscene(++currentCutsceneNumber);    

        public void ResumeCutscene()
        {
            playableDirector.Resume();
            storytellingSectionView.ClearStoryTextArea();
            storytellingSectionView.SetNextStoryPartButtonActive(false);
        }

        public void ShowFirstStoryPart() => ShowNewStoryPart(1);

        public void ShowNextStoryPart() => ShowNewStoryPart(++currentStoryPartNumber);     

        public void SkipCurrentStoryPart()
        {
            storytellingSectionView.SkipStoryTextShowing();
            playableDirector.time = GetCurrentClipStopTime();
        }

        private void PlayCutscene(int cutsceneNumber)
        {
            currentCutsceneNumber = cutsceneNumber;
            storyPart = GameContentManager.GetStoryInfo(currentCutsceneNumber);
            playableDirector.time = 0;
            playableDirector.Play(cutscenes[currentCutsceneNumber - 1]);
        }

        private void ShowNewStoryPart(int storyPartNumber)
        {
            currentStoryPartNumber = storyPartNumber;
            var totalTextAppearingTime = (float)(GetCurrentClipStopTime() - playableDirector.time);
            storytellingSectionView.ShowStoryText(storyPart.Articles[currentStoryPartNumber - 1], totalTextAppearingTime);
        }

        private double GetCurrentClipStopTime()
        {
            var currentCameraClip = GetCurrentCutsceneTrackClips(1)[currentStoryPartNumber - 1];
            var blackScreenClips = GetCurrentCutsceneTrackClips(2);
            if (currentStoryPartNumber * 2 < blackScreenClips.Count)
            {
                var blackScreenShowDuration = blackScreenClips[currentStoryPartNumber * 2 - 1].duration;
                return currentCameraClip.end - blackScreenShowDuration - correctiveTimeOffset;
            }
            return currentCameraClip.end - currentCameraClip.blendOutDuration - correctiveTimeOffset;
        }

        private List<TimelineClip> GetCurrentCutsceneTrackClips(int trackIndex) => cutscenes[currentCutsceneNumber - 1].GetOutputTrack(trackIndex).GetClips().ToList();
    }
}
