using UnityEngine;
using UnityEngine.UI;

namespace UI.MainMenu
{
    public class StatisticsMenuView : MonoBehaviour
    {
        [SerializeField] private Button _backToPreviousPageButton;
        [SerializeField] private Button _closeViewButton;
        [SerializeField] private GameObject _levelStatisticsCardsContainer;
        [SerializeField] private GameObject _detailedLevelStatisticsPagesContainer;

        public Button BackToPreviousPageButton => _backToPreviousPageButton;
        public Button CloseViewButton => _closeViewButton;
        public GameObject LevelStatisticsCardsContainer => _levelStatisticsCardsContainer;
        public GameObject DetailedLevelStatisticsPagesContainer => _detailedLevelStatisticsPagesContainer;

        public void SetActive(bool isActive) => gameObject.SetActive(isActive);

        public float GetSectionHeight() => GetComponent<RectTransform>().rect.height;

        public void SetBackToPreviousPageButtonActive(bool isActive) => _backToPreviousPageButton.gameObject.SetActive(isActive);  
    }
}
