using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

namespace Scripts
{
    public class StorytellingManager : MonoBehaviour
    {
        [SerializeField]
        private StorytellingSectionView storytellingSectionView;
        [SerializeField]
        private PlayableDirector playableDirector;

        [SerializeField] 
        private List<PlayableAsset> cutscenes;
        [SerializeField] 
        private float totalTextAppearingTime = 5;
        [SerializeField] 
        private float cutsceneFrameLength = 7;
        [SerializeField] 
        private float transitionToNextFrameTime = 1.2f;

        private Story[] storyParts;
        private int currentCutsceneNumber;
        private int currentStoryPartNumber;

        public void LoadStoryParts(int taskNumber) => storyParts = ResourcesData.StoryParts[taskNumber - 1];

        public void PlayFirstCutscene()
        {
            currentCutsceneNumber = 1;
            PlayCutscene(currentCutsceneNumber);
        }

        public void PlayNextCutscene() => PlayCutscene(++currentCutsceneNumber);

        public void ResumeCutscene()
        {
            playableDirector.Resume();
            storytellingSectionView.ClearStoryTextArea();
            storytellingSectionView.SetNextStoryPartButtonActive(false);
        }

        public void ShowFirstStoryPart()
        {
            currentStoryPartNumber = 1;
            storytellingSectionView.ShowStoryText(storyParts[currentStoryPartNumber - 1].Text, totalTextAppearingTime);
        }

        public void ShowNextStoryPart()
        {
            currentStoryPartNumber++;
            storytellingSectionView.ShowStoryText(storyParts[currentStoryPartNumber - 1].Text, totalTextAppearingTime);
        }      

        public void SkipCurrentStoryPart()
        {
            storytellingSectionView.SkipStoryTextShowing();
            ChangeCutsceneCurrentTime(currentStoryPartNumber * cutsceneFrameLength - transitionToNextFrameTime);
        }

        private void PlayCutscene(int cutsceneNumber)
        {
            ChangeCutsceneCurrentTime(0);
            playableDirector.Play(cutscenes[cutsceneNumber - 1]);
        }

        private void ChangeCutsceneCurrentTime(float newTime) => playableDirector.time = newTime;
    }
}
