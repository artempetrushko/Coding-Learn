using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Scripts
{
    public class GameTaskManager
    {
        private ChallengeManager _challengeManager;
        private DevEnvironmentSectionController _devEnvironmentSectionController;
        private RewardingSectionController _rewardingSectionController;
        private TaskDescriptionSectionController _taskDescriptionSectionController;

        public GameTaskManager(ChallengeManager challengesManager, TaskDescriptionSectionController taskDescriptionSectionController)
        {
            _challengeManager = challengesManager;
            _taskDescriptionSectionController = taskDescriptionSectionController;
        }

        public void LoadNewTask(TaskContent taskContent)
        {
            _taskDescriptionSectionController.SetContent(taskContent.Title.GetLocalizedString(), taskContent.Description.GetLocalizedString());
        }

        public async UniTask ShowTaskContent() { } //await _taskSectionController.ChangeMainContentVisibilityAsync(true);

        public async UniTask HideTaskContent() { } //await _taskSectionController.ChangeMainContentVisibilityAsync(false);

        public void FinishTask() => _ = ProcessTaskResultsAsync(false);

        public void SkipTask() => _ = ProcessTaskResultsAsync(true);

        private async UniTask ProcessTaskResultsAsync(bool isTaskSkipped)
        {
            await HideTaskContent();
            _challengeManager.CheckCurrentChallengesCompleting(isTaskSkipped);
        }


        public async UniTask ShowChallengesResults((string description, bool isCompleted)[] challengeResults) => await _rewardingSectionController.ShowChallengesResultsAsync(challengeResults);

        public void HideChallengesResults()
        {
            UniTask.Void(async () =>
            {
                await _rewardingSectionController.HideChallengesResultsAsync();
                //OnChallengesCompletingChecked?.Invoke();
            });
        }


        public async UniTask ChangeMainContentVisibilityAsync(bool isVisible)
        {
            var movementOffsetXSign = isVisible ? 1f : -1f;
            var padViewRightMargin = 20;

            await _taskDescriptionSectionController.SetVisibilityAsync(isVisible);

            //padSectionView.transform.DOLocalMoveX(padSectionView.transform.localPosition.x - (padSectionView.GetComponent<RectTransform>().sizeDelta.x + padViewRightMargin) * movementOffsetXSign, 1f);
            await UniTask.WaitForSeconds(1);
        }
    }
}
