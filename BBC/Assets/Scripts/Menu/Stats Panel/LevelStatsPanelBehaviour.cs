using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class LevelStatsPanelBehaviour : MonoBehaviour
    {
        [SerializeField] private Button backToPreviousPageButton;
        [SerializeField] private GameObject levelCards;
        [SerializeField] private GameObject detalizedLevelStats;
        [SerializeField] private GameObject levelStatsCardPrefab;
        [SerializeField] private GameObject taskStatsPrefab;

        public void ReturnToLevelStatsCards()
        {
            gameObject.GetComponent<Animator>().Play("HideDetalizedLevelStats");
            backToPreviousPageButton.gameObject.SetActive(false);
        }

        private void CreateLevelStatsCards()
        {
            var availableLevelsCount = SaveManager.SaveData.ChallengeCompletingStatuses.Count;
            for (var i = 1; i <= availableLevelsCount; i++)
            {
                var levelNumber = i;
                var levelCard = Instantiate(levelStatsCardPrefab, levelCards.transform);
                var completedAndTotalChallengesCount = GetCompletedAndTotalChallengesCount(levelNumber);
                levelCard.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Load Screens/LoadScreen_Level" + levelNumber);
                levelCard.GetComponentInChildren<Text>().text = completedAndTotalChallengesCount.Item1 + "/" + completedAndTotalChallengesCount.Item2;
                levelCard.GetComponentInChildren<Button>().onClick.AddListener(() => ShowDetalizedLevelStats(levelNumber));
            }
        }

        private void ShowDetalizedLevelStats(int levelNumber)
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
                taskStats.transform.GetChild(2).GetComponent<Text>().text = GetCompletedChallengesCount(levelNumber, i) + "/" + ResourcesData.TaskChallenges[levelNumber - 1][i - 1].Length;
            }
        }

        private Tuple<int, int> GetCompletedAndTotalChallengesCount(int levelNumber)
        {
            var totalChallengesCount = ResourcesData.TaskChallenges[levelNumber - 1].Sum(x => x.Length);
            var completedChallengesCount = 0;
            for (var i = 1; i <= SaveManager.SaveData.ChallengeCompletingStatuses[levelNumber - 1].Count; i++)
            {
                completedChallengesCount += GetCompletedChallengesCount(levelNumber, i);
            }
            return Tuple.Create(completedChallengesCount, totalChallengesCount);
        }

        private int GetCompletedChallengesCount(int levelNumber, int taskNumber)
        {
            var taskChallengesStatuses = SaveManager.SaveData.ChallengeCompletingStatuses[levelNumber - 1][taskNumber - 1];         
            return taskChallengesStatuses.Where(status => status).Count();
        }

        private void Start()
        {
            CreateLevelStatsCards();           
        }
    }
}
