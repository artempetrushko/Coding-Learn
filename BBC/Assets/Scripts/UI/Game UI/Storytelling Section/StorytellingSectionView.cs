using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Scripts
{
    public class StorytellingSectionView : MonoBehaviour
    {
        [SerializeField] 
        private TMP_Text storyTextArea;
        [SerializeField] 
        private Button nextStoryPartButton;
        [SerializeField] 
        private Button skipStoryPartButton;

        private bool isSkipButtonPressed = false;

        public void ShowStoryText(string storyText, float textShowingTime) => StartCoroutine(ShowStoryText_COR(storyText, textShowingTime));

        public void SkipStoryTextShowing() => isSkipButtonPressed = true;

        public void ClearStoryTextArea() => storyTextArea.text = "";

        public void SetNextStoryPartButtonActive(bool isActive) => nextStoryPartButton.gameObject.SetActive(isActive);

        private IEnumerator ShowStoryText_COR(string storyText, float textShowingTime)
        {
            skipStoryPartButton.gameObject.SetActive(true);
            var latency = textShowingTime / storyText.Length;
            for (var i = 0; i < storyText.Length; i++)
            {
                if (isSkipButtonPressed)
                {
                    isSkipButtonPressed = false;
                    storyTextArea.text = storyText;
                    break;
                }
                storyTextArea.text += storyText[i];
                yield return new WaitForSeconds(latency);
            }
            skipStoryPartButton.gameObject.SetActive(false);
            SetNextStoryPartButtonActive(true);
        }
    }
}
