using Cysharp.Threading.Tasks;
using System;
using System.Linq;
using UnityEngine;

namespace Scripts
{
    public class ChallengesController : IPadSecondaryFunction
    {
        public event Action OnChallengesCompletingChecked;

        private PadChallengesScreenView padChallengesScreenView;
        private ChallengesData currentChallengesData;
        private TaskChallengesResults currentTaskChallengesResults;
        private bool isTimerStopped;
        private int challengeCompletingTime;
        private int usedTipsCount;

        public ChallengesController(PadChallengesScreenView padChallengesScreenView)
        {
            this.padChallengesScreenView = padChallengesScreenView;
        }

        public void SetChallengeData(ChallengesData challengesData)
        {
            currentChallengesData = challengesData;
            var challengeDescriptions = currentChallengesData.Challenges.Select(challenge => challenge.Description).ToList();
            //padChallengesScreenView.CreateNewChallengeViews(challengeDescriptions);

            //currentTaskChallengesResults = GameSaveManager.GetCurrentTaskChallengesResults(currentTaskNumber);
            currentTaskChallengesResults.ChallengeCompletingStatuses ??= new bool[currentChallengesData.Challenges.Length].ToList();
            usedTipsCount = 0;
        }

        public void ShowModalWindow() => padChallengesScreenView.SetVisibility(true);

        public void HideModalWindow() => padChallengesScreenView.SetVisibility(false);

        public void CheckCurrentChallengesCompleting(bool isTaskSkipped)
        {
            var challengeDatas = currentChallengesData.Challenges
                                    .Select(challenge => (description: challenge.Description, isCompleted: !isTaskSkipped && IsChallengeCompleting(challenge)))
                                    .ToList();
            for (var i = 0; i < currentTaskChallengesResults.ChallengeCompletingStatuses.Count; i++)
            {
                if (challengeDatas[i].isCompleted && !currentTaskChallengesResults.ChallengeCompletingStatuses[i])
                {
                    currentTaskChallengesResults.ChallengeCompletingStatuses[i] = challengeDatas[i].isCompleted;
                }             
            }
            
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

        private bool IsChallengeCompleting(Challenge challenge)
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
