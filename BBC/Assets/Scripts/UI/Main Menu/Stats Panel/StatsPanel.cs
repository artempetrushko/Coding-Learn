using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class StatsPanel : MonoBehaviour
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

        public void ReturnToLevelStatsCards()
        {
            gameObject.GetComponent<Animator>().Play("HideDetalizedLevelStats");
            backToPreviousPageButton.gameObject.SetActive(false);
        }

        public void CreateLevelStatsCards(List<LevelStatsCardData> cardDatas)
        {
            foreach (var cardData in cardDatas) 
            {
                var levelCard = Instantiate(levelStatsCardPrefab, levelStatsCards.transform);
                levelCard.SetInfo(cardData);
            }
        }

        public void ShowDetalizedLevelStats(int levelNumber)
        {
            CreateDetalizedLevelStats(levelNumber);
            backToPreviousPageButton.gameObject.SetActive(true);
            gameObject.GetComponent<Animator>().Play("ShowDetalizedLevelStats");
        }

        private void CreateDetalizedLevelStats(int levelNumber)
        {
            for (var i = detalizedLevelStats.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(detalizedLevelStats.transform.GetChild(i).gameObject);
            }
            var tasksChallenges = SaveManager.SaveData.ChallengeCompletingStatuses[levelNumber - 1];
            for (var i = 1; i <= tasksChallenges.Count; i++)
            {
                var taskStats = Instantiate(taskStatsPrefab, detalizedLevelStats.transform);
                taskStats.transform.GetChild(0).GetComponent<Text>().text = ResourcesData.TaskTexts[levelNumber - 1][i - 1].Title;
                //taskStats.transform.GetChild(2).GetComponent<Text>().text = GetCompletedChallengesCount(levelNumber, i) + "/" + ResourcesData.TaskChallenges[levelNumber - 1][i - 1].Length;
            }
        }
    }
}
