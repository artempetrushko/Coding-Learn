using TMPro;
using UnityEngine;

namespace UI.Game
{
    public class ChallengeView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _descriptionText;

        public void SetDescriptionText(string text) => _descriptionText.text = text;
    }
}
