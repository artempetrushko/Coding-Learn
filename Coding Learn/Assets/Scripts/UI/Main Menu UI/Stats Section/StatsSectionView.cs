using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class StatsSectionView : MonoBehaviour
    {
        [SerializeField] 
        private Button backToPreviousPageButton;
        [SerializeField] 
        private GameObject levelStatsCards;
        [SerializeField] 
        private GameObject detalizedLevelStats;
        [SerializeField] 
        private LevelStatsCardView levelStatsCardPrefab;
        [SerializeField] 
        private TaskStatsView taskStatsPrefab;
        [Space, SerializeField]
        private StatsSectionAnimator animator;

        public async UniTask ChangeVisibilityAsync(bool isVisible)
        {
            backToPreviousPageButton.gameObject.SetActive(false);
            await animator.ChangeVisibilityAsync(isVisible);
        }

        public void CreateLevelStatsCards(List<LevelStatsCardData> cardDatas)
        {
            foreach (var cardData in cardDatas) 
            {
                var levelCard = Instantiate(levelStatsCardPrefab, levelStatsCards.transform);
                levelCard.SetContent(cardData);
            }
        }

        public void ShowDetalizedLevelStats(List<TaskStatsData> taskStatsDatas)
        {
            CreateDetalizedLevelStats(taskStatsDatas);
            backToPreviousPageButton.gameObject.SetActive(true);
            _ = animator.ShowDetailedLevelStatsAsync();
        }

        public void ReturnToLevelStatsCards()
        {
            backToPreviousPageButton.gameObject.SetActive(false);
            _ = animator.ReturnToLevelCardsAsync();
        }

        private void CreateDetalizedLevelStats(List<TaskStatsData> taskStatsDatas)
        {
            DeletePreviousDetailedStats();
            foreach (var taskStatsData in taskStatsDatas)
            {
                var taskStats = Instantiate(taskStatsPrefab, detalizedLevelStats.transform);
                taskStats.SetContent(taskStatsData);
            }
        }

        private void DeletePreviousDetailedStats()
        {
            for (var i = detalizedLevelStats.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(detalizedLevelStats.transform.GetChild(i).gameObject);
            }
            detalizedLevelStats.transform.DetachChildren();
        }
    }
}
