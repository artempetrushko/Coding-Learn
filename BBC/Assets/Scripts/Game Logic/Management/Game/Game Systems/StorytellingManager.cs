using System.Collections;
using System.Collections.Generic;
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
        [Space, SerializeField] 
        private float totalTextAppearingTime = 5;
        [SerializeField] 
        private float cutsceneFrameLength = 7;
        [SerializeField] 
        private float transitionToNextFrameTime = 1.2f;
        [SerializeField]
        private List<PlayableAsset> cutscenes = new List<PlayableAsset>();

        private StoryInfo[] storyParts;
        //private int currentCutsceneNumber;
        private int currentStoryPartNumber;

        public void LoadStoryParts(int taskNumber) => storyParts = ContentManager.StoryParts[taskNumber - 1];

        /*public void PlayFirstCutscene()
        {
            currentCutsceneNumber = 1;
            PlayCutscene(currentCutsceneNumber);
        }

        public void PlayNextCutscene() => PlayCutscene(++currentCutsceneNumber);*/

        public void PlayCutscene(int cutsceneNumber)
        {
            storyParts = ContentManager.StoryParts[cutsceneNumber - 1];
            ChangeCutsceneCurrentTime(0);
            playableDirector.Play(cutscenes[cutsceneNumber - 1]);
        }

        public void ResumeCutscene()
        {
            playableDirector.Resume();
            storytellingSectionView.ClearStoryTextArea();
            storytellingSectionView.SetNextStoryPartButtonActive(false);
        }

        public void ShowFirstStoryPart()
        {
            currentStoryPartNumber = 1;
            storytellingSectionView.ShowStoryText(storyParts[currentStoryPartNumber - 1].Story, totalTextAppearingTime);
        }

        public void ShowNextStoryPart()
        {
            currentStoryPartNumber++;
            storytellingSectionView.ShowStoryText(storyParts[currentStoryPartNumber - 1].Story, totalTextAppearingTime);
        }      

        public void SkipCurrentStoryPart()
        {
            storytellingSectionView.SkipStoryTextShowing();
            ChangeCutsceneCurrentTime(currentStoryPartNumber * cutsceneFrameLength - transitionToNextFrameTime);
        }

        private void ChangeCutsceneCurrentTime(float newTime) => playableDirector.time = newTime;
    }
}
