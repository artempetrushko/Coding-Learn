using Cysharp.Threading.Tasks;

namespace Scripts
{
    public class GameTaskManager
    {
        private ChallengeManager challengesController;
        private TaskSectionView taskSectionView;
        private TaskDescriptionSectionView taskDescriptionSectionView;

        public GameTaskManager(ChallengeManager challengesController, TaskSectionView taskSectionView, TaskDescriptionSectionView taskDescriptionSectionView)
        {
            this.challengesController = challengesController;
            this.taskSectionView = taskSectionView;
            this.taskDescriptionSectionView = taskDescriptionSectionView;
        }

        public void LoadNewTask(TaskContent taskContent)
        {
            SetCurrentTaskContent(taskContent);
            taskDescriptionSectionView.SetContent(taskContent.Title.GetLocalizedString(), taskContent.Description.GetLocalizedString());
        }

        public async UniTask ShowTaskContent() => await taskSectionView.ChangeMainContentVisibilityAsync(true);

        public async UniTask HideTaskContent() => await taskSectionView.ChangeMainContentVisibilityAsync(false);

        public void FinishTask() => _ = ProcessTaskResultsAsync(false);

        public void SkipTask() => _ = ProcessTaskResultsAsync(true);

        private async UniTask ProcessTaskResultsAsync(bool isTaskSkipped)
        {
            await HideTaskContent();
            challengesController.CheckCurrentChallengesCompleting(isTaskSkipped);
        }



        private RewardingSectionView rewardingSectionView;

        public async UniTask ShowChallengesResults((string description, bool isCompleted)[] challengeResults) => await rewardingSectionView.ShowChallengesResultsAsync(challengeResults);

        public void HideChallengesResults()
        {
            UniTask.Void(async () =>
            {
                await rewardingSectionView.HideChallengesResultsAsync();
                //OnChallengesCompletingChecked?.Invoke();
            });
        }
    }
}
