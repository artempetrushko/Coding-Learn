using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts
{
    public class LevelStatsCardData
    {
        public readonly Sprite Thumbnail;
        public readonly int StarsCurrentCount;
        public readonly int StarsTotalCount;
        public readonly UnityAction CardPressedAction;

        public LevelStatsCardData(Sprite thumbnail, int starsCurrentCount, int starsTotalCount, UnityAction cardPressedAction)
        {
            Thumbnail = thumbnail;
            StarsCurrentCount = starsCurrentCount;
            StarsTotalCount = starsTotalCount;
            CardPressedAction = cardPressedAction;
        }
    }
}
