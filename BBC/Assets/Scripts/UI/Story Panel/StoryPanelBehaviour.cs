using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Scripts
{
    public class StoryPanelBehaviour : MonoBehaviour
    {
        [HideInInspector] public int storyPartNumber;

        [SerializeField] private TMP_Text storyText;
        [SerializeField] private Button nextStoryPartButton;
        [SerializeField] private float totalTextAppearingTime = 5;

        private GameManager gameManager;

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

        private IEnumerator ShowStoryText_COR()
        {
            var storyPartText = gameManager.StoryParts[gameManager.CurrentTaskNumber - 1][storyPartNumber - 1].Script;
            var latency = totalTextAppearingTime / storyPartText.Length;
            for (var i = 0; i < storyPartText.Length; i++)
            {
                storyText.text += storyPartText[i];
                yield return new WaitForSeconds(latency);
            }
            nextStoryPartButton.gameObject.SetActive(true);
        }

        private void Start()
        {
            gameManager = GameManager.Instance;
            storyPartNumber = 1;
        }
    }
}
