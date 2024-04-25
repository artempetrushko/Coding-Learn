using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Scripts
{
    public class GameTaskManager : MonoBehaviour
    {
        [SerializeField]
        private CodingTrainingManager codingTrainingManager;
        [SerializeField]
        private DevEnvironmentManager devEnvironmentManager;
        [SerializeField]
        private ChallengesManager challengesManager;
        [SerializeField]
        private TipsManager tipsManager;
        [SerializeField]
        private HandbookManager handbookManager;
        [Space, SerializeField]
        private TaskSectionView taskSectionView;
        [SerializeField]
        private TaskDescriptionSectionView taskDescriptionSectionView;

        private int currentTaskNumber;
        private bool isTaskStarted;

        public void LoadNewTask()
        {
            currentTaskNumber++;
            SetTaskInfo();
            challengesManager.Initialize(currentTaskNumber);
            tipsManager.Initialize(currentTaskNumber);
            handbookManager.Initialize(currentTaskNumber);
            codingTrainingManager.ShowTrainingContent(GameManager.CurrentLevelNumber, currentTaskNumber);
        }

        public void ShowTaskContent()
        {
            if (!isTaskStarted)
            {
                isTaskStarted = true;
                challengesManager.StartChallengeTimer();
            }
            _ = taskSectionView.ChangeMainContentVisibilityAsync(true);
        }

        public void ReturnToCodingTraining(CodingTrainingInfo[] codingTrainingInfos)
        {
            UniTask.Void(async () =>
            {
                await taskSectionView.ChangeMainContentVisibilityAsync(false);
                codingTrainingManager.ShowTrainingContent(codingTrainingInfos);
            });
        }

        public void FinishTask() => _ = ProcessTaskResultsAsync(false);

        public void SkipTask() => _ = ProcessTaskResultsAsync(true);

        private async UniTask ProcessTaskResultsAsync(bool isTaskSkipped)
        {
            isTaskStarted = false;
            await taskSectionView.ChangeMainContentVisibilityAsync(false);
            challengesManager.CheckChallengesCompleting(currentTaskNumber, isTaskSkipped);
        }

        private void SetTaskInfo()
        {
            var currentTaskInfo = GameContentManager.GetTaskInfo(currentTaskNumber);
            taskDescriptionSectionView.SetContent(currentTaskInfo.Title, currentTaskInfo.Description);
            devEnvironmentManager.SetCurrentTaskInfo(currentTaskInfo.StartCode, currentTaskInfo.TestInfo);
        }
    }
}
