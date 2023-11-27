using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scripts
{
    public class ChallengesManager : MonoBehaviour
    {
        [SerializeField]
        private PadChallengesScreenView padChallengesScreenView;
        [SerializeField]
        private RewardingSectionView rewardingSectionView;

        private int challengeCompletingTime;
        private bool isTimerStopped;

        public void SetChallengeInfos(int currentTaskNumber)
        {
            var challengeInfos = GameContentManager.GetTaskInfo(currentTaskNumber).ChallengeInfos
                .Select(challenge => challenge.Description)
                .ToList();
            padChallengesScreenView.CreateNewChallengeViews(challengeInfos);
        }

        public void CheckChallengesCompleting(int currentTaskNumber, bool isTaskSkipped = false)
        {
            var challengeDatas = GameContentManager.GetTaskInfo(currentTaskNumber).ChallengeInfos
                                    .Select(challenge => (description: challenge.Description, isCompleted: !isTaskSkipped && IsChallengeCompleting(challenge)))
                                    .ToList();
            //SaveManager.SaveTemporaryChallengeProgress(i + 1, isChallengeCompleted);

            StartCoroutine(rewardingSectionView.ShowChallengesResults_COR(challengeDatas));  
        }

        public void StartChallengeTimer() => StartCoroutine(StartChallengeTimer_COR());

        public void StopChallengeTimer() => isTimerStopped = true;

        public IEnumerator StartChallengeTimer_COR()
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
                ChallengeType.SolveTask => (bool)challenge.CheckValue,
                ChallengeType.NoTips => (bool)challenge.CheckValue, //TODO: сравнивать со значением из TipsManager
                ChallengeType.CompletingTimeLessThan => challengeCompletingTime < (int)challenge.CheckValue
            };
        }
    }
}
