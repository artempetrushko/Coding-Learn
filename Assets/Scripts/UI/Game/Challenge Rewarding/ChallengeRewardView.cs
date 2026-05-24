using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameLogic
{
    public class ChallengeRewardView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _challengeDescriptionText;
        [SerializeField] private Image _starFillingImage;

        public Image StarFillingImage => _starFillingImage;

        public void SetChallengeDescription(string challengeDescription) => _challengeDescriptionText.text = challengeDescription;

        public void SetChallengeDescriptionColor(Color color) => _challengeDescriptionText.color = color;
    }
}
