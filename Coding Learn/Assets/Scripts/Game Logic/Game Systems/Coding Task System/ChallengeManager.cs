using Cysharp.Threading.Tasks;
using System;
using System.Linq;

namespace Scripts
{
    public class ChallengeManager : IPadSecondaryFunction
    {
        public event Action ChallengesCompletingChecked;

        private ChallengesSectionController _challengesSectionController;
        private ChallengesData currentChallengesData;
        private TaskChallengesResults currentTaskChallengesResults;
        private bool isTimerStopped;
        private int challengeCompletingTime;
        private int usedTipsCount;

        public ChallengeManager(ChallengesSectionController challengesSectionController)
        {
            _challengesSectionController = challengesSectionController;
        }

        public void SetChallengeData(ChallengesData challengesData)
        {
            currentChallengesData = challengesData;
            var challengeDescriptions = currentChallengesData.Challenges.Select(challenge => challenge.Description.GetLocalizedString()).ToList();
            _challengesSectionController.CreateNewChallengeViews(challengeDescriptions);

            //currentTaskChallengesResults = GameSaveManager.GetCurrentTaskChallengesResults(currentTaskNumber);
            currentTaskChallengesResults.ChallengeCompletingStatuses ??= new bool[currentChallengesData.Challenges.Length].ToList();
            usedTipsCount = 0;
        }

        public void ShowModalSection() => _challengesSectionController.SetVisibility(true);

        public void HideModalSection() => _challengesSectionController.SetVisibility(false);

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
                    await UniTask.WaitForSeconds(1);
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
