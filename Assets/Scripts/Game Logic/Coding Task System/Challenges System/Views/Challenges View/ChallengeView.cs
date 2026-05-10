using TMPro;
using UnityEngine;

namespace GameLogic
{
    public class ChallengeView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _descriptionText;

        public void SetDescriptionText(string text) => _descriptionText.text = text;
    }
}
