using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class LevelStatsCardView : MonoBehaviour
    {
        [SerializeField]
        private Button showDetailedStatsButton;
        [SerializeField]
        private Image levelThumbnail;
        [SerializeField]
        private TMP_Text starsCounterText;

        public void SetInfo(LevelStatsCardData data)
        {
            levelThumbnail.sprite = data.Thumbnail;
            starsCounterText.text = $"{data.StarsCurrentCount}/{data.StarsTotalCount}";
            showDetailedStatsButton.onClick.AddListener(data.CardPressedAction);
        }
    }
}
