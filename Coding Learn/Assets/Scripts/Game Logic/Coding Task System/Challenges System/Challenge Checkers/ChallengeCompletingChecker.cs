using UnityEngine;

namespace GameLogic
{
    public abstract class ChallengeCompletingChecker : ScriptableObject
    {
        public abstract bool IsCompleted(CodingTaskModel codingTaskModel);
    }
}
