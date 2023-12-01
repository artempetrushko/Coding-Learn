using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scripts
{
    public class StatsManager : MainMenuSectionManager
    {
        [SerializeField]
        private StatsSectionView statsSectionView;

        public void CreateLevelStatsCards()
        {
            statsSectionView.CreateLevelStatsCards(GetAllStatsCardDatas());
        }

        private List<LevelStatsCardData> GetAllStatsCardDatas()
        {
            var availableLevelsCount = MainMenuSaveManager.SaveData.AllChallengeStatuses.Length;
            var statsCardDatas = new List<LevelStatsCardData>();
            for (var i = 1; i <= availableLevelsCount; i++)
            {
                var levelNumber = i;
                var levelCardThumbnail = MainMenuContentManager.GetLoadingScreen(levelNumber);
                var challengesData = GetLevelChallengesData(levelNumber);
                var taskStatsDatas = GetDetailedTasksStats(levelNumber);
                statsCardDatas.Add(new LevelStatsCardData(levelCardThumbnail, challengesData.completedCount, challengesData.totalCount, 
                                                                () => statsSectionView.ShowDetalizedLevelStats(taskStatsDatas)));
            }
            return statsCardDatas;
        }

        private (int totalCount, int completedCount) GetLevelChallengesData(int levelNumber)
        {
            var totalChallengesCount = MainMenuContentManager.GetLevelTaskInfos(levelNumber).Sum(task => task.ChallengeInfos.Length);
            var completedChallengesCount = MainMenuSaveManager.SaveData.AllChallengeStatuses[levelNumber - 1].TasksChallengesResults
                .Sum(taskChallengeStatuses => taskChallengeStatuses.ChallengeCompletingStatuses.Count(status => status));
            return (totalChallengesCount, completedChallengesCount);
        }

        private (int totalCount, int completedCount) GetTaskChallengesData(int levelNumber, int taskNumber)
        {
            var totalChallengesCount = MainMenuContentManager.GetTaskInfo(levelNumber, taskNumber).ChallengeInfos.Length;
            var completedChallengesCount = MainMenuSaveManager.SaveData.AllChallengeStatuses[levelNumber - 1].TasksChallengesResults[taskNumber - 1].ChallengeCompletingStatuses.Count(status => status);
            return (totalChallengesCount, completedChallengesCount);
        }

        private List<TaskStatsData> GetDetailedTasksStats(int levelNumber)
        {
            var taskChallengesInfos = MainMenuSaveManager.SaveData.AllChallengeStatuses[levelNumber - 1];
            var taskStatsDatas = new List<TaskStatsData>();
            for (var i = 1; i <= taskChallengesInfos.TasksChallengesResults.Length; i++)
            {
                var taskChallengesData = GetTaskChallengesData(levelNumber, i);
                taskStatsDatas.Add(new TaskStatsData(MainMenuContentManager.GetTaskInfo(levelNumber, i).Title, taskChallengesData.completedCount, taskChallengesData.totalCount));
            }
            return taskStatsDatas;
        }

        public override IEnumerator ShowSectionView_COR()
        {
            yield return StartCoroutine(statsSectionView.ChangeVisibility_COR(true));
        }

        public override IEnumerator HideSectionView_COR()
        {
            yield return StartCoroutine(statsSectionView.ChangeVisibility_COR(false));
        }
    }
}
