using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.MainMenu
{
    public class LevelStatisticsCardView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public event Action<LevelStatisticsCardView> PointerEnter;
        public event Action<LevelStatisticsCardView> PointerExit;

        [SerializeField] private Button _showDetailedStatisticssButton;
        [SerializeField] private Image _levelThumbnail;
        [SerializeField] private GameObject _starsCounter;
        [SerializeField] private TMP_Text _starsCounterText;
        [SerializeField] private Image _foreground;

        public Button ShowDetailedStatisticsButton => _showDetailedStatisticssButton;
        public GameObject StarsCounter => _starsCounter;
        public Image Foreground => _foreground;

		public void OnPointerEnter(PointerEventData eventData) => PointerEnter?.Invoke(this);

		public void OnPointerExit(PointerEventData eventData) => PointerExit?.Invoke(this);

		public void SetLevelThumbnail(Sprite thumbnail) => _levelThumbnail.sprite = thumbnail;

        public void SetStarsCounterText(string text) => _starsCounterText.text = text;
    }
}
