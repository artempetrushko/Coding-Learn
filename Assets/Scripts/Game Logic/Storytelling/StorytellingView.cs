using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameLogic
{
    public class StorytellingView : MonoBehaviour
    {
        [SerializeField] private GameObject _storyTextSection;
        [SerializeField] private TMP_Text _storyText;
        [SerializeField] private Button _nextStoryPartButton;
        [SerializeField] private Button _skipStoryPartButton;
        [SerializeField] private Image _blackScreen;

        public Button NextStoryPartButton => _nextStoryPartButton;
        public Button SkipStoryPartButton => _skipStoryPartButton;
        public Image BlackScreen => _blackScreen;

        public void SetStoryTextSectionActive(bool isActive) => _storyTextSection.SetActive(isActive);

        public void SetStoryText(string text) => _storyText.text = text;
    }
}
