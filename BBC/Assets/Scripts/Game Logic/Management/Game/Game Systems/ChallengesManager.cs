using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scripts
{
    public class ChallengesManager : MonoBehaviour
    {
        [SerializeField]
        private RewardingSectionView rewardingSectionView;

        private int challengeCompletingTime;
        private bool isTimerStopped;

        public void CheckChallengesCompleting(int currentTaskNumber, bool isTaskSkipped = false)
        {
            rewardingSectionView.gameObject.SetActive(true);
            //yield return StartCoroutine(PlayTimeline_COR(rewardingPanel, "ShowRewardingPanel"));

            var challengeDatas = ContentManager.TaskChallenges[GameManager.CurrentLevelNumber - 1][currentTaskNumber - 1]
                                    .Select(challenge => (description: challenge.Challenge, isCompleted: IsChallengeCompleting(challenge.CheckValue)))
                                    .ToList();
            StartCoroutine(rewardingSectionView.ShowChallengesResults_COR(challengeDatas));
            //SaveManager.SaveTemporaryChallengeProgress(i + 1, isChallengeCompleted);
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

        private bool IsChallengeCompleting(double checkValue)
        {
            switch (checkValue)
            {
                /*case 1:
                    return true;
                case 0:
                    return !gameManager.AvailableTipsData[gameManager.GetCurrentTaskNumber() - 1].IsShown;
                case 300:
                    return SpentTime <= checkValue;*/
                default:
                    return false;
            }
        }
    }
}
