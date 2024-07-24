using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class LevelStatsCardView : MonoBehaviour
    {
        [SerializeField] private Button _showDetailedStatsButton;
        [SerializeField] private Image _levelThumbnail;
        [SerializeField] private GameObject _starsCounter;
        [SerializeField] private TMP_Text _starsCounterText;
        [SerializeField] private Image _foreground;

        public Button ShowDetailedStatsButton => _showDetailedStatsButton;
        public GameObject StarsCounter => _starsCounter;
        public Image Foreground => _foreground;
        
        public void SetLevelThumbnail(Sprite thumbnail) => _levelThumbnail.sprite = thumbnail;

        public void SetStarsCounterText(string text) => _starsCounterText.text = text;
    }
}
