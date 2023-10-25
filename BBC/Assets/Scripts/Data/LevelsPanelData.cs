using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    [CreateAssetMenu(fileName = "Levels Panel Data", menuName = "Game Data/UI/Levels Panel Data", order = 1)]
    public class LevelsPanelData : ScriptableObject
    {
        [SerializeField]
        private List<Sprite> loadingScreens = new List<Sprite>();
        [Header("÷вета кнопок выбора уровн€")]
        [SerializeField]
        private Color levelButtonNormalColor;
        [SerializeField]
        private Color levelButtonHighlightedColor;
        [SerializeField]
        private Color levelButtonSelectedColor;
        [SerializeField]
        private Color levelButtonSelectedHighlightedColor;

        public List<Sprite> LoadingScreens => loadingScreens;
        public Color LevelButtonNormalColor => levelButtonNormalColor;
        public Color LevelButtonHighlightedColor => levelButtonHighlightedColor;
        public Color LevelButtonSelectedColor => levelButtonSelectedColor;
        public Color LevelButtonSelectedHighlightedColor => levelButtonSelectedHighlightedColor;
    }
}
