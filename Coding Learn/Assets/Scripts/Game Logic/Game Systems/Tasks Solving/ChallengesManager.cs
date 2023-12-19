using System;
using System.Collections;
using System.Collections.Generic;
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
            StartCoroutine(rewardingSectionView.ShowChallengesResults_COR(challengeDatas));  
        }

        public void HideRewardingSection() => StartCoroutine(HideRewardingSection_COR());

        public void StartChallengeTimer() => StartCoroutine(StartChallengeTimer_COR());

        public void StopChallengeTimer() => isTimerStopped = true;

        public void IncreaseUsedTipsCountByOne() => usedTipsCount++;

        private IEnumerator StartChallengeTimer_COR()
        {
            challengeCompletingTime = 0;
            isTimerStopped = false;
            while (!isTimerStopped)
            {
                yield return new WaitForSeconds(1f);
                challengeCompletingTime++;
            }
        }

        private bool IsChallengeCompleting(ChallengeInfo challenge)
        {
            return challenge.Type switch
            {
                ChallengeType.SolveTask => true,
                ChallengeType.NoTips => usedTipsCount == 0,
                ChallengeType.CompletingTimeLessThan => challengeCompletingTime < Convert.ToInt32(challenge.CheckValue)
            };
        }

        private IEnumerator HideRewardingSection_COR()
        {
            yield return StartCoroutine(rewardingSectionView.HideChallengesResults_COR());
            onChallengesCompletingChecked.Invoke();
        }
    }
}
