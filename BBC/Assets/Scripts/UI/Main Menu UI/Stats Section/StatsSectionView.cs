using System;
using System.Collections;
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
        [Space, SerializeField]
        private StatsSectionAnimator animator;

        public IEnumerator ChangeVisibility_COR(bool isVisible)
        {
            backToPreviousPageButton.gameObject.SetActive(false);
            yield return StartCoroutine(animator.ChangeVisibility_COR(isVisible));
        }

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
            StartCoroutine(animator.ShowDetailedLevelStats_COR());
        }

        public void ReturnToLevelStatsCards()
        {
            backToPreviousPageButton.gameObject.SetActive(false);
            StartCoroutine(animator.ReturnToLevelCards_COR());
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
