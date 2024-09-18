using UnityEngine;

namespace GameLogic
{
    [CreateAssetMenu(fileName = "Completing Time Challenge Checker", menuName = "Game Configs/Challenges/Checkers/Completing Time Challenge Checker")]
    public class CompletingTimeChallengeChecker : ChallengeCompletingChecker
    {
        [SerializeField] private float _requiredCompletingTimeInMinutes;

        public override bool IsCompleted(CodingTaskModel codingTaskModel)
        {
            return (float)codingTaskModel.TaskCompletingTimeInSeconds / 60 < _requiredCompletingTimeInMinutes;
        }
    }
}
