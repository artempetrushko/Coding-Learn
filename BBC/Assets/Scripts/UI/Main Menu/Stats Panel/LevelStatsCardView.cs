using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
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
            starsCounterText.text = string.Format(@"{0}/{1}", data.StarsCurrentCount, data.StarsTotalCount);
            showDetailedStatsButton.onClick.AddListener(data.CardPressedAction);
        }
    }
}
