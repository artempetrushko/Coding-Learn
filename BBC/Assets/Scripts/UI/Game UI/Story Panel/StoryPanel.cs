using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Scripts
{
    public class StoryPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text storyText;
        [SerializeField] private Button nextStoryPartButton;
        [SerializeField] private Button skipStoryPartButton;
        [SerializeField] private float totalTextAppearingTime = 5;
        [SerializeField] private float cutsceneFrameLength = 7;
        [SerializeField] private float transitionToNextFrameTime = 1.2f;

        private GameManager gameManager;
        private int storyPartNumber;
        private bool isSkipButtonPressed = false;

        public void ShowStoryText() => StartCoroutine(ShowStoryText_COR());

        public void ShowFirstStoryText()
        {
            storyPartNumber = 1;
            StartCoroutine(ShowStoryText_COR());
        }

        public void HideTextAndContinue()
        {
            nextStoryPartButton.gameObject.SetActive(false);
            storyText.text = "";
            storyPartNumber++;
        }

        public void SkipText()
        {
            isSkipButtonPressed = true;
            gameManager.ChangeCutsceneCurrentTime(storyPartNumber * cutsceneFrameLength - transitionToNextFrameTime);
        }

        private IEnumerator ShowStoryText_COR()
        {
            skipStoryPartButton.gameObject.SetActive(true);
            var storyPartText = ResourcesData.StoryParts[gameManager.GetCurrentTaskNumber() - 1][storyPartNumber - 1].Script;
            var latency = totalTextAppearingTime / storyPartText.Length;
            for (var i = 0; i < storyPartText.Length; i++)
            {
                if (isSkipButtonPressed)
                {
                    isSkipButtonPressed = false;
                    storyText.text = storyPartText;
                    break;
                }
                storyText.text += storyPartText[i];
                yield return new WaitForSeconds(latency);
            }
            skipStoryPartButton.gameObject.SetActive(false);
            //nextStoryPartButton.gameObject.SetActive(true);
        }

        private void Start()
        {
            gameManager = GameManager.Instance;
            storyPartNumber = 1;
        }
    }
}
