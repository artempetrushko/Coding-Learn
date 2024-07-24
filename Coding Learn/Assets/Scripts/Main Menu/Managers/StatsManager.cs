using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace Scripts
{
    public class StatsManager : IMainMenuSectionManager
    {
        private StatsSectionController _statsSectionController;

        public StatsManager(StatsSectionController statsSectionController)
        {
            _statsSectionController = statsSectionController;
        }

        public async UniTask ShowSectionAsync()
        {
            await _statsSectionController.ChangeVisibilityAsync(true);
        }

        public async UniTask HideSectionAsync()
        {
            await _statsSectionController.ChangeVisibilityAsync(false);
        }

        public void CreateLevelStatsCards()
        {
            _statsSectionController.CreateLevelStatsCards(GetAllStatsCardDatas());
        }

        private List<LevelStatsCardData> GetAllStatsCardDatas()
        {
            var availableLevelsCount = SaveManager.GameProgressData.AllChallengeStatuses.Count(x => x.TasksChallengesResults != null && x.TasksChallengesResults.Count > 0);
            var statsCardDatas = new List<LevelStatsCardData>();
            for (var i = 1; i <= availableLevelsCount; i++)
            {
                /*var levelNumber = i;
                var levelCardThumbnail = MainMenuContentManager.GetLoadingScreen(levelNumber);
                var (totalChallengesCount, completedChallengesCount) = GetLevelChallengesData(levelNumber);
                statsCardDatas.Add(new LevelStatsCardData(levelCardThumbnail, completedChallengesCount, totalChallengesCount, 
                                                                () => statsSectionView.ShowDetalizedLevelStats(GetDetailedTasksStats(levelNumber))));*/
            }
            return statsCardDatas;
        }

        private (int totalChallengesCount, int completedChallengesCount) GetLevelChallengesData(int levelNumber)
        {
            var totalChallengesCount = 0;// MainMenuContentManager.GetLevelTaskInfos(levelNumber).Sum(task => task.Challenges.Challenges.Length);
            var completedChallengesCount = 0;
            var tasksChallengesResults = SaveManager.GameProgressData.AllChallengeStatuses[levelNumber - 1].TasksChallengesResults;
            if (tasksChallengesResults != null && tasksChallengesResults.Count > 0)
            {
                completedChallengesCount = tasksChallengesResults.Sum(taskChallengeStatuses => taskChallengeStatuses.ChallengeCompletingStatuses.Count(status => status));
            }      
            return (totalChallengesCount, completedChallengesCount);
        }

        private List<TaskStatsData> GetDetailedTasksStats(int levelNumber)
        {
            var taskChallengesInfos = SaveManager.GameProgressData.AllChallengeStatuses[levelNumber - 1];
            var taskStatsDatas = new List<TaskStatsData>();
            if (taskChallengesInfos.TasksChallengesResults != null && taskChallengesInfos.TasksChallengesResults.Count > 0)
            {
                for (var i = 1; i <= taskChallengesInfos.TasksChallengesResults.Count; i++)
                {
                    var (totalChallengesCount, completedChallengesCount) = GetTaskChallengesData(levelNumber, i);
                    //taskStatsDatas.Add(new TaskStatsData(MainMenuContentManager.GetTaskInfo(levelNumber, i).Title, completedChallengesCount, totalChallengesCount));
                }
            }           
            return taskStatsDatas;
        }

        private (int totalChallengesCount, int completedChallengesCount) GetTaskChallengesData(int levelNumber, int taskNumber)
        {
            var totalChallengesCount = 0;// MainMenuContentManager.GetTaskInfo(levelNumber, taskNumber).ChallengeInfos.Length;
            var completedChallengesCount = SaveManager.GameProgressData.AllChallengeStatuses[levelNumber - 1].TasksChallengesResults[taskNumber - 1].ChallengeCompletingStatuses.Count(status => status);
            return (totalChallengesCount, completedChallengesCount);
        }
    }
}
