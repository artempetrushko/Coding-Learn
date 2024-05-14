using Cysharp.Threading.Tasks;

namespace Scripts
{
    public class GameTaskController
    {
        private PadController padController;
        private ChallengesController challengesController;
        private TaskSectionView taskSectionView;
        private TaskDescriptionSectionView taskDescriptionSectionView;

        public GameTaskController(PadController padController, ChallengesController challengesController, TaskSectionView taskSectionView, TaskDescriptionSectionView taskDescriptionSectionView)
        {
            this.padController = padController;
            this.challengesController = challengesController;
            this.taskSectionView = taskSectionView;
            this.taskDescriptionSectionView = taskDescriptionSectionView;
        }

        public void LoadNewTask(TaskContent taskContent)
        {
            padController.SetCurrentTaskContent(taskContent);
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
    }
}
