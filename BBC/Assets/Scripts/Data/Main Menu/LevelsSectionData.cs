using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    [CreateAssetMenu(fileName = "Levels Panel Data", menuName = "Game Data/UI/Levels Panel Data", order = 1)]
    public class LevelsSectionData : ScriptableObject
    {
        [SerializeField]
        private List<Sprite> loadingScreens = new List<Sprite>();
        [Header("÷вета кнопок выбора уровн€")]
        [SerializeField]
        private Color levelButtonNormalColor;
        [SerializeField]
        private Color levelButtonSelectedColor;

        public List<Sprite> LoadingScreens => loadingScreens;
        public Color LevelButtonNormalColor => levelButtonNormalColor;
        public Color LevelButtonSelectedColor => levelButtonSelectedColor;
    }
}
