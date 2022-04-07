using System;
using System.Collections.Generic;
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
        [SerializeField] private GameObject challengeStatsPrefab;

        private List<TaskText[]> tasks = new List<TaskText[]>();
        private List<List<Challenges[]>> challenges = new List<List<Challenges[]>>();

        public void ReturnToLevelStatsCards()
        {
            gameObject.GetComponent<Animator>().Play("HideDetalizedLevelStats");
            backToPreviousPageButton.gameObject.SetActive(false);
        }

        private void CreateLevelStatsCards()
        {
            var availableLevelsCount = PlayerPrefs.HasKey("LastAvailableLevelNumber") ? PlayerPrefs.GetInt("LastAvailableLevelNumber") : 1;
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
                Destroy(detalizedLevelStats.transform.GetChild(i).gameObject);
            var tasksChallenges = challenges[levelNumber - 1];
            for (var i = 0; i < tasksChallenges.Count; i++)
            {
                var taskStats = Instantiate(taskStatsPrefab, detalizedLevelStats.transform);
                taskStats.transform.GetChild(0).GetComponent<Text>().text = tasks[levelNumber - 1][i].Title;
                taskStats.transform.GetChild(2).GetComponent<Text>().text = GetCompletedChallengesCount(levelNumber, i) + "/3";
            }
        }

        private Tuple<int, int> GetCompletedAndTotalChallengesCount(int levelNumber)
        {
            var totalChallengesCount = tasks[levelNumber].Length * 3;
            var completedChallengesCount = 0;
            for (var i = 1; i <= tasks[levelNumber].Length; i++)
                completedChallengesCount += GetCompletedChallengesCount(levelNumber, i);
            return Tuple.Create(completedChallengesCount, totalChallengesCount);
        }

        private int GetCompletedChallengesCount(int levelNumber, int taskNumber)
        {
            var completedChallengesCount = 0;
            for (var i = 1; i <= 3; i++)
            {
                var completedChallengeSaveKey = "Level " + levelNumber + " Task " + taskNumber + " Challenge " + i + " completed";
                if (PlayerPrefs.HasKey(completedChallengeSaveKey))
                    completedChallengesCount += PlayerPrefs.GetInt(completedChallengeSaveKey);
            }
            return completedChallengesCount;
        }

        private void GetDataResources()
        {
            var currentLanguage = PlayerPrefs.HasKey("Language") ? (Language)PlayerPrefs.GetInt("Language") : Language.EN;
            for (var i = 1; i <= PlayerPrefs.GetInt("LastAvailableLevelNumber"); i++)
            {
                var challengesFiles = Resources.LoadAll<TextAsset>("Data/" + currentLanguage.ToString() + "/Challenges/Level " + i);
                var taskChallenges = new List<Challenges[]>();
                for (var j = 0; j < challengesFiles.Length; j++)
                    taskChallenges.Add(JsonHelper.FromJson<Challenges>(challengesFiles[j].text));
                challenges.Add(taskChallenges);

                var tasksFiles = Resources.LoadAll<TextAsset>("Data/" + currentLanguage.ToString() + "/Tasks");
                for (var j = 0; j < tasksFiles.Length; j++)
                    tasks.Add(JsonHelper.FromJson<TaskText>(tasksFiles[j].text));
            }
        }

        private void Awake()
        {
            GetDataResources();
            CreateLevelStatsCards();           
        }
    }
}
