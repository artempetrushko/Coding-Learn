using UnityEngine;

namespace GameLogic
{
    [CreateAssetMenu(fileName = "No Tips Challenge Checker", menuName = "Game Configs/Challenges/Checkers/No Tips Challenge Checker")]
    public class NoTipsChallengeChecker : ChallengeCompletingChecker
    {
        public override bool IsCompleted(CodingTaskModel codingTaskModel)
        {
            return codingTaskModel.UsedTipsCount == 0;
        }
    }
}
