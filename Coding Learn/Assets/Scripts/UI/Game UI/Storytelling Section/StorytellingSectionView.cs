using Cysharp.Threading.Tasks;
using System;
using TMPro;
using UnityEngine;
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

        public async UniTask ShowStoryTextAsync(string storyText, float textShowingTime)
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
                await UniTask.Delay(TimeSpan.FromSeconds(latency));
            }
            skipStoryPartButton.gameObject.SetActive(false);
            SetNextStoryPartButtonActive(true);
        }

        public void SkipStoryTextShowing() => isSkipButtonPressed = true;

        public void ClearStoryTextArea() => storyTextArea.text = "";

        public void SetNextStoryPartButtonActive(bool isActive) => nextStoryPartButton.gameObject.SetActive(isActive);
    }
}
