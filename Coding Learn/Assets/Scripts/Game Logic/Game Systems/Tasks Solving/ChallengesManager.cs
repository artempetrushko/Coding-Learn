using Cysharp.Threading.Tasks;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts
{
    public class ChallengesManager : PadFunctionManager
    {
        [SerializeField]
        private PadChallengesScreenView padChallengesScreenView;
        [SerializeField]
        private RewardingSectionView rewardingSectionView;
        [Space, SerializeField]
        private UnityEvent onChallengesCompletingChecked;

        private TaskChallengesResults currentTaskChallengesResults;
        private bool isTimerStopped;
        private int challengeCompletingTime;
        private int usedTipsCount;

        public override void Initialize(int currentTaskNumber)
        {
            var challengeInfos = GameContentManager.GetTaskInfo(currentTaskNumber).ChallengeInfos;
            var challengeDescriptions = challengeInfos.Select(challenge => challenge.Description).ToList();
            padChallengesScreenView.CreateNewChallengeViews(challengeDescriptions);

            currentTaskChallengesResults = GameSaveManager.GetCurrentTaskChallengesResults(currentTaskNumber);
            currentTaskChallengesResults.ChallengeCompletingStatuses ??= new bool[challengeInfos.Length].ToList();
            usedTipsCount = 0;
        }

        public override void ShowModalWindow() => padChallengesScreenView.SetVisibility(true);

        public override void HideModalWindow() => padChallengesScreenView.SetVisibility(false);

        public void CheckChallengesCompleting(int currentTaskNumber, bool isTaskSkipped)
        {
            var challengeDatas = GameContentManager.GetTaskInfo(currentTaskNumber).ChallengeInfos
                                    .Select(challenge => (description: challenge.Description, isCompleted: !isTaskSkipped && IsChallengeCompleting(challenge)))
                                    .ToList();
            for (var i = 0; i < currentTaskChallengesResults.ChallengeCompletingStatuses.Count; i++)
            {
                if (challengeDatas[i].isCompleted && !currentTaskChallengesResults.ChallengeCompletingStatuses[i])
                {
                    currentTaskChallengesResults.ChallengeCompletingStatuses[i] = challengeDatas[i].isCompleted;
                }             
            }
            _ = rewardingSectionView.ShowChallengesResultsAsync(challengeDatas);  
        }

        public void HideRewardingSection()
        {
            UniTask.Void(async () =>
            {
                await rewardingSectionView.HideChallengesResultsAsync();
                onChallengesCompletingChecked.Invoke();
            });
        }

        public void StartChallengeTimer()
        {
            UniTask.Void(async () =>
            {
                challengeCompletingTime = 0;
                isTimerStopped = false;
                while (!isTimerStopped)
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(1));
                    challengeCompletingTime++;
                }
            });
        }

        public void StopChallengeTimer() => isTimerStopped = true;

        public void IncreaseUsedTipsCountByOne() => usedTipsCount++;

        private bool IsChallengeCompleting(ChallengeInfo challenge)
        {
            return challenge.Type switch
            {
                ChallengeType.SolveTask => true,
                ChallengeType.NoTips => usedTipsCount == 0,
                ChallengeType.CompletingTimeLessThan => challengeCompletingTime < Convert.ToInt32(challenge.CheckValue)
            };
        }
    }
}
