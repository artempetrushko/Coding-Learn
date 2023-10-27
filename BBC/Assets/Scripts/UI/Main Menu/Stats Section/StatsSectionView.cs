using System;
using System.Collections.Generic;
using System.Linq;
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

        public void CreateLevelStatsCards(List<LevelStatsCardData> cardDatas)
        {
            foreach (var cardData in cardDatas) 
            {
                var levelCard = Instantiate(levelStatsCardPrefab, levelStatsCards.transform);
                levelCard.SetInfo(cardData);
            }
        }

        public void ShowDetalizedLevelStats(List<TaskStatsData> taskStatsDatas)
        {
            CreateDetalizedLevelStats(taskStatsDatas);
            backToPreviousPageButton.gameObject.SetActive(true);
            GetComponent<Animator>().Play("ShowDetalizedLevelStats");
        }

        public void ReturnToLevelStatsCards()
        {
            GetComponent<Animator>().Play("HideDetalizedLevelStats");
            backToPreviousPageButton.gameObject.SetActive(false);
        }

        private void CreateDetalizedLevelStats(List<TaskStatsData> taskStatsDatas)
        {
            DeletePreviousDetailedStats();
            foreach (var taskStatsData in taskStatsDatas)
            {
                var taskStats = Instantiate(taskStatsPrefab, detalizedLevelStats.transform);
                taskStats.SetInfo(taskStatsData);
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
