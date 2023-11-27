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

        private void Start()
        {
            statsSectionView.CreateLevelStatsCards(GetAllStatsCardDatas());
        }

        private List<LevelStatsCardData> GetAllStatsCardDatas()
        {
            var availableLevelsCount = MainMenuSaveManager.SaveData.ChallengeCompletingStatuses.Count;
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
            var completedChallengesCount = MainMenuSaveManager.SaveData.ChallengeCompletingStatuses[levelNumber - 1].Sum(taskChallengeStatuses => taskChallengeStatuses.Where(status => status).Count());
            return (totalChallengesCount, completedChallengesCount);
        }

        private (int totalCount, int completedCount) GetTaskChallengesData(int levelNumber, int taskNumber)
        {
            var totalChallengesCount = MainMenuContentManager.GetTaskInfo(levelNumber, taskNumber).ChallengeInfos.Length;
            var completedChallengesCount = MainMenuSaveManager.SaveData.ChallengeCompletingStatuses[levelNumber - 1][taskNumber - 1].Where(status => status).Count();
            return (totalChallengesCount, completedChallengesCount);
        }

        private List<TaskStatsData> GetDetailedTasksStats(int levelNumber)
        {
            var taskChallengesInfos = MainMenuSaveManager.SaveData.ChallengeCompletingStatuses[levelNumber - 1];
            var taskStatsDatas = new List<TaskStatsData>();
            for (var i = 1; i <= taskChallengesInfos.Count; i++)
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
