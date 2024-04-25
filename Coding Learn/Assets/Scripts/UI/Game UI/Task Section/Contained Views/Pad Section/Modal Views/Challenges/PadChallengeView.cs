using TMPro;
using UnityEngine;

namespace Scripts
{
    public class PadChallengeView : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text descriptionText;

        public void SetInfo(string challengeDescription)
        {
            descriptionText.text = challengeDescription;
        }
    }
}
