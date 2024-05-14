using System;
using UnityEngine;
using UnityEngine.Localization;

namespace Scripts
{
    public enum ChallengeType
    {
        SolveTask,
        NoTips,
        CompletingTimeLessThan
    }

    [CreateAssetMenu(fileName = "Challenges Data", menuName = "Game Content/Challenges Data", order = 10)]
    public class ChallengesData : ScriptableObject
    {
        [field: SerializeField]
        public Challenge[] Challenges { get; private set; }
    }

    [Serializable]
    public class Challenge
    {
        [field: SerializeField]
        public ChallengeType Type { get; private set; }
        [field: SerializeField]
        public LocalizedString Description { get; private set; }
        [field: SerializeField]
        public string CheckValue { get; private set; }
    }
}
