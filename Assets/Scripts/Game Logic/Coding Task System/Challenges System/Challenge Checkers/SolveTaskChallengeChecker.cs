using UnityEngine;

namespace GameLogic
{
    [CreateAssetMenu(fileName = "Solve Task Challenge Checker", menuName = "Game Configs/Challenges/Checkers/Solve Task Challenge Checker")]
    public class SolveTaskChallengeChecker : ChallengeCompletingChecker
    {
        public override bool IsCompleted(CodingTaskModel codingTaskModel)
        {
            return true;
        }
    }
}
