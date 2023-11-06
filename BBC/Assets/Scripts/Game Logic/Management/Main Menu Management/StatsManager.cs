using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scripts
{
    public class StatsManager : MonoBehaviour
    {
        [SerializeField]
        private StatsSectionView levelStatsPanel;

        private void Start()
        {
            levelStatsPanel.CreateLevelStatsCards(GetAllStatsCardDatas());
        }

        private List<LevelStatsCardData> GetAllStatsCardDatas()
        {
            var availableLevelsCount = SaveManager.SaveData.ChallengeCompletingStatuses.Count;
            var statsCardDatas = new List<LevelStatsCardData>();
            for (var i = 1; i <= availableLevelsCount; i++)
            {
                var levelNumber = i;
                var levelCardThumbnail = Resources.Load<Sprite>("Load Screens/LoadScreen_Level" + levelNumber);
                var completedAndTotalChallengesCount = GetCompletedAndTotalChallengesCount(levelNumber);
                var taskStatsDatas = GetDetailedTasksStats(levelNumber);
                statsCardDatas.Add(new LevelStatsCardData(levelCardThumbnail, completedAndTotalChallengesCount.Item1, completedAndTotalChallengesCount.Item2, 
                                                                () => levelStatsPanel.ShowDetalizedLevelStats(taskStatsDatas)));
            }
            return statsCardDatas;
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

        private List<TaskStatsData> GetDetailedTasksStats(int levelNumber)
        {
            var taskChallengesInfos = SaveManager.SaveData.ChallengeCompletingStatuses[levelNumber - 1];
            var taskStatsDatas = new List<TaskStatsData>();
            for (var i = 1; i <= taskChallengesInfos.Count; i++)
            {
                taskStatsDatas.Add(new TaskStatsData(ResourcesData.TaskTexts[levelNumber - 1][i - 1].Title, 
                                                     GetCompletedChallengesCount(levelNumber, i), 
                                                     ResourcesData.TaskChallenges[levelNumber - 1][i - 1].Length));
            }
            return taskStatsDatas;
        }
    }
}
