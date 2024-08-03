using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class StorytellingSectionView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _storyText;
        [SerializeField] private Button _nextStoryPartButton;
        [SerializeField] private Button _skipStoryPartButton;

        public void SetNextStoryPartButtonActive(bool isActive) => _nextStoryPartButton.gameObject.SetActive(isActive);

        public void SetSkipStoryPartButtonActive(bool isActive) => _skipStoryPartButton.gameObject.SetActive(isActive);

        public void SetStoryText(string text) => _storyText.text = text;

        public void AddStoryTextFragment(string textFragment) => _storyText.text += textFragment;
    }
}
