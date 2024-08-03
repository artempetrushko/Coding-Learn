using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class StatsSectionView : MonoBehaviour
    {
        [SerializeField] private Button _backToPreviousPageButton;
        [SerializeField] private GameObject _levelStatsCardsContainer;
        [SerializeField] private GameObject _detalizedLevelStatsContainer;

        public GameObject LevelStatsCardsContainer => _levelStatsCardsContainer;
        public GameObject DetalizedLevelStatsContainer => _detalizedLevelStatsContainer;

        public float GetSectionHeight() => GetComponent<RectTransform>().rect.height;

        public void SetBackToPreviousPageButtonActive(bool isActive) => _backToPreviousPageButton.gameObject.SetActive(isActive);  
    }
}
