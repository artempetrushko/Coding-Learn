using TMPro;
using UnityEngine;

namespace Scripts
{
    public class PadChallengeView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _descriptionText;

        public void SetDescriptionText(string text) => _descriptionText.text = text;
    }
}
